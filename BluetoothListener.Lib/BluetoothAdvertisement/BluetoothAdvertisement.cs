using System;
using System.Collections.Generic;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public class BluetoothAdvertisement:IBluetoothAdvertisement
    {
        public IList<BluetoothLEAdvertisementDataSection> DataSections { get; set; }
        public BluetoothLEAdvertisementFlags? Flags { get; set; }
        public string LocalName { get; set; }
        public IList<BluetoothLEManufacturerData> ManufacturerData { get; set; }
        public IList<Guid> ServiceUuids { get; set; }
    }
}
