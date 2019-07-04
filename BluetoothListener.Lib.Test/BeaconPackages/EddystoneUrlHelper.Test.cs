using BluetoothListener.Lib.BeaconPackages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothListener.Lib.Test.Packages
{
    [TestClass]
    public class EddystoneUrlHelperTest
    {
        [TestMethod]
        public void FromEddystoneUrlTest()
        {
            const string encodedUrl = "026B6E746B2E696F2F6564647973746F6E65";
            const string decodedUrl = "http://kntk.io/eddystone";


            var functionDecoded = EddystoneUrlHelper.FromEddystoneUrl(encodedUrl);
            var functionEncoded = EddystoneUrlHelper.ToEddystoneUrl(decodedUrl);


            Assert.AreEqual(encodedUrl, functionEncoded);
            Assert.AreEqual(decodedUrl, functionDecoded);
        }
    }
}
