using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public static class Sound
    {
        public static void SendPlay(Client c, string sound)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(57);

            w.PushByte((byte)sound.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(sound));

            w.PushFloat(c.player.Position.X);
            w.PushFloat(c.player.Position.Y);
            w.PushFloat(c.player.Position.Z);

            w.PushFloat(1.0f);
            w.PushFloat(1.0f);
            w.PushFloat(5.0f);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
