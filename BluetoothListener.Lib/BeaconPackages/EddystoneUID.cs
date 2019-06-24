namespace BluetoothListener.Lib
{
    public class EddystoneUID : BeaconPackage
    {
        public string NamespaceId { get; internal set; }
        public string InstanceId { get; internal set; }

        public override string Display()
        {
            return $"Namespace Id: {NamespaceId} Instance Id: {InstanceId}";
                
        }
    }
}
