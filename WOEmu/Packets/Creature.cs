using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class Creature
    {
        /// <summary>
        /// This sets the craeture's border color
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="b">The actor</param>
        /// <param name="ID">(1-6)</param>
        public static void SendIFF(Client c, ActorBase b, byte ID)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(33);

            w.PushLong(b.ID);
            w.PushByte(ID);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendRemove(Client c, long ID)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(40);

            w.PushLong(ID);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Sends the damage of the creature/NPC
        /// </summary>
        /// <param name="c">Client to send the packet to</param>
        /// <param name="a">The actor whose damage has to be sent</param>
        public static void SendDamage(Client c, ActorBase a)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(92);

            w.PushLong(a.ID);
            w.PushFloat(100.0f);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SetColor(Client c, ActorBase b, Color col)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(79);

            w.PushLong(b.ID);
            w.PushFloat(col.R/255.0f); //red
            w.PushFloat(col.G/255.0f); //green
            w.PushFloat(col.B/255.0f); //blue
            w.PushFloat(0.0f);  //alpha value

            w.PushByte(1);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SetScale(Client c, byte xScale, byte yScale, byte zScale, ActorBase b)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(80);

            w.PushLong(b.ID);
            w.PushByte(xScale);
            w.PushByte(yScale);
            w.PushByte(zScale);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
