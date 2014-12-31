using System;
using System.Collections.Generic;
using System.Text;
using WO.Core;

namespace WOEmu.Misc
{
    /// <summary>
    /// Contains all possible MineTiletypes
    /// </summary>
    public enum MineTileType
    {
        Cave = 200,
        Cave2 = 201,
        CaveWall = 202,
        ReinforcedCaveWall = 203,

        GoldVein = 220,
        SilverVein = 221,
        IronVein = 222,
        CopperVein = 223,
        LeadVein = 224,
        ZincVein = 225,
        TinVein = 226
    }

    public class MineTile
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <param name="Z">Height</param>
        public MineTile(short x, short y, short Z,short roof)
        {
            X = x;
            Y = y;
            Height = Z;
            Roof = roof;
        }

        /// <summary>
        /// Set tiletype
        /// </summary>
        /// <param name="t">The minetiletype</param>
        public void SetType(MineTileType t)
        {
            Type = t;
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
        /// Return Mine tile flags
        /// </summary>
        /// <returns>int to push in bytebuffer</returns>
        /// unknown is there in the client, and is used for something,possibly height of mine roof?
        public int TileFlags()
        {
            return (Height << 24 ) + ((int)Type << 16) + (unknown * 10) ;
        }
        public short unknown = 1;
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
        public short Roof;
        public short Height;
        public MineTileType Type;
    }
}
