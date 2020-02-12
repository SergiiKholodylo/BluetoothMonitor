using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib
{
    public interface IBluetoothReceiver
    {
        event AdvertisementReceivedHandler AdvertisementReceived;
        void StartListening();
        void StopListening();

    }
}
