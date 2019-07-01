using System;
using System.Collections.Generic;
using System.Diagnostics;
using BluetoothListener.Lib.Packages;

namespace BluetoothListener.Lib
{
    public class BeaconDevice
    {


        protected List<BeaconPackage> Packages = new List<BeaconPackage>();
        public short Rssi { set; get; }
        public ulong BluetoothAddress { set; get; }

        public string BluetoothAddressHex => BluetoothAddress.ToString("X");
        public DateTimeOffset Timestamp { set;  get; }

        public List<byte[]> Manufacturer = new List<byte[]>();

        public List<byte[]> Data = new List<byte[]>();

        public string ManufacturerString { get {
                var str = "Manufacturer Data" + Environment.NewLine;
                foreach (var array in Manufacturer)
                {
                    str += Utils.PrintArray(array) + Environment.NewLine;
                }
                return str;
            } }

        public string DataString
        {
            get
            {
                var str = "Data" + Environment.NewLine;
                
                foreach (var array in Data)
                {
                    str += Utils.PrintArray(array) + Environment.NewLine;
                }
                return str;
            }
        }

        public string Info { get
            {
                var info = string.Empty;
                foreach (var package in Packages)
                {
                    info += package.Display() + Environment.NewLine;
                }
                return info;
            } }

        public BeaconDevice(ulong address, short rssi, DateTimeOffset timeOffset)
        {
            BluetoothAddress = address;
            Rssi = rssi;
            Timestamp = timeOffset;

        }

        public int NumberOfPackages()
        {
            return Packages.Count;
        }

        public bool RssiOutOfRange()
        {
            return Rssi == -127;
        }

        public void AddPackage(BeaconPackage package)
        {
            Packages.Add(package);
        }

        public void CopyUniquePackagesFrom(BeaconDevice source)
        {
            var newPackagesType = new Dictionary<Type, string>();

            foreach (var package in Packages)
            {
                var newType = package.GetType();
                if (!newPackagesType.ContainsKey(newType)) newPackagesType.Add(newType, "");
            }
            foreach (var package in source.Packages)
            {
                if (!newPackagesType.ContainsKey(package.GetType()))
                    AddPackage(package);
            }
        }
    }
}
