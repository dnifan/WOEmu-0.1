using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public static class Teleport
    {
        public static void SendTo(Client c, Vector3 pos, float rot)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(50);

            w.PushFloat(pos.Y);
            w.PushFloat(pos.X);
            w.PushFloat(pos.Z);
            w.PushFloat(rot);

            w.PushByte(0);
            w.PushByte(0);
            w.PushByte(0);
            w.PushByte(0);

            c.player.Position = pos;

            //We need to implement a broadcasting thing here for the other clients.

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
