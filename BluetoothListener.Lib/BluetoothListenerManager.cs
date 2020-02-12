using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using BluetoothListener.Lib.BeaconPackages;
using BluetoothListener.Lib.BluetoothAdvertisement;

namespace BluetoothListener.Lib
{
    public class BluetoothListenerManager
    {
        private readonly IViewData _data;
        private readonly CoreDispatcher _dispatcher;
        private readonly IBluetoothReceiver _bluetoothDevice;
        private bool _isActive;


        private static bool _isBusy;
        private IBeaconCache _cache;

        public BluetoothListenerManager(IViewData data, CoreDispatcher dispatcher, IBeaconCache cache)
        {
            _data = data;
            _dispatcher = dispatcher;
            _cache = cache;
            _bluetoothDevice = new BluetoothReceiver();
            //_bluetoothDevice.AdvertisementReceived += PackageReceived;
            _isActive = false;
        }

        private async void PackageReceived(IBluetoothAdvertisementPackage args)
        {
            try
            {
                //await RunWithDispatcher(() => { _data.Received++; });
            
                if (_isBusy)
                {
                    //await RunWithDispatcher(() => { _data.Dropped++; });
                
                    Debug.WriteLine($"A Package was Dropped {_data.Dropped} from {_data.Received} ({_data.Dropped * 100.0 / (1.0 * _data.Received) }%)");
                    return;
                }

                var beacon = BeaconBuilder.CreateBeaconDeviceFromBleAdvertisement(args);

                if (beacon.NumberOfPackages() == 0) return;

                if (beacon.RssiOutOfRange()) return;

                Debug.WriteLine(beacon);

                _cache.AddOrUpdate(beacon);

                //InsertOrReplaceBeaconInCollection(beacon);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unknown ERROR!!!!!!");
            }
        }

        private void InsertOrReplaceBeaconInCollection(IBluetoothBeacon beacon)
        {
            try
            {
                var devices = _data.Devices;
                _isBusy = true;

                var bluetoothAddress = beacon.BluetoothAddress;
                var found = devices.ContainsKey(bluetoothAddress);

                if (found)
                {
                    SaveDataFromExistingBeaconToNewOne(beacon, devices, bluetoothAddress);
                }
                devices.Add(bluetoothAddress, beacon);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Oooops, Lost Package {ex.Message}");
            }
            finally
            {
                _isBusy = false;
            }
        }

        private static void SaveDataFromExistingBeaconToNewOne(IBluetoothBeacon beacon, IDictionary<ulong, IBluetoothBeacon> devices, ulong bluetoothAddress)
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
            });
            _cache.Clear();
            GC.Collect();
        }
    }
}
