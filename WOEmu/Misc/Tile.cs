using System;
using System.Collections.Generic;
using System.Text;
using WO.Core;

namespace WOEmu.Misc
{
    /// <summary>
    /// Contains all possible tiletypes
    /// </summary>
    public enum TileType
    {
        Hole, //0
        Sand,
        Grass,
        Tree,
        Rock,
        Dirt,
        Clay,
        Field,
        PackedDirt,
        CobbleStone,
        Mycelium,
        InfectedTree,
        Lava,
        EnchantedGrass,
        EnchantedTree,
        WoodenPlanks,
        StoneSlabs,
        Gravel,
        Peat,
        Tundra,
        Moss,
        Cliff,
        Steppe,
        Marsh,
        Tar,
        Door,
        Rock2,
        DoorGold,
        DoorSilver,
        DoorSteel,

        Cave = 200,
        Cave2,
        CaveWall,
        ReinforcedCaveWall,

        GoldVein = 220,
        SilverVein,
        IronVein,
        CopperVein,
        LeadVein,
        ZincVein,
        TinVein
    }

    /// <summary>
    /// All tree types
    /// </summary>
    public enum TreeType
    {
        Birch,
        Pine,
        Oak,
        Cedar,
        Willow,
        Maple,
        Apple,
        Lemon,
        Olive,
        Cherry,
        Lavender,
        Rose,
        Thorn,
        Grape,
        Camellia,
        Oleander
    }

    public class Tile
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <param name="Z">Height</param>
        public Tile(short x, short y, short Z)
        {
            X = x;
            Y = y;
            Height = Z;
            NormalTile = true;
            Age = 0;
        }

        /// <summary>
        /// Set tiletype
        /// </summary>
        /// <param name="t">The tiletype</param>
        public void SetType(TileType t)
        {
            Type = t;
        }

        public void SetAge(byte age)
        {
            Age = age;
        }

        /// <summary>
        /// Returns string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Type.ToString() + ": [" + X + ", " + Y + ", " + Height + "]";
        }

        /// <summary>
        /// Return tile flags
        /// </summary>
        /// <returns>int to push in bytebuffer</returns>
        public int TileFlags()
        {
            //return (Height * 10) + ((int)Type << 16);

            //we still need a byte value somewhere, but I dont know what it is
            //after llooking in the client code, your AGE variable ISNT age, it is the " subtype "
            // say the Type was of Tree, then what you have called " Age " here would actually be wether it is a Maple/Lemon/Oak/Ceder etc.... not AGE
            // i has found your missing byte too wich is the " REAL " age so you wanna implement something liek
            // return (Height * 10) + ((int)Type << 24) + (SubType << 16) + (Age << 4);
            //age = [0: New, 1-3: Young, 4-7: Mature, 8-11: Old, 12-14: Very old, 15: Shriveled]
            return (Height * 10) + ((int)Type << 24) + (SubType << 16);
        }

        /// <summary>
        /// Retrieves not tile positions, but real positions
        /// </summary>
        /// <returns>Vector</returns>
        public Vector3 GetRealPos()
        {
            return new Vector3(X * 4, Y * 4, Height);
        }

        public short X;
        public short Y;
        public short Height;
        public TileType Type;
        /// <summary>
        /// Normal tile, instead of the border types.
        /// </summary>
        public bool NormalTile;
        public byte Age;
        public byte SubType;
    }
}
