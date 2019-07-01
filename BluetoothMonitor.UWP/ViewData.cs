using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BluetoothListener.Lib;

namespace BluetoothMonitor.UWP
{
    public class ViewData: IViewData, INotifyPropertyChanged
    {
        private string _mode;
        private long _received;
        private long _dropped;
        private bool _busy;
        private ObservableCollection<BeaconDevice> _devices;

        public ObservableCollection<BeaconDevice> Devices
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
            Devices =  new ObservableCollection<BeaconDevice>();
            ClearData();
        }

        public void ClearData()
        {
            _devices.Clear();
            Mode = "Stop";
            Received = 0L;
            Dropped = 0L;
            Busy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
