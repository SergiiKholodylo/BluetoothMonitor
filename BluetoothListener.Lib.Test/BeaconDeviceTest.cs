using System;
using BluetoothListener.Lib.BeaconPackages;
using BluetoothListener.Lib.BeaconPackages.Packets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothListener.Lib.Test
{
    [TestClass]
    public class BeaconDeviceTest
    {
        [TestMethod]
        public void TestCopyMissedPackagesFromBeaconReplacePackage()
        {
            var dateTimeOffset = new DateTimeOffset();

            var sourceBeacon = new BeaconDevice(0L, 0, dateTimeOffset);
            var targetBeacon = new BeaconDevice(0L, 0, dateTimeOffset);

            sourceBeacon.AddPackage(new EddystoneEID());
            sourceBeacon.AddPackage(new EddystoneTlm());
            sourceBeacon.AddPackage(new ClassicBeaconPackage());

            targetBeacon.AddPackage(new ClassicBeaconPackage());


            targetBeacon.CopyMissedPackagesFromBeacon(sourceBeacon);


            var targetBeaconCount = targetBeacon.NumberOfPackages();
            Assert.AreEqual(3, targetBeaconCount);

        }

        [TestMethod]
        public void TestCopyMissedPackagesFromBeaconAddPackage()
        {
            var dateTimeOffset = new DateTimeOffset();

            var sourceBeacon = new BeaconDevice(0L, 0, dateTimeOffset);
            var targetBeacon = new BeaconDevice(0L, 0, dateTimeOffset);

            sourceBeacon.AddPackage(new EddystoneEID());

            targetBeacon.AddPackage(new ClassicBeaconPackage());


            targetBeacon.CopyMissedPackagesFromBeacon(sourceBeacon);


            var targetBeaconCount = targetBeacon.NumberOfPackages();
            Assert.AreEqual(2, targetBeaconCount);

        }

        [TestMethod]
        public void TestCopyMissedPackagesFromBeaconNoPackages()
        {
            var dateTimeOffset = new DateTimeOffset();

            var sourceBeacon = new BeaconDevice(0L, 0, dateTimeOffset);
            var targetBeacon = new BeaconDevice(0L, 0, dateTimeOffset);


            targetBeacon.CopyMissedPackagesFromBeacon(sourceBeacon);


            var targetBeaconCount = targetBeacon.NumberOfPackages();
            Assert.AreEqual(0, targetBeaconCount);

        }


        [TestMethod]
        public void TestUpdatePackageCounterAndPeriodBetweenPackages()
        {
            var dateTimeOffset = new DateTimeOffset();
            var dateTimeOffsetPlus40Secs = dateTimeOffset.AddSeconds(40);

            var sourceBeacon = new BeaconDevice(0L, 0, dateTimeOffset);
            var targetBeacon = new BeaconDevice(0L, 0, dateTimeOffsetPlus40Secs);


            targetBeacon.UpdatePackageCounterAndPeriodBetweenPackages(sourceBeacon);


            Assert.AreEqual(40UL,targetBeacon.TimeSinceLastPacketReceivedInSec);
            Assert.AreEqual(2UL,targetBeacon.ReceivedTimes);
        }

        [TestMethod]
        public void TestUpdateFromDevice()
        {
            var dateTimeOffset = new DateTimeOffset();
            var dateTimeOffsetPlus40Secs = dateTimeOffset.AddSeconds(40);

            var existingBeacon = new BeaconDevice(0L, 0, dateTimeOffset);
            var newBeacon = new BeaconDevice(0x88, -66, dateTimeOffsetPlus40Secs);


            existingBeacon.UpdateFromDevice(newBeacon);


            Assert.AreEqual(40UL, existingBeacon.TimeSinceLastPacketReceivedInSec);
            Assert.AreEqual(2UL, existingBeacon.ReceivedTimes);

            Assert.AreEqual(0x88UL,existingBeacon.BluetoothAddress);
            Assert.AreEqual(-66, existingBeacon.Rssi);
            Assert.AreEqual(dateTimeOffsetPlus40Secs, existingBeacon.Timestamp);

        }


    }
}
