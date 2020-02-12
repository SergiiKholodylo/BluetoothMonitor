namespace BluetoothListener.Lib.BeaconPackages.Packets
{
    public class EddystoneTlm : AbstractBeacon,IBeaconPackage
    {
        public EddystoneTlm()
        {
            BeaconPacketType = BeaconPacket.EddystoneTlm;
        }

        public EddystoneTlm(byte version, ushort batteryVoltage, sbyte beaconTemperature, uint advertisementPduCountSinceBoot, uint timeSinceBoot):this()
        {
            Version = version;
            BatteryVoltage = batteryVoltage;
            BeaconTemperature = beaconTemperature;
            AdvertisementPduCountSinceBoot = advertisementPduCountSinceBoot;
            TimeSinceBoot = timeSinceBoot;
        }

        public byte Version { set; get; }
        public ushort BatteryVoltage { set; get; }
        public sbyte BeaconTemperature { set; get; }
        public uint AdvertisementPduCountSinceBoot { set; get; }
        public uint TimeSinceBoot { get; set; }




        public override string ToString()
        {
            return $"Version: {Version}  Battery: {BatteryVoltage} mV \nTemperature: {BeaconTemperature}\nSent: {AdvertisementPduCountSinceBoot} Work: {TimeSinceBoot/10} sec";
        }
    }
}
