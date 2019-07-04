using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Advertisement;
using BluetoothListener.Lib.BeaconPackages;
using BluetoothListener.Lib.BluetoothAdvertisement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothListener.Lib.Test.BeaconPackages
{
    [TestClass]
    public class BeaconBuilderTest
    {
        const ulong BluetoothAddress = 0x88UL;
        const short Rssi = -80;


        [TestMethod]
        public void TestCreateBeacon()
        {
            var dateTimeOffset = new DateTimeOffset();

            var beacon = BeaconBuilder.CreateBeacon(new BluetoothAdvertisementPackage
            {
                BluetoothAddress = BluetoothAddress,
                RawSignalStrengthInDBm = Rssi,
                Timestamp = dateTimeOffset
            });

            Assert.AreEqual(dateTimeOffset,beacon.Timestamp,"Copying Timestamp from Advertisement");
            Assert.AreEqual(BluetoothAddress, beacon.BluetoothAddress, "Copying Bluetooth Address from Advertisement");
            Assert.AreEqual(Rssi, beacon.Rssi, "Copying RSSI from Advertisement");
        }


        [TestMethod]
        public void TestCreateBeaconDeviceFromBleAdvertisement()
        {
            var dateTimeOffset = new DateTimeOffset();
            var advertisementPackage = CreateAdvertisementPackage(dateTimeOffset);

            var beacon =
                BeaconBuilder.CreateBeaconDeviceFromBleAdvertisement(advertisementPackage);

            Assert.AreEqual(0UL, beacon.TimeSinceLastPacketReceivedInSec);
            Assert.AreEqual(1UL, beacon.ReceivedTimes);
            Assert.AreEqual(BluetoothAddress, beacon.BluetoothAddress);
            Assert.AreEqual(Rssi, beacon.Rssi);
            Assert.AreEqual(dateTimeOffset, beacon.Timestamp);
            Assert.AreEqual(0, beacon.NumberOfPackages());
            Assert.AreEqual(false,beacon.RssiOutOfRange());
        }

        [TestMethod]
        public void TestCreateBeaconDeviceFromBleAdvertisementWithClassicalPackage()
        {
            var dateTimeOffset = new DateTimeOffset();
            var advertisementPackage = CreateAdvertisementPackage(dateTimeOffset);

            var beacon =
                BeaconBuilder.CreateBeaconDeviceFromBleAdvertisement(advertisementPackage);
            AddManufacturerDataValidData(advertisementPackage);
            BeaconBuilder.AddPackagesFromManufacturerSection(advertisementPackage,beacon);
            var firstPackage = beacon.Packages.GetPackages().FirstOrDefault();


            Assert.AreEqual(1, beacon.NumberOfPackages());
            Assert.IsInstanceOfType(firstPackage,typeof(ClassicBeaconPackage));
        }

        [TestMethod]
        public void TestCreateBeaconDeviceFromBleAdvertisementWithClassicalPackageInvalidData()
        {
            var dateTimeOffset = new DateTimeOffset();
            var advertisementPackage = CreateAdvertisementPackage(dateTimeOffset);


            var beacon =
                BeaconBuilder.CreateBeaconDeviceFromBleAdvertisement(advertisementPackage);
            AddManufacturerDataInvalidData(advertisementPackage);
            BeaconBuilder.AddPackagesFromManufacturerSection(advertisementPackage, beacon);



            Assert.AreEqual(0, beacon.NumberOfPackages());
        }

        private IBluetoothAdvertisementPackage CreateAdvertisementPackage(DateTimeOffset dateTimeOffset)
        {
            var advertisementPackage = new BluetoothAdvertisementPackage
            {
                BluetoothAddress = BluetoothAddress,
                Timestamp = dateTimeOffset,
                RawSignalStrengthInDBm = Rssi,
                AdvertisementType = BluetoothLEAdvertisementType.NonConnectableUndirected,
                Advertisement = new BluetoothAdvertisement.BluetoothAdvertisement
                {
                    ManufacturerData = new List<BluetoothLEManufacturerData>(),
                    DataSections = new List<BluetoothLEAdvertisementDataSection>()
                }
            };
            return advertisementPackage;
        }

        private void AddManufacturerDataValidData(IBluetoothAdvertisementPackage package)
        {
            package.Advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData
            {
                Data = (new byte[]{ 0x02,0x15, 0xF7, 0x82, 0x6D, 0xA6, 0x4F, 0xA2, 0x4E, 0x98, 0x80, 0x24, 0xBC, 0x5B, 0x71, 0xE0, 0x89, 0x3E, 0x27, 0x70, 0x98, 0x4B, 0xB3 }).AsBuffer(),
                CompanyId = 0x99
            });
        }

        private void AddManufacturerDataInvalidData(IBluetoothAdvertisementPackage package)
        {
            package.Advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData
            {
                Data = (new byte[] { 0x10, 0x05, 0x03,0x1C, 0xFD, 0xD5, 0xA0 }).AsBuffer(),
                CompanyId = 0x99
            });
        }
    }
}
