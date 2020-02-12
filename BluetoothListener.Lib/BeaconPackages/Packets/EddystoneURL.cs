namespace BluetoothListener.Lib.BeaconPackages.Packets
{
    public class EddystoneUrl : AbstractBeacon,IBeaconPackage
    {
        public EddystoneUrl()
        {
            BeaconPacketType = BeaconPacket.EddystoneUrl;
        }
        public EddystoneUrl(string url):this()
        {
            Url = url;
        }

        public string Url { get; internal set; }

        public override string ToString()
        {
            return $"Url: {EddystoneUrlHelper.FromEddystoneUrl( Url )}";
        }
    }
}
