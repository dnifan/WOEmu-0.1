using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Objects
{
    public static class WallTypes
    {
        public static byte WoodenFence = 0;
        public static byte Palisade = 1;
        public static byte Stone_Wall = 2;
        public static byte Wooden_Fence_Gate = 3;
        public static byte Palisade_Gate = 4;
        public static byte Tall_Stone_Wall = 5;
        public static sbyte I_Wooden_Fence = -1;
        public static sbyte I_Palisade = -2;
        public static sbyte I_Stone_Wall = -3;
        public static sbyte I_Palisade_Gate = -4;
        public static sbyte I_Wooden_Fence_Gate = -5;
        public static sbyte I_Tall_Stone_Wall = -6;
    }

    public class Wall : ObjectBase
    {
        public void saveToDatabase()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("INSERT INTO walls VALUES(null," + (short)Position.X + "," + (short)Position.Y + ",+" + this.WallType + "," + (this.flipped == true ? 1 : 0) + ");");
            s.Dispose();
        }

        public Wall(short X, short Y, byte type, bool f)
        {
            WallType = type;
            Position = new Vector3((float)X, (float)Y, 0.0f);
            flipped = f;
            Open = false;
        }

        public bool flipped;
        public byte WallType;
        public bool Open;
    }
}
