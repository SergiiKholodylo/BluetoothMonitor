namespace BluetoothListener.Lib.BeaconPackages
{
    public class EddystoneTlm : IBeaconPackage
    {
        public byte Version { set; get; }
        public ushort BatteryVoltage { set; get; }
        public short BeaconTemperature { set; get; }
        public uint AdvertisementPduCountSinceBoot { set; get; }
        public uint TimeSinceBoot { get; set; }




        public string Display()
        {
            return $"Version: {Version}  Battery: {BatteryVoltage} mV \nTemperature: {BeaconTemperature}\nSent: {AdvertisementPduCountSinceBoot} Work: {TimeSinceBoot/10} sec";
        }
    }
}
