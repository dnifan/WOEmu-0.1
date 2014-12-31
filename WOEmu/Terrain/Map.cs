using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace WOEmu.Terrain
{
    public static class Map
    {
        public static void initMap()
        {
            if (!File.Exists("maps/MAPS.TXT"))
            {
                throw new Exception("Unable to find MAPS.TXT file!");
            }

            FileStream fs = File.Open("maps/MAPS.TXT", FileMode.Open);
            StreamReader reader = new StreamReader(fs);

            int xSize = int.Parse(reader.ReadLine());
            int ySize = int.Parse(reader.ReadLine());

            WO.Core.Logger.Logger.printInfo("Map size: " + xSize + ", " + ySize);

            System.Console.Write("Loading map: ");
            mapTerrain = new Terrain[xSize][];

            for (int x = 0; x < xSize; x++)
            {
                mapTerrain[x] = new Terrain[ySize];
                for (int y = 0; y < ySize; y++)
                {
                    mapTerrain[x][y] = HeightMapLoader.Load("maps/H" + x + "-" + y + ".bmp", (short)(x * 64), (short)(y * 64));
                    TileMapLoader.Load(mapTerrain[x][y], (short)x, (short)y);
                    System.Console.Write(".");
                }
            }

            System.Console.WriteLine("DONE!");

            mapX = xSize;
            mapY = ySize;
        }

        public static void sendAll(Client c)
        {
            for(int x=0;x<mapX;x++)
                for (int y = 0; y < mapY; y++)
                {
                    Packets.TerrainPacket.SendTo(c, mapTerrain[x][y]);
                }
        }

        public static int mapX;
        public static int mapY;
        public static Terrain[][] mapTerrain;
    }
}
