using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.UI.Core;
using BluetoothListener.Lib.BeaconPackages;
using BluetoothListener.Lib.BluetoothAdvertisement;

namespace BluetoothListener.Lib
{
    public class BluetoothListenerManager:IDisposable
    {
        private readonly IViewData _data;
        private readonly CoreDispatcher _dispatcher;
        private readonly IBluetoothReceiver _bluetoothDevice;
        private bool _isActive;


        private static bool _busy;

        public BluetoothListenerManager(IViewData data, CoreDispatcher dispatcher)
        {
            _data = data;
            _dispatcher = dispatcher;
            _bluetoothDevice = new BluetoothReceiver();
            //_bluetoothDevice.AdvertisementReceived += PackageReceived;
            _isActive = false;
        }

        private async void PackageReceived(IBluetoothAdvertisementPackage args)
        {
            await RunWithDispatcher(() => { _data.Received++; });
            
            if (_busy)
            {
                await RunWithDispatcher(() => { _data.Dropped++; });
                
                Debug.WriteLine($"A Package was Dropped {_data.Dropped} from {_data.Received} ({_data.Dropped * 100.0 / (1.0 * _data.Received) }%)");
                return;
            }

            var beacon = BeaconBuilder.CreateBeaconDeviceFromBleAdvertisement(args);

            if (beacon.NumberOfPackages() == 0) return;

            if (beacon.RssiOutOfRange()) return;

            InsertOrReplaceBeaconInCollection(beacon);
        }

        private void InsertOrReplaceBeaconInCollection(IBluetoothBeacon beacon)
        {
            try
            {
                var devices = _data.Devices;
                _busy = true;

                var bluetoothAddress = beacon.BluetoothAddress;
                var found = devices.ContainsKey(bluetoothAddress);

                if (found)
                {
                    IBluetoothBeacon existingBeacon;

                    const int endlessLoopLimit = 40;
                    var tries = 0;
                    while (!devices.TryGetValue(bluetoothAddress, out existingBeacon))
                    {
                        if (tries++ > endlessLoopLimit) throw new Exception("Can't read value!");
                    }

                    beacon.CopyMissedPackagesFromBeacon(existingBeacon);
                    beacon.UpdatePackageCounterAndPeriodBetweenPackages(existingBeacon);
                    devices.Remove(bluetoothAddress);
                    existingBeacon = null;
                }
                devices.Add(bluetoothAddress, beacon);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Oooops, Lost Package {ex.Message}");
            }
            finally
            {
                _busy = false;
            }
        }

        private async Task RunWithDispatcher(DispatchedHandler agileCallback)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, agileCallback);
        }

        public async void Start()
        {
            if(_isActive) return;
            _isActive = true;
            _bluetoothDevice.AdvertisementReceived += PackageReceived;
            await RunWithDispatcher(() =>
            {
                _data.ClearData();
                _data.Mode = "Running";
            });
            _bluetoothDevice.StartListening();
        }

        public async void Stop()
        {
            if (! _isActive) return;
            _isActive = false;
            _bluetoothDevice.AdvertisementReceived -= PackageReceived;
            _bluetoothDevice.StopListening();
            await RunWithDispatcher(() =>
            {
                _data.Mode = "Stopped";
                _data.ClearData();
            });
        }

        public void Dispose()
        {
            //_bluetoothDevice.AdvertisementReceived -= PackageReceived;
        }
    }
}
