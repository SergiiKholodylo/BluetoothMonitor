namespace BluetoothListener.Lib.BeaconPackages
{
    public class ClassicBeaconPackage:IBeaconPackage
    {
        public string ProximityUuid { set; get; }
        public string Major { set; get; }
        public string Minor { set; get; }

        public string Display()
        {
            return $"UUID: {ProximityUuid} Major: {Major} Minor: {Minor}";
        }
    }
}
