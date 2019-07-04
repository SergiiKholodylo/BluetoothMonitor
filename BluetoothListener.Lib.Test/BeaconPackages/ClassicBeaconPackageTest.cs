using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluetoothListener.Lib.BeaconPackages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothListener.Lib.Test.BeaconPackages
{
    [TestClass]
    public class ClassicBeaconPackageTest
    {
        [TestMethod]
        public void TestDisplayFunction()
        {
            var classicBeaconPackage = new ClassicBeaconPackage
            {
                ProximityUuid = "123456789",
                Major = "12345",
                Minor = "67890"
            };
            var standardOutput = "UUID: 123456789 Major: 12345 Minor: 67890";


            var displayInfo = classicBeaconPackage.Display();

            Assert.AreEqual(displayInfo,standardOutput);
        }
    }
}
