using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib.BluetoothAdvertisement
{
    public interface IBluetoothConverter
    {
       IBluetoothAdvertisementPackage ConvertFromBluetoothLeAdvertisementPackage(BluetoothLEAdvertisementReceivedEventArgs e);

       
    }
}
