using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public static class InterfaceOptions
    {
        public static void SendCompass(Client c)
        {
            PacketWriter w = new PacketWriter();
            w.PushShort(0);
            w.PushByte(6);

            w.PushInt(0);   //Compass
            w.PushInt(1);   //Some unknown variable

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
