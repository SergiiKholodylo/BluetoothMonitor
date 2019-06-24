using BluetoothListener.Lib.Packages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.UI.Core;

namespace BluetoothListener.Lib
{
    public class BluetoothListenerManager
    {
        private readonly ObservableCollection<BeaconDevice> _devices;
        private readonly CoreDispatcher _dispatcher;
        private readonly IBluetoothReceiver _bluetoothDevice;
        private int _packagesReceived;
        private int _packagesDropped;


        private static bool _busy;

        public BluetoothListenerManager(ObservableCollection<BeaconDevice> devices, CoreDispatcher dispatcher)
        {
            _devices = devices;
            _dispatcher = dispatcher;
            _bluetoothDevice = new BluetoothReceiver();
            _bluetoothDevice.AdvertisementReceived += PackageReceived;
        }

        private async void PackageReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            _packagesReceived++;
            if (_busy)
            {
                _packagesDropped++;
                Debug.WriteLine($"A Package was Dropped {_packagesDropped} from {_packagesReceived} ({_packagesDropped * 100.0 / (1.0 * _packagesReceived) }%)");
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
                _busy = true;
                var alreadyExist = _devices.FirstOrDefault((x => x.BluetoothAddress == beacon.BluetoothAddress));
                var index = _devices.IndexOf(alreadyExist);
                if (index >= 0)
                {
                    beacon.CopyUniquePackagesFrom(alreadyExist);
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        _devices.Remove(alreadyExist);
                    });
                }
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _devices.Add(beacon);
                });

            }
            finally
            {
                _busy = false;
            }
        }

        public void Start()
        {
            _devices.Clear();
            _bluetoothDevice.StartListening();
            Debug.WriteLine("Start");
        }

        public void Stop()
        {
            _bluetoothDevice.StopListening();
            Debug.WriteLine("Stop");
        }

        private void PrintList()
        {
            //Debug.WriteLine($"***********Beacons List Start* {_devices.Count} **********");
            foreach (var device in _devices)
            {
                //Debug.WriteLine($"   Address: {device.BluetoothAddress:X} RSSI: {device.Rssi}dB TimeStamp {device.Timestamp}");

                //Debug.WriteLine($"***********Manufacturer Settings* {device.Manufacturer.Count} **********");
                foreach (var manufacture in device.Manufacturer)
                {
                    Debug.WriteLine(Utils.PrintArray(manufacture));
                }
                Debug.WriteLine($"***********Data Settings* {device.Data.Count} **********");
                foreach (var data in device.Data)
                {
                    Debug.WriteLine(Utils.PrintArray(data));
                }
            }
            //Debug.WriteLine("***********Beacons List End******************");
        }


        
    }
}
