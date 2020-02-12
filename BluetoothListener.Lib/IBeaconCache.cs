namespace BluetoothListener.Lib
{
    public interface IBeaconCache
    {
        void AddOrUpdate(IBluetoothBeacon beacon);
        void Clear();
    }
}