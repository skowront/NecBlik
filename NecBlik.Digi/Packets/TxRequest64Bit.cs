using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBeeLibrary.Core.Models;

namespace NecBlik.Digi.Packets
{
    public class TxRequest64Bit : XBeeLibrary.Core.Packet.XBeePacket
    {
        protected override LinkedDictionary<string, string> PacketParameters => new LinkedDictionary<string, string>();

        private string address64bit = "0000000000000000";
        private OperatingMode mode = OperatingMode.API;

        public TxRequest64Bit(string address,OperatingMode mode)
        {
            this.mode = mode;
            if (address == string.Empty || address.Length != 16)
                throw new ArgumentException();
            this.address64bit = address;
        }

        public override byte[] GetPacketData()
        {
            byte[] data = new byte[] { 
                                       0x7E, //Start delimiter
                                       0x00, //Length
                                       0x0F, //Length
                                       0x10, //Frame type
                                       0x01, //Frame ID
                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //64bit address
                                       0xFF, 0xFE, //16 Bit address, left as unknown == 0xFFFE
                                       0x00, //Broadcast Radius
                                       0x00, //Options
                                       0x30, //Payload ascii char '0'
                                       0xFF  //Checksum
                                     };             
            var add = StringToByteArray(this.address64bit);
            for(int i = 0; i<add.Length;i++)
            {
                data[i+5] = add[i];
            }
            for (int i = 3; i < data.Length - 1; i++)
            {
                data[data.Length - 1] -= data[i];
            }
            if (this.mode == OperatingMode.API_ESCAPE)
            {
                List<byte> list = new List<byte>(data);
                for (int i = 4; i < list.Count - 1; i++)
                {
                    if(list[i]==0x7E || list[i] == 0x7D || list[i]==0x11 || list[i] == 0x13)
                    {
                        list.Insert(i, 0x7D);
                        list[i + 1] = (byte)(list[i + 1] ^ 0x20);
                    }
                }
                data = list.ToArray();
            }
            
            return data;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }
    }
}
