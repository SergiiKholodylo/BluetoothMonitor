using BluetoothListener.Lib.BeaconPackages;
using BluetoothListener.Lib.BeaconPackages.Packets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothListener.Lib.Test.BeaconPackages
{
    [TestClass]
    public class PackageFactoryTest:PackageFactory
    {
        [TestMethod]
        public void TestParseEddystoneTlm()
        {
            var payload = new byte[] { 0xAA, 0xFE, 0x20, 0x00, 0x0E, 0x0F, 0x1B, 0x00, 0x1F, 0x66, 0xD2, 0x9B, 0x15, 0xD8, 0x3D, 0x52 };

            var package = (EddystoneTlm)ParseEddystoneTlm(payload);

            Assert.IsTrue(package.Version==0);
        }

        [TestMethod]
        public void TestParseEddystoneEid()
        {
            var payload = new byte[] { 0xAA, 0xFE, 0x30, 0x9A, 0x0E, 0x0F, 0x1B, 0x00, 0x1F, 0x66, 0xD2, 0x9B};

            var package = (EddystoneEID)ParseEddystoneEid(payload);

            Assert.IsTrue(package.RangingData == -102);
        }
    }
}
