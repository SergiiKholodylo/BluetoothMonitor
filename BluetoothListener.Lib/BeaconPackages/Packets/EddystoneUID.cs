using System.Diagnostics;

namespace BluetoothListener.Lib.BeaconPackages.Packets
{

    [DebuggerDisplay("Eddystone Uid Namespace:{NamespaceId} Instance:{InstanceId} ")]
    public class EddystoneUid : AbstractBeacon, IBeaconPackage
    {
        public EddystoneUid()
        {
            BeaconPacketType = BeaconPacket.EddystoneUid;
        }

        public EddystoneUid(string namespaceId, string instanceId) : this()
        {
            NamespaceId = namespaceId;
            InstanceId = instanceId;
        }

        public string NamespaceId { get; internal set; }
        public string InstanceId { get; internal set; }

        public override string ToString()
        {
            return $"Namespace Id: {NamespaceId} Instance Id: {InstanceId}";
        }
    }
}
