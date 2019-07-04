using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BluetoothListener.Lib.Dictionaries;

namespace BluetoothListener.Lib
{
    public interface IViewData
    {
        IDictionary <ulong, IBluetoothBeacon> Devices { set; get; }
        string Mode { set; get; }

        long Received { set; get; }

        long Dropped { set; get; }

        bool Busy { set; get; }

        void ClearData();
    }
}
