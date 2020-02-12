using BluetoothListener.Lib.BeaconPackages;
using BluetoothListener.Lib.BeaconPackages.Packets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothListener.Lib.Test.BeaconPackages
{
    [TestClass]
    public class PackageStorageTest
    {
        [TestMethod]
        public void TestPackageStorageCount()
        {
            var storage = new PackageStorage();

            var elementsNumber = storage.Count();

            Assert.AreEqual(0, elementsNumber);
        }

        [TestMethod]
        public void TestPackageStorageAdd()
        {
            var storage = new PackageStorage();
            storage.Add(new EddystoneTlm());

            var elementsNumber = storage.Count();

            Assert.AreEqual(1, elementsNumber);
        }

        [TestMethod]
        public void TestPackageStorageConcurrentAdd()
        {
            var storage = new PackageStorage();
            storage.Add(new EddystoneTlm());
            storage.Add(new EddystoneTlm());

            var elementsNumber = storage.Count();

            Assert.AreEqual(1, elementsNumber);
        }

        [TestMethod]
        public void TestPackageStorageClear()
        {
            var storage = new PackageStorage();
            storage.Add(new EddystoneTlm());
            storage.Clear();

            var elementsNumber = storage.Count();

            Assert.AreEqual(0, elementsNumber);
        }


    }
}
