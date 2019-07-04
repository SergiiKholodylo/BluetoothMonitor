using System;
using System.Text;

namespace BluetoothListener.Lib.BeaconPackages
{
    public class EddystoneUrlHelper
    {
        public static string FromEddystoneUrl(string eddystoneUrl)
        {
            var returnString = string.Empty;

            var tempString = eddystoneUrl.ToCharArray();
            for (var i = 0; i < tempString.Length; i += 2)
            {
                var symbol = Convert.ToInt32(tempString[i] + tempString[i + 1].ToString(), 16);

                #region Schema

                if (i == 0 && symbol < 3)
                {
                    switch (symbol)
                    {
                        case 0:
                            returnString += "http://www.";
                            break;
                        case 1:
                            returnString += "https://www.";
                            break;
                        case 2:
                            returnString += "http://";
                            break;
                        case 3:
                            returnString += "https://";
                            break;
                    }
                    continue;
                }

                #endregion

                if (symbol > 0x20 && symbol < 0x7F)
                {
                    var character = (char)symbol;
                    returnString += character.ToString();
                }
                else
                {

                    #region Extention

                    switch (symbol)
                    {
                        case 0:
                            returnString += ".com/";
                            break;
                        case 1:
                            returnString += ".org/";
                            break;
                        case 2:
                            returnString += ".edu/";
                            break;
                        case 3:
                            returnString += ".net/";
                            break;
                        case 4:
                            returnString += ".info/";
                            break;
                        case 5:
                            returnString += ".biz/";
                            break;
                        case 6:
                            returnString += ".gov/";
                            break;
                        case 7:
                            returnString += ".com";
                            break;
                        case 8:
                            returnString += ".org";
                            break;
                        case 9:
                            returnString += ".edu";
                            break;
                        case 10:
                            returnString += ".net";
                            break;
                        case 11:
                            returnString += ".info";
                            break;
                        case 12:
                            returnString += ".biz";
                            break;
                        case 13:
                            returnString += ".gov";
                            break;
                    }

                    #endregion
                }

            }

            return returnString;
        }

        public static string ToEddystoneUrl(string decoded)
        {
            var returnString = string.Empty;

            while (decoded.Length > 0)
            {

                if (decoded.StartsWith("http://www."))
                {
                    returnString += "00";
                    decoded = decoded.Substring(11);
                    continue;
                }
                if (decoded.StartsWith("https://www."))
                {
                    returnString += "01";
                    decoded = decoded.Substring(12);
                    continue;
                }
                if (decoded.StartsWith("http://"))
                {
                    returnString += "02";
                    decoded = decoded.Substring(7);
                    continue;
                }
                if (decoded.StartsWith("https://"))
                {
                    returnString += "03";
                    decoded = decoded.Substring(8);
                    continue;
                }

                if (decoded.StartsWith(".com/"))
                {
                    returnString += "00";
                    decoded = decoded.Substring(5);
                    continue;
                }

                if (decoded.StartsWith(".org/"))
                {
                    returnString += "01";
                    decoded = decoded.Substring(5);
                    continue;
                }
                if (decoded.StartsWith(".edu/"))
                {
                    returnString += "02";
                    decoded = decoded.Substring(5);
                    continue;
                }
                if (decoded.StartsWith(".net/"))
                {
                    returnString += "03";
                    decoded = decoded.Substring(5);
                    continue;
                }
                if (decoded.StartsWith(".info/"))
                {
                    returnString += "04";
                    decoded = decoded.Substring(6);
                    continue;
                }
                if (decoded.StartsWith(".biz/"))
                {
                    returnString += "05";
                    decoded = decoded.Substring(5);
                    continue;
                }
                if (decoded.StartsWith(".gov/"))
                {
                    returnString += "06";
                    decoded = decoded.Substring(5);
                    continue;
                }


                if (decoded.StartsWith(".com"))
                {
                    returnString += "07";
                    decoded = decoded.Substring(4);
                    continue;
                }

                if (decoded.StartsWith(".org"))
                {
                    returnString += "08";
                    decoded = decoded.Substring(4);
                    continue;
                }
                if (decoded.StartsWith(".edu"))
                {
                    returnString += "09";
                    decoded = decoded.Substring(4);
                    continue;
                }
                if (decoded.StartsWith(".net"))
                {
                    returnString += "0A";
                    decoded = decoded.Substring(4);
                    continue;
                }
                if (decoded.StartsWith(".info"))
                {
                    returnString += "0B";
                    decoded = decoded.Substring(5);
                    continue;
                }
                if (decoded.StartsWith(".biz"))
                {
                    returnString += "0C";
                    decoded = decoded.Substring(4);
                    continue;
                }
                if (decoded.StartsWith(".gov"))
                {
                    returnString += "0D";
                    decoded = decoded.Substring(4);
                    continue;
                }

                var asciiBytes = Encoding.ASCII.GetBytes(decoded.ToCharArray(), 0, 1);
                returnString += asciiBytes[0].ToString("X");
                decoded = decoded.Substring(1);
            }

            if (returnString.Length > 36)
                throw new Exception($"Encoded URL ({returnString}) is too long (max 18 bytes) ");

            return returnString;
        }
    }
}
