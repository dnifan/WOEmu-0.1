using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class Walls
    {
        /// <summary>
        /// Sends all the structures in the pool
        /// </summary>
        /// <param name="c">Client to send to</param>
        public static void SendAll(Client c)
        {
            foreach (ObjectBase b in ObjectPool.Pool)
            {
                if (b.Type == ObjectType.Wall)
                {
                    Wall w = (Wall)b;
                    Send(c, w);
                }
            }
        }

        public static void SendOpen(Client c, Wall wa, bool status)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(56);

            w.PushShort((short)wa.Position.X);
            w.PushShort((short)wa.Position.Y);
            w.PushByte((byte)(wa.flipped == true ? 2 : 0));

            w.PushByte((byte)(status == true ? 1 : 0));
            w.PushByte((byte)(status == true ? 1 : 0));

            wa.Open = status;

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void Send(Client c, Wall wa)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(54);

            w.PushShort((short)wa.Position.X);
            w.PushShort((short)wa.Position.Y);
            
            w.PushByte((byte)(wa.flipped == true ? 2 : 0)); //flipped?
            w.PushByte(wa.WallType);
            w.PushByte(1); //seems to be always one?

            w.PushByte(0); //Colored.

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendRemove(Client c, Wall wa)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(55);

            w.PushShort((short)wa.Position.X);
            w.PushShort((short)wa.Position.Y);
            w.PushByte((byte)(wa.flipped == true ? 2 : 0));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
