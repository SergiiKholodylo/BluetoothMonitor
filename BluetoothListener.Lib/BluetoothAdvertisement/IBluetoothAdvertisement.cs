using System;
using System.Collections.Generic;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public interface IBluetoothAdvertisement
    {
        //IReadOnlyList<BluetoothLEManufacturerData> GetManufacturerDataByCompanyId(ushort companyId);
        //IReadOnlyList<BluetoothLEAdvertisementDataSection> GetSectionsByType(byte type);

        IList<BluetoothLEAdvertisementDataSection> DataSections { get; set; }
        BluetoothLEAdvertisementFlags? Flags { get; set; }
        string LocalName { get; set; }
        IList<BluetoothLEManufacturerData> ManufacturerData { get; set; }
        IList<Guid> ServiceUuids { get; }
    }
}
