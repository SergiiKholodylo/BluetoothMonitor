namespace BluetoothListener.Lib.BeaconPackages.Packets
{
    public class ClassicBeaconPackage: AbstractBeacon, IBeaconPackage
    {
        public ClassicBeaconPackage()
        {
            BeaconPacketType = BeaconPacket.Classic;
        }
        public ClassicBeaconPackage(string proximityUuid, string major, string minor):this()
        {
            ProximityUuid = proximityUuid;
            Major = major;
            Minor = minor;
        }

        public string ProximityUuid { set; get; }
        public string Major { set; get; }
        public string Minor { set; get; }

        public override string ToString()
        {
            return $"UUID: {ProximityUuid} Major: {Major} Minor: {Minor}";
        }
    }
}
