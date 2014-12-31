using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;

namespace WO.Core
{
    public class PacketWriter
    {
        public PacketWriter()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public void PushSByte(sbyte b)
        {
            writer.Write(b);
        }

        public void Dispose()
        {
            stream.Close();
            writer.Close();
        }
           
        public void PushByte(byte p)
        {
            writer.Write(p);
        }

        public void PushInt(int p)
        {
            writer.Write(IPAddress.HostToNetworkOrder(p));
        }

        public void PushLong(long p)
        {
            writer.Write(IPAddress.HostToNetworkOrder(p));
        }

        public void PushFloat(float p)
        {
            byte[] _f = BitConverter.GetBytes(p);
            Array.Reverse(_f);
            writer.Write(_f);
        }

        public void PushShort(short p)
        {
            writer.Write(IPAddress.HostToNetworkOrder(p));
        }

        public void PushBytes(byte[] p)
        {
            writer.Write(p);
        }

        public byte[] Finish()
        {
            stream.Capacity = (int)stream.Length;
            byte[] Packet;

            Packet = stream.GetBuffer();
            writer.Close();
            stream.Dispose();

            byte[] l_length = BitConverter.GetBytes((short)(Packet.Length - 2));
            Packet[0] = l_length[1];
            Packet[1] = l_length[0];
            
            return Packet;
        }

        private MemoryStream stream;
        private BinaryWriter writer;
    }
}
