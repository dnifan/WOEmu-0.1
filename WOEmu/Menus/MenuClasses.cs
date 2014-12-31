using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

using WOEmu.Objects;
using WOEmu.Misc;

namespace WOEmu.Menus
{
    public class handler_Open
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            Wall w = TileCalculator.getWall(ID);
            foreach (Client c in Program.clients)
            {
                Packets.Walls.SendOpen(c, w, true);
            }
            return true;
        }
    }

    public class handler_Close
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            Wall w = TileCalculator.getWall(ID);
            foreach (Client c in Program.clients)
            {
                Packets.Walls.SendOpen(c, w, false);
            }
            return true;
        }
    }

    public class handler_Plant
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            Tile t = TileCalculator.getTile(ID);
            t.Type = TileType.Tree;
            t.SubType = (byte)TreeType.Apple;
            t.Age = 10;
            foreach (Client c in Program.clients)
                Packets.TerrainPacket.UpdateTile(c, t);
            return true;
        }
    }

    /// <summary>
    /// Handler to remove items from the world
    /// </summary>
    public class handler_Remove
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            return true;
        }
    }

    public class handler_MakeGate
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            Vector3 pos = TileCalculator.getTileBorderAbsPos(ID);
            bool isFlipped = TileCalculator.getTileBorderIsFlipped(ID);

            Wall w = new Wall((short)pos.X, (short)pos.Y, WallTypes.Wooden_Fence_Gate, isFlipped);
            foreach (Client c in Program.clients)
            {
                Packets.Walls.Send(c, w);
                Packets.Walls.SendOpen(c, w, true);
            }

            w.saveToDatabase();
            return true;
        }
    }

    public class handler_MakeFence
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            Vector3 pos = TileCalculator.getTileBorderAbsPos(ID);
            WO.Core.Logger.Logger.printDebug(pos.ToString());
            bool isFlipped = TileCalculator.getTileBorderIsFlipped(ID);
            if (isFlipped)
                WO.Core.Logger.Logger.printDebug("Flipped!");
            Wall w = new Wall((short)pos.X, (short)pos.Y, WallTypes.Stone_Wall, isFlipped);

            foreach (Client c in Program.clients)
            {
                Packets.Walls.Send(c, w);
            }
            w.saveToDatabase();

            return true;
        }
    }

    public class handler_CreateStructure
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            Tile t = TileCalculator.getTile(ID);
            Sql query = new Sql(Program.sqlData);

            int numRows = 0;

            query.ExecuteQuery("SELECT * FROM structures");

            while (query.reader.Read())
            {
                numRows++;
            }

            Structure s = new Structure(numRows +1, e.player.Name + "'s house");
            s.SetOrigin(new Vector3(t.X, t.Y, t.Height));

            StructureNode n1 = new StructureNode(s);
            n1.type = StructureType.Wall;
            n1.SetOptions(StructureType.Wall, new Vector3(1, 0, 0), "wood", true, false);
            //Packets.StructurePacket.SendStructureNode(e, n1);

            StructureNode n2 = new StructureNode(s);
            n2.type = StructureType.Door;
            n2.SetOptions(StructureType.Door, new Vector3(0, 0, 0), "wood", false, false);

            StructureNode n3 = new StructureNode(s);
            n3.type = StructureType.Window;
            n3.SetOptions(StructureType.Window, new Vector3(0, 1, 0), "wood", false, false);

            StructureNode n4 = new StructureNode(s);
            n4.type = StructureType.Wall;
            n4.SetOptions(StructureType.Wall, new Vector3(0, 0, 0), "wood", true, false);

            s.AddNode(n1);
            s.AddNode(n2);
            s.AddNode(n3);
            s.AddNode(n4);

            //Lol - lots of packets ^^
            foreach (Client c in Program.clients)
            {
                Packets.StructurePacket.SendAddStructure_Bare(c, s);
                Packets.StructurePacket.MarkTile(c, s);

                Packets.StructurePacket.SendStructureNode(c, n1);
                Packets.StructurePacket.SendStructureNode(c, n2);
                Packets.StructurePacket.openDoor(c, n2);
                Packets.StructurePacket.ClipNode(c, n2, true);
                Packets.StructurePacket.SendStructureNode(c, n3);
                Packets.StructurePacket.SendStructureNode(c, n4);
            }

            s.saveToDatabase(e.player);

            return true;
        }
    }

    public class handler_Pave
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {


            return true;
        }
    }

    public class handler_Pack
    {
        public bool onClick(string full, Client e, long ID, Item i)
        {
            //We can be sure this is a tile, since this option
            //is only used on tiles.
            Tile t = TileCalculator.getTile(ID);

            t.Type = TileType.PackedDirt;
            foreach (Client c in Program.clients)
            {
                Packets.TerrainPacket.UpdateTile(c, t);
            }

            return true;
        }        
    }

    /// <summary>
    /// This is the handler for the 'Examine' button.
    /// </summary>
    public class handler_Examine
    {
        public bool onClick(string fullOption, Client executor, long targetID, Item i)
        {
            ObjectBase b = ObjectPool.GetObject(targetID);

            if (b == null)
            {
                Tile t = TileCalculator.getTile(targetID);

                switch (t.Type)
                {
                    case TileType.Grass:
                        executor.SendChat("You see a grass tile (" + new Vector3(t.X, t.Y, t.Height).ToString() + ")", ":Event");
                        executor.SendChat("Also: tile pos: " + TileCalculator.getTileAbsPos(t.X, t.Y), ":Event");
                        break;

                    default:
                        executor.SendChat("You see a tile.", ":Event");
                        break;
                }
            }
            else
            {
                executor.SendChat(b.examine, ":Event");
            }

            return true;
        }
    }

    public class handler_Dig
    {
        public bool onClick(string fulloption, Client ex, long targetID, Item i)
        {
            byte digMod = 1;
            Tile t = TileCalculator.getTile(targetID);
            Vector3 tileCoords = TileCalculator.getTileAbsPos(targetID);

            t.Height -= digMod;

            foreach (Client c in Program.clients)
            {
                Packets.TerrainPacket.UpdateTile(c, t);
            }

            return true;
        }
    }

    public class handler_Plan
    {
        public bool onClick(string fullopt, Client ex, long targetID, Item i)
        {
            return true;
        }
    }
}
