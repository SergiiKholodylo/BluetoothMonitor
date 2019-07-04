using System;
using BluetoothListener.Lib.BeaconPackages;

namespace BluetoothListener.Lib
{
    public interface IBluetoothBeacon
    {
         short Rssi { set; get; }
         ulong BluetoothAddress { set; get; }
         DateTimeOffset Timestamp { set; get; }

         ulong ReceivedTimes { set; get; }
         ulong TimeSinceLastPacketReceivedInSec { set; get; }

         IPackageStorage Packages { set; get; }

        void AddPackage(IBeaconPackage package);
        void CopyMissedPackagesFromBeacon(IBluetoothBeacon source);
        int NumberOfPackages();
        bool RssiOutOfRange();
        void UpdateFromDevice(IBluetoothBeacon beacon);
        void UpdatePackageCounterAndPeriodBetweenPackages(IBluetoothBeacon previousBeacon);
    }
}
