using System;
using System.Collections.Generic;
using static System.Diagnostics.Debug;

namespace BluetoothListener.Lib
{
    public class BeaconCache:IBeaconCache
    {
        private readonly Dictionary<ulong, IBluetoothBeacon> _beaconCache = new Dictionary<ulong, IBluetoothBeacon>();

        public void AddOrUpdate(IBluetoothBeacon beacon)
        {
            WriteLine($"Add or Update Start {beacon}");
            var beaconAddress = beacon.BluetoothAddress;

            lock (_beaconCache)
            {
                if (_beaconCache.ContainsKey(beaconAddress))
                {
                    SaveDataFromExistingBeaconToNewOne(beacon);
                }
                _beaconCache.Add(beaconAddress, beacon);
            }
            WriteLine($"Add or Update End {beacon}");
        }

        private void SaveDataFromExistingBeaconToNewOne(IBluetoothBeacon beacon)
        {
            IBluetoothBeacon existingBeacon;

            var bluetoothAddress = beacon.BluetoothAddress;

            const int endlessLoopLimit = 40;
            var tries = 0;
            while (!_beaconCache.TryGetValue(bluetoothAddress, out existingBeacon))
            {
                if (tries++ > endlessLoopLimit) throw new Exception("Can't read value!");
            }

            beacon.CopyMissedPackagesFromBeacon(existingBeacon);
            beacon.UpdatePackageCounterAndPeriodBetweenPackages(existingBeacon);
            _beaconCache.Remove(bluetoothAddress);
        }

        public void Clear()
        {
            lock (_beaconCache)
            {
                _beaconCache.Clear();
            }
        }
    }
}
