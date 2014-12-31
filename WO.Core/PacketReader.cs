using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace WO.Core
{
    public class PacketReader
    {
        public PacketReader(byte[] inc)
        {
            stream = new MemoryStream(inc);
            reader = new BinaryReader(stream);
        }

        public int PopInt()
        {
            return IPAddress.HostToNetworkOrder(reader.ReadInt32());
        }

        public long PopLong()
        {
            return IPAddress.HostToNetworkOrder(reader.ReadInt64());
        }

        public byte[] PopBytes(int size)
        {
            return reader.ReadBytes(size);
        }

        public float PopFloat()
        {
            byte[] _float = reader.ReadBytes(4);
            Array.Reverse(_float);
            return BitConverter.ToSingle(_float, 0);
        }

        public byte PopByte()
        {
            return reader.ReadByte();
        }

        public short PopShort()
        {
            return IPAddress.HostToNetworkOrder(reader.ReadInt16());
        }

        private BinaryReader reader;
        private MemoryStream stream;
    }
}
