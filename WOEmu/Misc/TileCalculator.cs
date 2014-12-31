using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

using WOEmu.Objects;
using WOEmu.Terrain;

namespace WOEmu.Misc
{
    /// <summary>
    /// Calculates Tile from ID
    /// </summary>
    public static class TileCalculator
    {
        public static Wall getWall(long ID)
        {
            //TODO: Make this more efficient.
            Vector3 pos = getTileBorderAbsPos(ID);
            foreach (ObjectBase b in ObjectPool.Pool)
            {
                if (b.Type == ObjectType.Wall)
                {
                    if (b.Position.X == pos.X && b.Position.Y == pos.Y)
                        return (Wall)b;
                }
            }
            return null;
        }

        public static bool isWall(long ID)
        {
            Vector3 pos = getTileBorderAbsPos(ID);
            foreach (ObjectBase b in ObjectPool.Pool)
            {
                if (b.Type == ObjectType.Wall)
                {
                    if (b.Position.X == pos.X && b.Position.Y == pos.Y)
                        return true;
                }
            }
            return false;
        }

        public static Vector3 getTileAbsPos(long ID)
        {
            long itemID = ID;

            short Va, Vb, Vc;
            itemID -= 7;
            Vc = (short)(itemID >> 48);
            Va = (short)(itemID >> 32);
            Vb = (short)(itemID >> 16);

            short coordX = (short)((Va / 256));
            short coordY = (short)(Vb);

            return new Vector3(coordX, coordY, 0.0f);
        }

        public static Vector3 getTileBorderAbsPos(long ID)
        {
            long itemID = ID;

            short Va, Vb, Vc;
            itemID -= 7;
            Vc = (short)(itemID >> 48);
            Va = (short)(itemID >> 32);
            Vb = (short)(itemID >> 16);

            WO.Core.Logger.Logger.printDebug("" + Vc + ", " + Va + ", " + Vb);

            short coordX = Va;
            short coordY = Vb;

            return new Vector3(coordX, coordY, 0.0f);
        }

        public static bool getTileBorderIsFlipped(long ID)
        {
            long itemID = ID;

            short Va, Vb, Vc;
            itemID -= 7;
            Vc = (short)(itemID >> 48);
            Va = (short)(itemID >> 32);
            Vb = (short)(itemID >> 16);

            WO.Core.Logger.Logger.printDebug("" + Vc + ", " + Va + ", " + Vb);

            return (Vc == 2);
        }

        public static Vector3 getTileAbsPos(float X, float Y)
        {
            float posX = X / 64;
            float posY = Y / 64;

            return new Vector3(posX, posY, 0.0f);
        }

        public static Tile getTile(long ID)
        {
            long itemID = ID;

            short Va, Vb, Vc;
            itemID -= 7;
            Vc = (short)(itemID >> 48);
            Va = (short)(itemID >> 32);
            Vb = (short)(itemID >> 16);

            short coordX = (short)((Va / 256));
            short coordY = (short)(Vb);

            float coordXdiv = (float)Math.Floor((float)(coordX / 64));
            float coordYdiv = (float)Math.Floor((float)(coordY / 64));

            WO.Core.Logger.Logger.printDebug("At map chunk [" + coordXdiv + ", " + coordYdiv + "]");

            short normalizedX = (short)PacketHandlers.Movement.Delta(coordX, (coordXdiv * 64));
            short normalizedY = (short)PacketHandlers.Movement.Delta(coordY, (coordYdiv * 64));

            int tileID = Terrain.Map.mapTerrain[(int)coordXdiv][(int)coordYdiv].GetTileID(normalizedX, (short)(normalizedY+1));
            Tile tile = Terrain.Map.mapTerrain[(int)coordXdiv][(int)coordYdiv].Tiles[tileID];

            return tile;
        }
    }
}
