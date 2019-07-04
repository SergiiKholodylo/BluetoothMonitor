using System;
using System.Diagnostics;

namespace BluetoothListener.Lib.BeaconPackages
{
    /* Eddystone package format -  https://github.com/google/eddystone */

    public class PackageFactory
    {
        private const int EddystoneMinHeaderSize = 4;
        private const int BeaconUuidMinSize = 0x15;
        private const int EddystoneIdMinSize = 0x14;
        private const int EddystonePackageDataOffset = 0x04;
        private const int EddystoneTlmPackageSize = 0x10;
        private const int EddystoneEidPackageSize = 0x0C;

        private const int EncryptedEddystoneTlm = 0x01;

        private const byte BeaconSubtype = 0x02;

        public static IBeaconPackage CreatePackageFromManufacturerPayload(byte[] data)
        {
            /*
            Byte 3: Length: 0x1a
            Byte 4: Type: 0xff (Custom Manufacturer Packet)
            Byte 5-6: Manufacturer ID : 0x4c00 (Apple)
            Byte 7: SubType: 0x02 (iBeacon)
            Byte 8: SubType Length: 0x15
            Byte 9-24: Proximity UUID
            Byte 25-26: Major
            Byte 27-28: Minor
            Byte 29: Signal Power             
             */
            Debug.WriteLine($"Manufacturer Payload ({data.Length} bytes):{Utils.PrintArray(data, "")}");
            if (data.Length < BeaconUuidMinSize) 
                throw new PackageException($"Wrong payload {Utils.PrintArray(data, "")}");
            var packageSubtype = data[0];
            if(packageSubtype != BeaconSubtype)
                throw new PackageException($"The packet is not iBeacon {data[0]}");
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

        public static IBeaconPackage CreatePackageFromDataPayload(byte[] data)
        {
            Debug.WriteLine($"Data Payload ({data.Length} bytes):{Utils.PrintArray(data, "")}");

            if (data.Length <= EddystoneMinHeaderSize)
                throw new PackageException($"Wrong payload {Utils.PrintArray(data, "")}"); 

            

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

                case (byte)EddystonePacketType.EddystoneTlm:
                    return ParseEddystoneTlm(data);

                case (byte)EddystonePacketType.EddystoneEid:
                    return ParseEddystoneEid(data);

                default:
                    throw new PackageException($"Wrong Eddystone Package Type {eddystonePacketType:X}");
            }
        }

        protected static IBeaconPackage ParseEddystoneEid(byte[] data)
        {
            if (data.Length != EddystoneEidPackageSize)
                throw new PackageException($"Wrong Eddystone EID size {data.Length} byte(s)");
            var rangingData = (sbyte)data[3];
            var ephemeralIdentifier = BitConverter.ToUInt64(data, 4);
            return new EddystoneEID
            {
                RangingData = rangingData,
                EphemeralIdentifier = ephemeralIdentifier
            };
        }

        protected static IBeaconPackage ParseEddystoneTlm(byte[] data)
        {
            if(data.Length!=EddystoneTlmPackageSize)
                throw new PackageException($"Wrong Eddystone TLM size {data.Length} byte(s)");
            var version = data[3];
            if(version == EncryptedEddystoneTlm)
                throw new PackageException($"Encrypted Eddystone TLM doesn't support!");
            var batteryVoltage = BitConverter.ToUInt16(data,4);
            var temperature = BitConverter.ToInt16(data, 6);
            var advertisementCount = BitConverter.ToUInt32(data, 8);
            var timeSinceBoot = BitConverter.ToUInt32(data, 12);

            return new EddystoneTlm
            {
                Version = version,
                BatteryVoltage = batteryVoltage,
                BeaconTemperature = temperature,
                AdvertisementPduCountSinceBoot = advertisementCount,
                TimeSinceBoot = timeSinceBoot
            };
        }

        private static IBeaconPackage ParseEddystoneUrl( byte[] data )
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

        private static IBeaconPackage ParseEddystoneId( byte[] data )
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
        EddystoneUrl = 0x10,
        EddystoneTlm = 0x20,
        EddystoneEid = 0x30
    }
}
