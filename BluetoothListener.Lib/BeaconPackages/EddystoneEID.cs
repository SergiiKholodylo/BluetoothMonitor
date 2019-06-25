namespace BluetoothListener.Lib.Packages
{
    public class EddystoneEID : BeaconPackage
    {
        public sbyte RangingData { set; get; }
        public ulong EphemeralIdentifier { set; get; }
        public override string Display()
        {
            return $"Tx (0 m): {RangingData}dB Ephemeral Id: {EphemeralIdentifier:X}";
        }
    }
}
