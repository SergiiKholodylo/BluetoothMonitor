using System.Collections.Generic;

namespace BluetoothListener.Lib.BeaconPackages
{
    public interface IPackageStorage
    {
        void Add(IBeaconPackage package);
        void Clear();
        int Count();

        string Info();
        void CopyMissedPackagesFromBeacon(IBluetoothBeacon source);

        IEnumerable<IBeaconPackage> GetPackages();
    }
}
