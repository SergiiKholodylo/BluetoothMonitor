namespace BluetoothListener.Lib.BeaconPackages
{
    public class EddystoneEID : IBeaconPackage
    {
        public sbyte RangingData { set; get; }
        public ulong EphemeralIdentifier { set; get; }
        public string Display()
        {
            return $"Tx (0 m): {RangingData}dB Ephemeral Id: {EphemeralIdentifier:X}";
        }
    }
}
