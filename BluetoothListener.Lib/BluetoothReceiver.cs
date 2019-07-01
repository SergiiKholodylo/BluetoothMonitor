using System;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothListener.Lib
{   
    public class BluetoothReceiver: IBluetoothReceiver

    {
        private readonly BluetoothLEAdvertisementWatcher _watcher;

        private bool _isActive;

        public event AdvertisementReceivedHandler AdvertisementReceived;

        public BluetoothReceiver()
        {
            _watcher = CreateWatcherWithSettings();
        }

        public void StartListening()
        {
            if (_isActive) return;
            _isActive = true;
            SubscribeHandlers();
            _watcher.Start();
        }
        public void StopListening()
        {
            if (!_isActive) return;
            _isActive = false;
            _watcher.Stop();
            UnsubscribeHandlers();
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
            _isActive = false;
            UnsubscribeHandlers();
        }
        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            AdvertisementReceived?.Invoke(sender,args);
        }


    }
}
