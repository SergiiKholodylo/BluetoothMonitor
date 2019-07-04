using System;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public class BluetoothAdvertisementPackage:IBluetoothAdvertisementPackage
    {
        public IBluetoothAdvertisement Advertisement { get; set; }
        public BluetoothLEAdvertisementType AdvertisementType { get; set; }
        public ulong BluetoothAddress { get; set; }
        public short RawSignalStrengthInDBm { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
