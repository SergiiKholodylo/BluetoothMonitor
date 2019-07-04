namespace BluetoothListener.Lib.BeaconPackages
{
    public class EddystoneURL : IBeaconPackage
    {
        public string Url { get; internal set; }

        public string Display()
        {
            return $"Url: {EddystoneUrlHelper.FromEddystoneUrl( Url )}";
        }
    }
}
