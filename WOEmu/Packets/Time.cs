using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public static class Time
    {
        public static void SendTo(Client c)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(72);

            double secs = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            long time = ((long)secs) * 1000;
            WO.Core.Logger.Logger.printDebug("Time: " + time);

            w.PushLong(time);

            int degrees = (((3600*(DateTime.Now.Hour)) % 86400) * 259) / 86400;
            w.PushLong((long)(((1000*(8*degrees))/24)));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendTo(Client c, long sun)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(72);

            double secs = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            long time = ((long)secs) * 1000;
            WO.Core.Logger.Logger.printDebug("Time: " + time);

            w.PushLong(time);
            w.PushLong(sun);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
