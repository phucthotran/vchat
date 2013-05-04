using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Module.VoIP
{
    public class DataPacket
    {
        public String Name { get; set; }
        public CallCommand Command { get; set; }

        public DataPacket()
        {
            this.Name = null;
            this.Command = CallCommand.Null;
        }

        public DataPacket(byte[] Data)
        {
            this.Command = (CallCommand)BitConverter.ToInt32(Data, 0);
            int nameLen = BitConverter.ToInt32(Data, 4);

            if (nameLen > 0)
                this.Name = Encoding.UTF8.GetString(Data, 8, nameLen);
            else
                this.Name = null;
        }

        public byte[] ToByte()
        {
            List<Byte> byteData = new List<Byte>();

            byteData.AddRange(BitConverter.GetBytes((int)Command));

            if (Name != null)
                byteData.AddRange(BitConverter.GetBytes(Name.Length));
            else
                byteData.AddRange(BitConverter.GetBytes(0));

            if (Name != null)
                byteData.AddRange(Encoding.UTF8.GetBytes(Name));

            return byteData.ToArray();
        }
    }
}
