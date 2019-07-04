using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
