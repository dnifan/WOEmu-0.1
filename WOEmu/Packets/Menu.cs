using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// Produces a very ugly 'menu' in the client. Probably
    /// some old deprecated packet.
    /// </summary>
    public static class Menu
    {
        /// <summary>
        /// Send the menu
        /// </summary>
        /// <param name="c">Client</param>
        /// <param name="caption">Window caption</param>
        /// <param name="body">Menu body, may contain HTML code.</param>
        public static void SendTo(Client c, string caption, string body)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(19);

            w.PushByte((byte)caption.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(caption));

            w.PushShort((short)body.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(body));

            byte[] t = w.Finish();

            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
