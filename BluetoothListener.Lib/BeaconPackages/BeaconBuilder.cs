using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.Packages
{
    
    public class BeaconBuilder
    {

        public static BeaconDevice CreateBeaconDeviceFromBleAdvertisement(BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            BeaconDevice beaconDevice = CreateBeacon(eventArgs);

            AddPackagesFromManufacturerSection(eventArgs, beaconDevice);

            AddPackagesFromDataSection(eventArgs, beaconDevice);

            return beaconDevice;
        }

        private static void AddPackagesFromDataSection(BluetoothLEAdvertisementReceivedEventArgs eventArgs, BeaconDevice beaconDevice)
        {
            var dataSections = eventArgs.Advertisement.DataSections;
            foreach (var data in dataSections)
            {
                var array = data.Data.ToArray();
                beaconDevice.Data.Add(array);

                try
                {
                    var package = PackageFactory.CreatePackageFromDataPayload(array);
                    beaconDevice.AddPackage(package);
                }
                catch (PackageException e)
                {
                    //Debug.WriteLine(e.Message);
                }
            }
        }

        private static void AddPackagesFromManufacturerSection(BluetoothLEAdvertisementReceivedEventArgs eventArgs, BeaconDevice beaconDevice)
        {
            var manufacturerSections = eventArgs.Advertisement.ManufacturerData;
            foreach (var manufacture in manufacturerSections)
            {
                var array = manufacture.Data.ToArray();

                beaconDevice.Manufacturer.Add(array);
                try
                {
                    var package = PackageFactory.CreatePackageFromManufacturerPayload(array);
                    beaconDevice.AddPackage(package);
                }
                catch (PackageException e)
                {
                    //Debug.WriteLine(e.Message);
                }
            }
        }

        private static BeaconDevice CreateBeacon(BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            var rssi = eventArgs.RawSignalStrengthInDBm;
            var address = eventArgs.BluetoothAddress;
            var timestamp = eventArgs.Timestamp;

            var beaconDevice = new BeaconDevice(address, rssi, timestamp);
            return beaconDevice;
        }

    }

    
}
