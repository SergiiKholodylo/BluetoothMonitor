using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
using Buffer = Windows.Storage.Streams.Buffer;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public class RegularBluetoothConverter:IBluetoothConverter
    {
        private IBluetoothAdvertisementPackage _advertisementPackage;

        public IBluetoothAdvertisementPackage ConvertFromBluetoothLeAdvertisementPackage(BluetoothLEAdvertisementReceivedEventArgs e)
        {
            _advertisementPackage = new BluetoothAdvertisementPackage
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

            CopyManufacturerData(adv);

            CopyDataSection(adv);

            return _advertisementPackage;
        }

        private void CopyDataSection(BluetoothLEAdvertisement adv)
        {
            var dataSection = adv.DataSections;
            foreach (var advertisementDataSection in dataSection)
            {
                var destination = new byte[advertisementDataSection.Data.Length];
                advertisementDataSection.Data.CopyTo(destination);
                _advertisementPackage.Advertisement.DataSections.Add(new BluetoothLEAdvertisementDataSection
                {
                    Data = destination.AsBuffer(),
                    DataType = advertisementDataSection.DataType
                });
            }
        }

        private void CopyManufacturerData( BluetoothLEAdvertisement adv)
        {
            var manufacturer = adv.ManufacturerData;
            foreach (var data in manufacturer)
            {
                var destination = new byte[data.Data.Length];
                data.Data.CopyTo(destination);
                _advertisementPackage.Advertisement.ManufacturerData.Add(new BluetoothLEManufacturerData
                {
                    Data = destination.AsBuffer(),
                    CompanyId = data.CompanyId
                });
            }
        }
    }
}
