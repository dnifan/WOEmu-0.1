using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WO.Core;

namespace WOEmu.Packets
{
    /// <summary>
    /// Send this to a client and it will disconnect automatically.
    /// </summary>
    public static class ServerShuttingDown
    {
        /// <summary>
        /// Send the packet.
        /// </summary>
        /// <param name="c">To this client.</param>
        public static void SendTo(Client c)
        {
            PacketWriter _writer = new PacketWriter();
            _writer.PushShort(0); //len
            _writer.PushByte(7); //shutting down
            byte[] Tosend = _writer.Finish();

            c.Encrypt(Tosend, 0, Tosend.Length);
            c.Send(Tosend);
        }
    }
}
