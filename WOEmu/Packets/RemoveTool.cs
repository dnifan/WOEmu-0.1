using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;
using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class RemoveTool
    {
        public static void SendTo(Client c, ActorBase a)
        {
            // This method tells specified client to empty specified actor's right hand.

            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(77);
            w.PushLong(a.ID); // This is the ID of the equipping creature. -1 indicates that it is the player it was sent to.

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
