﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using BluetoothListener.Lib.BeaconPackages;

namespace BluetoothListener.Lib
{
    public class BeaconDevice:IBluetoothBeacon
    {
        public short Rssi { set; get; }
        public DateTimeOffset Timestamp { set; get; }
        public ulong ReceivedTimes { get; set; }
        public ulong TimeSinceLastPacketReceivedInSec { get; set; }
        public ulong BluetoothAddress { set; get; }

        public IPackageStorage Packages { set; get; }

        public string BluetoothAddressHex => BluetoothAddress.ToString("X");

        public string Info => Packages.Info();

        public BeaconDevice(ulong address, short rssi, DateTimeOffset timeOffset)
        {
            BluetoothAddress = address;
            Rssi = rssi;
            Timestamp = timeOffset;
            ReceivedTimes = 1L;
            Packages = new PackageStorage();
        }

        public int NumberOfPackages()
        {
            return Packages.Count();
        }

        public bool RssiOutOfRange()
        {
            return Rssi == -127;
        }

        public void AddPackage(IBeaconPackage package)
        {
            Packages.Add(package);
        }

        public void CopyMissedPackagesFromBeacon(IBluetoothBeacon source)
        {
            Packages.CopyMissedPackagesFromBeacon(source);
        }

        public void UpdatePackageCounterAndPeriodBetweenPackages(IBluetoothBeacon previousBeacon)
        {
            TimeSinceLastPacketReceivedInSec = Convert.ToUInt64(Math.Abs((Timestamp - previousBeacon.Timestamp).TotalSeconds));
            ReceivedTimes = previousBeacon.ReceivedTimes + 1;
        }

        public void UpdateFromDevice(IBluetoothBeacon beacon)
        {

            TimeSinceLastPacketReceivedInSec = Convert.ToUInt64(Math.Abs((Timestamp - beacon.Timestamp).TotalSeconds));
            ReceivedTimes ++;

            Rssi = beacon.Rssi;
            Timestamp = beacon.Timestamp;
            BluetoothAddress = beacon.BluetoothAddress;

            var newPackages = beacon.Packages.GetPackages();

            foreach (var beaconPackage in newPackages)
            {
                Packages.Add(beaconPackage);
            }
        }
    }
}
