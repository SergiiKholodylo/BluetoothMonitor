using BluetoothListener.Lib.Packages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.UI.Core;

namespace BluetoothListener.Lib
{
    public class BluetoothListenerManager
    {
        private readonly ObservableCollection<BeaconDevice> _devices;
        private readonly CoreDispatcher _dispatcher;
        private readonly BluetoothReceiver _bluetoothDevice;
        private int _packagesReceived = 0;
        private int _packagesDropped = 0;


        static bool _busy = false;

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
                Debug.WriteLine($"A Package was Dropped {_packagesDropped} from {_packagesReceived} ({_packagesDropped/_packagesReceived*100}%)");
                return;
            }

            var beacon = BeaconConverter.ToBeaconPackage(args);

            if (beacon.Packages.Count == 0) return;

            if (beacon.Rssi == -127) return;

            try
            {
                _busy = true;
                var alreadyExist = _devices.FirstOrDefault((x => x.BluetoothAddress == beacon.BluetoothAddress));
                var index = _devices.IndexOf(alreadyExist);
                if (index >= 0)
                {
                    MovePackagesToNewBeacon(alreadyExist, beacon);
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
            finally {
                _busy = false;
            }
        }

        private static void MovePackagesToNewBeacon(BeaconDevice alreadyExist, BeaconDevice beacon)
        {
            var newPackagesType = new Dictionary<Type, string>();

            foreach (var package in beacon.Packages)
            {
                var newType = package.GetType();
                if (!newPackagesType.ContainsKey(newType)) newPackagesType.Add(newType, "");
            }
            foreach (var package in alreadyExist.Packages) {
                if (!newPackagesType.ContainsKey(package.GetType()))
                    beacon.Packages.Add(package);
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
