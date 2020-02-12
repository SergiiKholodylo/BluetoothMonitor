using System;
using System.Collections.Generic;

namespace BluetoothListener.Lib.BeaconPackages
{
    public class PackageStorage : IPackageStorage
    {
        private readonly Dictionary<Type, IBeaconPackage> _beaconPackages = new Dictionary<Type, IBeaconPackage>();

        public void Add(IBeaconPackage package)
        {
            var type = package.GetType();
            if (_beaconPackages.ContainsKey(type))
            {
                _beaconPackages.Remove(type);
            }
            _beaconPackages.Add(type, package);
        }

        public void Clear()
        {
            _beaconPackages.Clear();
        }

        public int Count()
        {
            return _beaconPackages.Count;
        }

        public string Info()
        {
            var info = string.Empty;

            foreach (var beaconPackage in _beaconPackages)
            {
                info += beaconPackage.Value + Environment.NewLine;
            }

            return info;
        }

        public IEnumerable<IBeaconPackage> GetPackages()
        {
            return _beaconPackages.Values;
        }

        public void CopyMissedPackagesFromBeacon(IBluetoothBeacon source)
        {
            var existingPackages = source.Packages;
            var packages = existingPackages.GetPackages();

            foreach (var package in packages)
            {
                var type = package.GetType();
                if (_beaconPackages.ContainsKey(type)) continue;
                _beaconPackages.Add(type, package);
            }
        }


    }
}
