using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using WOEmu.Objects;
using WOEmu.Misc;

using System.IO;

namespace WOEmu.Terrain
{
    public static class MapSaver
    {
        public static void SaveMap()
        {
            for (int x = 0; x < Map.mapX; x++)
                for (int y = 0; y < Map.mapY; y++)
                {
                    Terrain terr = Map.mapTerrain[x][y];

                    FileStream tileMap = File.Open("maps/T" + x + "-" + y + ".BIN", FileMode.Open);
                    BinaryWriter tileWriter = new BinaryWriter(tileMap);
                    Bitmap bmp = new Bitmap(64, 64);

                    foreach (Tile t in terr.Tiles)
                    {
                        byte height = (byte)(t.Height + 20);
                        Color c = Color.FromArgb(height, height, height);
                        bmp.SetPixel(t.X, t.Y, c);

                        tileWriter.Write(System.Net.IPAddress.HostToNetworkOrder((short)(t.Type)));
                        tileWriter.Write((byte)(t.Age));
                    }

                    tileWriter.Close();
                    tileMap.Close();
                    bmp.Save("maps/H" + x + "-" + y + ".bmp");
                }
        }
    }
}
