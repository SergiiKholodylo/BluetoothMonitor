using BluetoothListener.Lib.Packages;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.UI.Core;
using BluetoothListener.Lib.BeaconPackages;

namespace BluetoothListener.Lib
{
    public class BluetoothListenerManager
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
            _bluetoothDevice.AdvertisementReceived += PackageReceived;
            _isActive = false;
        }

        private async void PackageReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
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

            await InsertOrReplaceBeaconInCollection(beacon);
        }

        private async Task InsertOrReplaceBeaconInCollection(BeaconDevice beacon)
        {
            try
            {
                var devices = _data.Devices;
                _busy = true;
                var alreadyExist = devices.FirstOrDefault((x => x.BluetoothAddress == beacon.BluetoothAddress));
                var index = devices.IndexOf(alreadyExist);
                if (index >= 0)
                {
                    beacon.CopyUniquePackagesFrom(alreadyExist);

                    await RunWithDispatcher(() => { devices.Remove(alreadyExist); });
                }
                await RunWithDispatcher(() => { devices.Add(beacon); });

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
            await RunWithDispatcher(() =>
            {
                _data.ClearData();
                _data.Mode = "Start";
            });
            _bluetoothDevice.StartListening();
        }

        public async void Stop()
        {
            if (! _isActive) return;
            _isActive = false;
            _bluetoothDevice.StopListening();
            await RunWithDispatcher(() =>
            {
                _data.Mode = "Stop";
            });
        }

        private void PrintList()
        {
            //Debug.WriteLine($"***********Beacons List Start* {_devices.Count} **********");
            //foreach (var device in _devices)
            //{
            //    //Debug.WriteLine($"   Address: {device.BluetoothAddress:X} RSSI: {device.Rssi}dB TimeStamp {device.Timestamp}");

            //    //Debug.WriteLine($"***********Manufacturer Settings* {device.Manufacturer.Count} **********");
            //    foreach (var manufacture in device.Manufacturer)
            //    {
            //        Debug.WriteLine(Utils.PrintArray(manufacture));
            //    }
            //    Debug.WriteLine($"***********Data Settings* {device.Data.Count} **********");
            //    foreach (var data in device.Data)
            //    {
            //        Debug.WriteLine(Utils.PrintArray(data));
            //    }
            //}
            //Debug.WriteLine("***********Beacons List End******************");
        }


        
    }
}
