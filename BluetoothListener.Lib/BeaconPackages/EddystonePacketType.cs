namespace BluetoothListener.Lib.BeaconPackages
{
    public enum EddystonePacketType : byte
    {
        EddystoneId = 0x00,
        EddystoneUrl = 0x10,
        EddystoneTlm = 0x20,
        EddystoneEid = 0x30
    }
}