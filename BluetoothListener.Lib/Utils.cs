using System;
using System.Linq;

namespace BluetoothListener.Lib
{
    public class Utils
    {
        public static void Swap(ref byte v1, ref byte v2)
        {
            var temp = v2;
            v2 = v1;
            v1 = temp;
        }

        public static string PrintArray(byte[] array, string separator = " ")
        {
            var temp = array.Aggregate(string.Empty, (current, b) => current + ("" + b.ToString("X2") + separator));

            return temp.Substring(0, Math.Max(temp.Length - separator.Length,0)); // remove final separator
        }
    }
}
