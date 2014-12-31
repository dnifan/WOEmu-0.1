using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public static class PlayerStats
    {
        public static void SetFightMode(Client c, byte type)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(91);
            w.PushByte(type);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void Toggle(Client c, byte Num, bool b)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(64);

            w.PushByte(Num);
            if (b == true)
                w.PushByte(1);
            else
                w.PushByte(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SetClimbing(Client c, bool climbing)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(1);

            if (climbing)
                w.PushByte(1);
            else
                w.PushByte(0);

            //set the value in player
            c.player.Climbing = climbing;

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SetStance(Client c, string Stance)
        {
            PacketWriter writer = new PacketWriter();

            writer.PushShort(0); //len
            writer.PushByte(58);

            writer.PushByte((byte)Stance.Length);
            writer.PushBytes(Encoding.ASCII.GetBytes(Stance));
            byte[] Tosend = writer.Finish();

            c.Encrypt(Tosend, 0, Tosend.Length);
            c.Send(Tosend);
        }

        public static void SetSpeed(Client c, float Speed)
        {
            PacketWriter writer = new PacketWriter();

            writer.PushShort(0);
            writer.PushByte(61);

            writer.PushFloat(Speed);

            byte[] Tosend = writer.Finish();

            c.Encrypt(Tosend, 0, Tosend.Length);
            c.Send(Tosend);

            c.player.Speed = Speed;
        }

        public static void SendHealthStamina(Client c)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(25);

            w.PushShort(c.Health);
            w.PushShort(c.Stamina);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendFood(Client c)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(63);
            w.PushShort(c.Food);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendWater(Client c)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(49);
            w.PushShort(c.Water);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);           
        }

        /// <summary>
        /// Send the client's stats (food, health, etc.)
        /// </summary>
        /// <param name="c">The client to send to</param>
        public static void SendTo(Client c)
        {
            SendHealthStamina(c);
            SendFood(c);
            SendWater(c);
        }
    }
}
