using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

using WOEmu.Objects;

namespace WOEmu.Packets
{
    public enum SpriteType
    {
        SmokeAndFlames, //0
        Lightning,        //1
        Plasma,         //2
        PlasmaAltered,  //3
        Smoke           //4
    }

    public static class Sprite
    {
        public static void Add(Client c, long ID, short spriteID, Vector3 position)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(45);

            w.PushLong(ID);
            w.PushShort(spriteID);
            w.PushFloat(position.Y); //coords
            w.PushFloat(position.X);
            w.PushFloat(position.Z);
            w.PushSByte((sbyte)-1);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void Remove(Client c, long ID)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(46);

            w.PushLong(ID);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
