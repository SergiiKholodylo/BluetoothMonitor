namespace BluetoothListener.Lib.BeaconPackages.Packets
{
    public class EddystoneEID : AbstractBeacon, IBeaconPackage
    {
        public EddystoneEID()
        {
            BeaconPacketType = BeaconPacket.EddystoneEid;
        }

        public EddystoneEID(sbyte rangingData, ulong ephemeralIdentifier):this()
        {
            RangingData = rangingData;
            EphemeralIdentifier = ephemeralIdentifier;
        }

        public sbyte RangingData { set; get; }
        public ulong EphemeralIdentifier { set; get; }

        public override string ToString()
        {
            return $"Tx (0 m): {RangingData}dB Ephemeral Id: {EphemeralIdentifier:X}";
        }
    }
}
