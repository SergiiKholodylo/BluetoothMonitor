using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib
{
    public delegate void AdvertisementReceivedHandler(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args);
    public interface IBluetoothReceiver
    {
        event AdvertisementReceivedHandler AdvertisementReceived;
        void StartListening();
        void StopListening();

    }
}
