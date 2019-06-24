namespace BluetoothListener.Lib.Packages
{
    public class ClassicBeaconPackage:BeaconPackage
    {
        public string ProximityUuid { set; get; }
        public string Major { set; get; }
        public string Minor { set; get; }

        public override string Display()
        {
            return $"UUID: {ProximityUuid} Major: {Major} Minor: {Minor}";
        }
    }
}
