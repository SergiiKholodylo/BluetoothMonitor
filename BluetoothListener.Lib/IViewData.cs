using System.Collections.ObjectModel;

namespace BluetoothListener.Lib
{
    public interface IViewData
    {
        ObservableCollection<BeaconDevice> Devices { set; get; }
        string Mode { set; get; }

        long Received { set; get; }

        long Dropped { set; get; }

        bool Busy { set; get; }

        void ClearData();
    }
}
