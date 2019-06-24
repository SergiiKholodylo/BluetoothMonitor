using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothListener.Lib.Test
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void SwapTest()
        {
            byte a = 1;
            byte b = 2;

            Utils.Swap(ref a, ref b);

            Assert.IsTrue(a == 2, "a should be 2");
            Assert.IsTrue(b == 1, "b should be 1");
        }

        [TestMethod]
        public void PrintArrayTest()
        {
            var array = new byte[] {16, 32, 48};
            var empty = new byte[0];

            var stringHexWithSpace = Utils.PrintArray(array);
            var stringHexWithNoSpace = Utils.PrintArray(array,"");
            var stringHexWithStar = Utils.PrintArray(array, "*");

            var emptyHexWithSpace = Utils.PrintArray(empty);
            var emptyHexWithNoSpace = Utils.PrintArray(empty, "");



            Assert.IsTrue(stringHexWithSpace.Equals("10 20 30"));
            Assert.IsTrue(stringHexWithStar.Equals("10*20*30"));
            Assert.IsTrue(stringHexWithNoSpace.Equals("102030"));

            Assert.IsTrue(emptyHexWithSpace.Equals(""));
            Assert.IsTrue(emptyHexWithNoSpace.Equals(""));

        }
    }
}
