using System;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib
{   
    public class BluetoothReceiver: IBluetoothReceiver

    {
        private readonly BluetoothLEAdvertisementWatcher _watcher;

        private bool _isListening;

        public event AdvertisementReceivedHandler AdvertisementReceived;

        public BluetoothReceiver()
        {
            _watcher = CreateWatcherWithSettings();
        }

        public void StartListening()
        {
            if (_isListening) return;

            SubscribeHandlers();

            _watcher.Start();

            _isListening = true;
        }
        public void StopListening()
        {
            if (!_isListening) return;

            _watcher.Stop();
            UnsubscribeHandlers();
            _isListening = false;
        }

        protected BluetoothLEAdvertisementWatcher CreateWatcherWithSettings()
        {
            return new BluetoothLEAdvertisementWatcher
            {
                SignalStrengthFilter =
                {
                    InRangeThresholdInDBm = -70,
                    OutOfRangeThresholdInDBm = -75,
                    OutOfRangeTimeout = TimeSpan.FromMilliseconds(2000)
                }
            };

            //var manufacturerData = new BluetoothLEManufacturerData { CompanyId = 0x004C };
            //var manufacturerData = new BluetoothLEManufacturerData { CompanyId = 0xFEAA };
            //var writer = new DataWriter();
            //writer.WriteUInt16(0x0215);
            //manufacturerData.Data = writer.DetachBuffer();

            //watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);
        }

        private void UnsubscribeHandlers()
        {
            _watcher.Received -= OnAdvertisementReceived;
            _watcher.Stopped -= OnAdvertisementWatcherStopped;
        }
        private void SubscribeHandlers()
        {
            _watcher.Received += OnAdvertisementReceived;
            _watcher.Stopped += OnAdvertisementWatcherStopped;
        }
        private void OnAdvertisementWatcherStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            _isListening = false;
            UnsubscribeHandlers();
        }
        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            AdvertisementReceived?.Invoke(sender,args);
        }


    }
}
