using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public interface IBluetoothConverter
    {
        IBluetoothAdvertisementPackage ConvertFromBluetoothLeAdvertisementPackage(BluetoothLEAdvertisementReceivedEventArgs e);
    }
}
