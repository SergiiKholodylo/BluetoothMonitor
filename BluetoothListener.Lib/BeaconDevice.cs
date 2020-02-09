using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BluetoothListener.Lib.BeaconPackages;

namespace BluetoothListener.Lib
{
    public class BeaconDevice:IBluetoothBeacon,INotifyPropertyChanged
    {
        private short _rssi;
        private DateTimeOffset _timestamp;
        private ulong _receivedTimes;
        private ulong _timeSinceLastPacketReceivedInSec;
        private ulong _bluetoothAddress;

        public short Rssi
        {
            set
            {
                _rssi = value;
                NotifyPropertyChanged();
            }
            get => _rssi;
        }

        public DateTimeOffset Timestamp
        {
            set
            {
                _timestamp = value;
                NotifyPropertyChanged();
            }
            get => _timestamp;
        }

        public ulong ReceivedTimes
        {
            get => _receivedTimes;
            set
            {
                _receivedTimes = value;
                NotifyPropertyChanged();
            }
        }

        public ulong TimeSinceLastPacketReceivedInSec
        {
            get => _timeSinceLastPacketReceivedInSec;
            set
            {
                _timeSinceLastPacketReceivedInSec = value;
                NotifyPropertyChanged();
            }
        }

        public ulong BluetoothAddress
        {
            set
            {
                _bluetoothAddress = value;
                NotifyPropertyChanged($"BluetoothAddressHex");
                
            }
            get => _bluetoothAddress;
        }

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
            NotifyPropertyChanged($"Info");
        }

        public void CopyMissedPackagesFromBeacon(IBluetoothBeacon source)
        {
            Packages.CopyMissedPackagesFromBeacon(source);
            NotifyPropertyChanged($"Info");
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
