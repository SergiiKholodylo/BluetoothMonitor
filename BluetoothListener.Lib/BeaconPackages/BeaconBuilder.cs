using System.Runtime.InteropServices.WindowsRuntime;
using BluetoothListener.Lib.BluetoothAdvertisement;

namespace BluetoothListener.Lib.BeaconPackages
{
    
    public static class BeaconBuilder
    {

        public static IBluetoothBeacon CreateBeaconDeviceFromBleAdvertisement(IBluetoothAdvertisementPackage eventArgs)
        {
            var beaconDevice = CreateBeacon(eventArgs);

            AddPackagesFromManufacturerSection(eventArgs, beaconDevice);

            AddPackagesFromDataSection(eventArgs, beaconDevice);

            return beaconDevice;
        }

        public static void AddPackagesFromDataSection(IBluetoothAdvertisementPackage eventArgs, IBluetoothBeacon beaconDevice)
        {
            var dataSections = eventArgs.Advertisement.DataSections;
            foreach (var data in dataSections)
            {
                var array = data.Data.ToArray();

                try
                {
                    var package = PackageFactory.CreatePackageFromDataPayload(array);
                    beaconDevice.AddPackage(package);
                }
                catch (PackageException e)
                {

                }
            }
        }

        public static void AddPackagesFromManufacturerSection(IBluetoothAdvertisementPackage eventArgs, IBluetoothBeacon beaconDevice)
        {
            var manufacturerSections = eventArgs.Advertisement.ManufacturerData;
            foreach (var manufacture in manufacturerSections)
            {
                var array = manufacture.Data.ToArray();

                try
                {
                    var package = PackageFactory.CreatePackageFromManufacturerPayload(array);
                    beaconDevice.AddPackage(package);
                }
                catch (PackageException e)
                {

                }
            }
        }

        public static IBluetoothBeacon CreateBeacon(IBluetoothAdvertisementPackage eventArgs)
        {
            var rssi = eventArgs.RawSignalStrengthInDBm;
            var address = eventArgs.BluetoothAddress;
            var timestamp = eventArgs.Timestamp;

            var beaconDevice = new BeaconDevice(address, rssi, timestamp);
            return beaconDevice;
        }

    }

    
}
