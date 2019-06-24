namespace BluetoothListener.Lib.Packages
{
    public class EddystoneURL : BeaconPackage
    {
        public string Url { get; internal set; }

        public override string Display()
        {
            return $"Url: {EddystoneUrlHelper.FromEddystoneUrl( Url )}";
        }
    }
}
