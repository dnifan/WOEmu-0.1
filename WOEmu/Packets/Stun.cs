using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public static class Stun
    {
        public static void Send(Client c, bool stunned)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(14);
            w.PushByte((byte)(stunned == true ? 1 : 0));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
