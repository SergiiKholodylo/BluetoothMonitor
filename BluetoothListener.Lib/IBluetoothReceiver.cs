using Windows.Devices.Bluetooth.Advertisement;
using BluetoothListener.Lib.BluetoothAdvertisement;

namespace BluetoothListener.Lib
{
    public delegate void AdvertisementReceivedHandler(IBluetoothAdvertisementPackage package);
    public interface IBluetoothReceiver
    {
        event AdvertisementReceivedHandler AdvertisementReceived;
        void StartListening();
        void StopListening();

    }
}
