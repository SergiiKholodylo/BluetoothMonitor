using System;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public interface IBluetoothAdvertisementPackage
    {
            IBluetoothAdvertisement Advertisement { get; }
            BluetoothLEAdvertisementType AdvertisementType { get; }
            ulong BluetoothAddress { get; }
            short RawSignalStrengthInDBm { get; }
            DateTimeOffset Timestamp { get; }

            
    }
}
