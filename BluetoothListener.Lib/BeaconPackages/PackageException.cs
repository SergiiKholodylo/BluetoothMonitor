using System;

namespace BluetoothListener.Lib.BeaconPackages
{
    public class PackageException:Exception
    {
        public PackageException(string message) : base(message)
        {
        }
    }
}
