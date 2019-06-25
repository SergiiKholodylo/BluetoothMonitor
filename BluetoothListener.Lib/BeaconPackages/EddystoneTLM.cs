namespace BluetoothListener.Lib.Packages
{
    public class EddystoneTlm : BeaconPackage
    {
        public byte Version { set; get; }
        public ushort BatteryVoltage { set; get; }
        public ushort BeaconTemperature { set; get; }
        public uint AdvertisementPduCountSinceBoot { set; get; }
        public uint TimeSinceBoot { get; set; }




        public override string Display()
        {
            return $"Version: {Version}  Battery: {BatteryVoltage} mV \nTemperature: {BeaconTemperature}\nSent: {AdvertisementPduCountSinceBoot} Work: {TimeSinceBoot/10} sec";
        }
    }
}
