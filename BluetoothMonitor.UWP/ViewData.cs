using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BluetoothListener.Lib;
using BluetoothListener.Lib.Dictionaries;

namespace BluetoothMonitor.UWP
{
    public class ViewData: IViewData, INotifyPropertyChanged
    {
        private string _mode;
        private long _received;
        private long _dropped;
        private bool _busy;
        private IDictionary<ulong, IBluetoothBeacon> _devices = new ObservableConcurrentDictionary<ulong, IBluetoothBeacon>();

        public IDictionary<ulong, IBluetoothBeacon> Devices
        {
            get => _devices;
            set
            {
                _devices = value; 
                NotifyPropertyChanged();
            }
        }

        public string Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        public long Received
        {
            get => _received;
            set
            {
                _received = value;
                NotifyPropertyChanged();
            }
        }

        public long Dropped
        {
            get => _dropped;
            set
            {
                _dropped = value;
                NotifyPropertyChanged();
            }
        }

        public bool Busy
        {
            get => _busy;
            set
            {
                _busy = value; 
                NotifyPropertyChanged();
            }
        }

        public ViewData()
        {
            ClearData();
        }

        public void ClearData()
        {
            Devices.Clear();
            Mode = "Stopped";
            Received = 0L;
            Dropped = 0L;
            Busy = false;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
