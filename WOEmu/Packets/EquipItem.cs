using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;
using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class EquipItem
    {
        public static void SendTo(Client c, ActorBase a, byte Position, string Model)
        {
            // This method tells specified client that specified player has equipped something
            // This packet does not seem to be fully implemented in the client, as specifying a position
            // other than 0 on most models does not seem to have an effect. To make a player equip a weapon
            // or tool, you should use the WeildTool class.

            PacketWriter w = new PacketWriter();

            w.PushByte(75);
            w.PushLong(a.ID); // This is the ID of the equipping creature. -1 indicates that it is the player it was sent to.
            w.PushByte(Position);
            w.PushShort((short)Model.Length);
            w.PushBytes(Encoding.UTF8.GetBytes(Model));
            
            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
