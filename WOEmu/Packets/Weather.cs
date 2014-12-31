using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public static class Weather
    {
        public static void sendWindSpeed(Client c, byte s)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(94);

            w.PushByte(s);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void sendRowingSpeed(Client c, byte s)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(96);

            w.PushByte(s);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void GenerateWeather()
        {
            Random cloudRandom = new Random();
            Random fogRandom = new Random();
            Random rainRandom = new Random();
            Random windxRandom = new Random();
            Random windyRandom = new Random();

            Clouds = (float)cloudRandom.NextDouble();
            Fog = (float)fogRandom.NextDouble();
            Rain = (float)rainRandom.NextDouble();
            WindX = (float)windxRandom.NextDouble();
            WindY = (float)windyRandom.NextDouble();
            Unknown = 0.0f;
            Unknown2 = 0.0f;
        }

        public static void SendTo(Client c)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(62);

            w.PushFloat(Clouds);
            w.PushFloat(Fog);
            w.PushFloat(Rain);
            w.PushFloat(WindX);
            w.PushFloat(WindY);
            w.PushFloat(Unknown);//effects nothing ?
            w.PushFloat(Unknown2);//seems to affect darkness

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static float Clouds = 0.0f;
        public static float Fog = 0.0f;
        public static float Rain = 0.0f;
        public static float WindX = 0.0f;
        public static float WindY = 0.0f;
        public static float Unknown = 0.0f;
        public static float Unknown2 = 0.0f;
    }
}
