namespace BluetoothListener.Lib.BeaconPackages
{
    public class EddystoneUID : IBeaconPackage
    {
        public string NamespaceId { get; internal set; }
        public string InstanceId { get; internal set; }

        public string Display()
        {
            return $"Namespace Id: {NamespaceId} Instance Id: {InstanceId}";
                
        }
    }
}
