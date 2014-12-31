using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Packets;
using System.Text;

namespace WOEmu.PacketHandlers
{
    public static class StanceHandler
    {
        public enum Stance
        {
            Climbing,
            Faithful,
            Lawful,
            Stealth,
            Autofight
        }

        public static void Read(Client c, byte[] Packet)
        {
            PacketReader r = new PacketReader(Packet);

            r.PopByte();

            byte b1 = r.PopByte();
            byte b2 = r.PopByte();

            switch ((Stance)b1)
            {
                case Stance.Climbing:
                    if (c.player.Climbing)
                    {
                        PlayerStats.SetClimbing(c, false);
                        PlayerStats.Toggle(c, b1, false);
                    }
                    else
                    {
                        PlayerStats.SetClimbing(c, true);
                        PlayerStats.Toggle(c, b1, true);
                    }
                    System.Console.WriteLine("Player wants to climb!");
                    break;

                case Stance.Faithful:
                    System.Console.WriteLine("Player wants to be faithful");
                    break;

                case Stance.Lawful:
                    System.Console.WriteLine("Player wants to be lawful");
                    break;
            }

            System.Console.WriteLine("Byte1: {0}, Byte2: {1}", b1, b2);
        }
    }
}
