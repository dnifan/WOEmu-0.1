using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;

namespace WOEmu.Terrain
{
    public static class TileMapLoader
    {
        public static void Load(Terrain t, short xTile, short yTile)
        {
            if (!File.Exists("maps/T" + xTile + "-" + yTile + ".BIN"))
            {
                throw new FileNotFoundException("Tilemap not found! (" + xTile + ", " + yTile + ")");
            }

            FileStream fs = File.Open("maps/T" + xTile + "-" + yTile + ".BIN", FileMode.Open);
            BinaryReader reader = new BinaryReader(fs);

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    short tileType = IPAddress.HostToNetworkOrder(reader.ReadInt16());
                    byte tileAge = reader.ReadByte();

                    int tileID = t.GetTileID((short)x, (short)y);

                    t.Tiles[tileID].Type = (WOEmu.Misc.TileType)tileType;
                    t.Tiles[tileID].Age = tileAge;
                }
            }
        }
    }
}
