using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public class RegularBluetoothConverter : IBluetoothConverter
    {
        public IBluetoothAdvertisementPackage ConvertFromBluetoothLeAdvertisementPackage(BluetoothLEAdvertisementReceivedEventArgs e)
        {
            var advertisementPackage = new BluetoothAdvertisementPackage
            {
                BluetoothAddress = e.BluetoothAddress,
                Timestamp = e.Timestamp,
                RawSignalStrengthInDBm = e.RawSignalStrengthInDBm,
                AdvertisementType = e.AdvertisementType,
                Advertisement = new BluetoothAdvertisement
                {
                    ManufacturerData = new List<BluetoothLEManufacturerData>(),
                    DataSections = new List<BluetoothLEAdvertisementDataSection>()
                }
            };

            var adv = e.Advertisement;

            CopyManufacturerData(adv, advertisementPackage);

            CopyDataSection(adv, advertisementPackage);

            return advertisementPackage;
        }

        private static void CopyDataSection(BluetoothLEAdvertisement adv, BluetoothAdvertisementPackage advertisementPackage)
        {
            var dataSection = adv.DataSections;
            foreach (var advertisementDataSection in dataSection)
            {
                var destination = new byte[advertisementDataSection.Data.Length];
                advertisementDataSection.Data.CopyTo(destination);
                advertisementPackage.Advertisement.DataSections.Add(new BluetoothLEAdvertisementDataSection
                {
                    Data = destination.AsBuffer(),
                    DataType = advertisementDataSection.DataType
                });
            }
        }

        private static void CopyManufacturerData(BluetoothLEAdvertisement adv,
            BluetoothAdvertisementPackage advertisementPackage)
        {
            var manufacturer = adv.ManufacturerData;
            foreach (var data in manufacturer)
            {
                var destination = new byte[data.Data.Length];
                data.Data.CopyTo(destination);
                advertisementPackage.Advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData
                {
                    Data = destination.AsBuffer(),
                    CompanyId = data.CompanyId
                });
            }
        }
    }
}
