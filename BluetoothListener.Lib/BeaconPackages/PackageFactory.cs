using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BluetoothListener.Lib.Packages
{
    public class PackageFactory
    {
        private const int EddystoneMinHeaderSize = 4;
        private const int BeaconUuidMinSize = 0x15;
        private const int EddystoneIdMinSize = 0x14;
        private const int EddystonePackageDataOffset = 0x04;

        public static BeaconPackage CreatePackageFromManufacturerPayload(byte[] data)
        {
            if (data.Length < BeaconUuidMinSize) 
                throw new PackageException($"Wrong payload {Utils.PrintArray(data, "")}");
            var major = data[0x12] * 0x100 + data[0x13];
            var minor = data[0x14] * 0x100 + data[0x15];

            byte[] byteGuid = ExtractGuidBytesArrayFromPayload(data);

            var guid = GetGuidFromBytesArray(byteGuid);

            var proximityUuid = guid.ToString("N").ToUpper();

            return new ClassicBeaconPackage
            {
                ProximityUuid = proximityUuid,
                Major = major.ToString("D"), //X4
                Minor = minor.ToString("D")
            };
        }

        public static BeaconPackage CreatePackageFromDataPayload(byte[] data)
        {
            if (data.Length <= EddystoneMinHeaderSize)
                throw new PackageException($"Wrong payload {Utils.PrintArray(data, "")}"); 

            Debug.WriteLine($"ParseData ({data.Length} bytes):{Utils.PrintArray(data, "")}");

            if (!HasKontaktIoSignature(data))
                throw new PackageException($"Payload Has Not Got a Signature!");

            Debug.WriteLine($"EddystonePacketType: {data[2]:X}");

            var eddystonePacketType = data[2];

            switch (eddystonePacketType)
            {
                case (byte)EddystonePacketType.EddystoneId:
                    return ParseEddystoneId( data );

                case (byte)EddystonePacketType.EddystoneUrl:
                    return ParseEddystoneUrl( data );
                default:
                    throw new PackageException($"Wrong Eddystone Package Type {eddystonePacketType:X}");
            }
        }

        private static BeaconPackage ParseEddystoneUrl( byte[] data )
        {
            var url = new byte[data.Length - EddystonePackageDataOffset];
            for (var j = EddystonePackageDataOffset; j < data.Length; j++)
                url[j - EddystonePackageDataOffset] = data[j];
            var eddystoneFrame = BitConverter.ToString(url).Replace("-", "");
            return new EddystoneURL
            {
                Url = eddystoneFrame
            };
        }

        private static BeaconPackage ParseEddystoneId( byte[] data )
        {
            if (data.Length < EddystoneIdMinSize)
                throw new PackageException($"Wrong payload {Utils.PrintArray(data, "")}");

            var beaconIdArray = new byte[EddystoneIdMinSize - EddystonePackageDataOffset];

            for (var j = EddystonePackageDataOffset; j < EddystoneIdMinSize; j++)
                beaconIdArray[j - EddystonePackageDataOffset] = data[j];

            var eddystoneFrame = BitConverter.ToString(beaconIdArray).Replace("-", "");

            return new EddystoneUID
            {
                NamespaceId = eddystoneFrame.Substring(0, 20),
                InstanceId = eddystoneFrame.Substring(20)
            };
        }

        private static bool HasKontaktIoSignature(byte[] data)
        {
            return data[0] == 0xAA && data[1] == 0xFE;
        }

        private static byte[] ExtractGuidBytesArrayFromPayload(byte[] data)
        {
            var byteGuid = new byte[0x10];

            for (var i = 0x02; i < 0x12; i++)
            {
                byteGuid[i - 2] = data[i];
            }

            return byteGuid;
        }

        private static Guid GetGuidFromBytesArray(byte[] byteGuid)
        {
            Utils.Swap(ref byteGuid[0], ref byteGuid[3]);
            Utils.Swap(ref byteGuid[1], ref byteGuid[2]);
            Utils.Swap(ref byteGuid[4], ref byteGuid[5]);
            Utils.Swap(ref byteGuid[6], ref byteGuid[7]);

            var guid = new Guid(byteGuid);
            return guid;
        }
    }

    public enum EddystonePacketType : byte
    {
        EddystoneId = 0x00,
        EddystoneUrl = 0x10
    }
}
