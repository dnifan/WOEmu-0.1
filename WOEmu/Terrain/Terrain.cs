using System;
using System.Collections.Generic;
using System.Text;
using WOEmu.Misc;

namespace WOEmu.Terrain
{
    public class Terrain
    {
        public Terrain(short w, short h, short xOffset, short yOffset)
        {
            Width = w;
            Height = h;

            cornerX = xOffset;
            cornerY = yOffset;

            Tiles = new List<Tile>(w * h);

            //Initialize terrain for first use.
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    Tile t = new Tile((short)x, (short)y, 2); //give safe initial height :)
                    t.SetType(t.Type);
                    t.SetAge(t.Age);
                    Tiles.Add(t);
                }
        }

        public short GetZ(short X, short Y)
        {
            Tile t = Tiles[GetTileID(X, Y)];
            return t.Height;
        }

        public int GetTileID(short x, short y)
        {
            int c = 0;
            foreach (Tile t in Tiles)
            {
                if (t.X == x && t.Y == y)
                    return c;
                c++;
            }
            return -1;
        }


        public short Width;
        public short Height;
        public short cornerX;
        public short cornerY;

        public List<Tile> Tiles;
    }
}
