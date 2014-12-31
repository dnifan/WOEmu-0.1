using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;
using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class WeildTool
    {
        public static void SendTo(Client c, ActorBase a, string Model)
        {
            // This method tells specified client that specified actor is holding something in their right hand.

            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(76);
            w.PushLong(a.ID); // This is the ID of the equipping creature. -1 indicates that it is the player it was sent to.
            w.PushShort((short)Model.Length);
            w.PushBytes(Encoding.UTF8.GetBytes(Model));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
