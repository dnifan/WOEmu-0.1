using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Text;

using WO.Core;

using WOEmu.Objects;
using WOEmu.Misc;

namespace WOEmu.Menus
{
    public class __menuItem
    {
        public __menuItem(short i, string n)
        {
            ID = i; name = n;
            submenu = false;
            parented = false;
        }

        public __menuItem(short i, string n, bool parented, bool subMenu)
        {
            ID = i;
            name = n;
            submenu = subMenu;
            this.parented = parented;
        }

        public short ID;
        public string name;
        public bool submenu;
        public bool parented;
    }

    public static class MenuProxy
    {
        public static __menuItem[] mItems;
        public static int itemPointer = 1; //1 because C# offset is 0, but SQL offset is 1. So this is to 
                                           //prevent confusion

        public static void addItem(__menuItem i)
        {
            mItems[itemPointer] = i;
            itemPointer++;
        }

        /// <summary>
        /// Loads all menu options from database.
        /// </summary>
        public static void Init()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT MAX(ID) FROM menuoptions");
            s.reader.Read();
            long amount = s.reader.GetInt64(0);
            WO.Core.Logger.Logger.printInfo(amount + " menu options.");

            mItems = new __menuItem[amount+1];

            s.ExecuteQuery("SELECT * FROM menuoptions");
            while (s.reader.Read())
            {
                addItem(new __menuItem(s.reader.GetInt16("ID"), s.reader.GetString("Name"), (s.reader.GetInt16("parented") == 1 ? true : false), (s.reader.GetInt16("subMenu") == 1 ? true : false)));
            }

            s.Dispose();
        }

        public static void doubleClick(Client clicker, long target)
        {

        }

        /// <summary>
        /// Gets called when a player clicks a item
        /// in the menu
        /// </summary>
        /// <param name="menuID">ID of the button pressed</param>
        /// <param name="caller">Client who pressed the button</param>
        /// <param name="targetID">ID of the target object</param>
        /// <param name="activeItem">ID of the item selected by the client</param>
        /// <returns>Success</returns>
        public static bool Call(short menuID, Client caller, long targetID, long activeItem)
        {
            string classString = mItems[menuID].name.Split(' ')[0];
            WO.Core.Logger.Logger.printDebug("Creating class: 'handler_" + classString + "'.");
            object retArg = null;

            try
            {
                Type callClass = Type.GetType("WOEmu.Menus.handler_" + classString, false);
                object obj = null;
                if (callClass != null)
                    obj = Activator.CreateInstance(callClass);

                Item active = (Item)ObjectPool.GetObject(activeItem);

                if (callClass == null || obj == null)
                {
                    ObjectBase b = ObjectPool.GetObject(targetID);
                    if (b == null)
                    {
                        //Tile:
                        Scripting.ScriptingInterface.runGameHook("OnTileMenuClick", new object[] { caller, TileCalculator.getTile(targetID), menuID });
                        return true;
                    }
                    else
                    {
                        Scripting.ScriptingInterface.runObjectHook(b, "OnClick", new object[] { caller, b, menuID });
                    }
                }
                else
                {
                    object[] args = new object[] { mItems[menuID].name, caller, targetID, active };
                    retArg = callClass.InvokeMember("onClick", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, args);
                }
            }
            catch (Exception e)
            {
                //this is sandbox thing, if ANYTHING happens within the callback Class and it throws an
                //exception, it gets caught here, and we will return false. (So the client willl see it failed)
                WO.Core.Logger.Logger.printDebug(e.ToString());
                return false;
            }

            return (bool)retArg;
        }

        public static void sendMenu(long oID, Client c, byte req, long activeItem)
        {
            List<__menuItem> localItems = new List<__menuItem>();
            localItems.Add(new __menuItem(2, mItems[2].name)); //"Examine", this is a standard thingy

            ObjectBase objBase = ObjectPool.GetObject(oID);
            if (objBase == null) //This should all be handled by scripts later.
            {
                //Check whether this is a multiple of 65536
                if (Math.IEEERemainder(oID, 65536) == 12)
                {
                    WO.Core.Logger.Logger.printDebug("Is a tile border!");
                    List<__menuItem> bList = TileMenus.getTileBorderMenu((Item)ObjectPool.GetObject(activeItem));
                    Packets.PopupMenu.SendTo(c, bList, "tile border", req);
                    return;
                }
                else if (TileCalculator.isWall(oID))
                {
                    //A fence/wall is here
                    List<__menuItem> wList = TileMenus.getWallMenu(TileCalculator.getWall(oID), (Item)ObjectPool.GetObject(activeItem));
                    Packets.PopupMenu.SendTo(c, wList, "Tile Border", req);
                }
                else
                {
                    //This is a tile.
                    WO.Core.Logger.Logger.printDebug("ID = " + oID);
                    Tile t = Misc.TileCalculator.getTile(oID);
                    WO.Core.Logger.Logger.printDebug("Tile: " + t.X + ", " + t.Y);

                    Item active = (Item)ObjectPool.GetObject(activeItem);
                    List<__menuItem> list = TileMenus.getTileMenu(t, active);

                    Packets.PopupMenu.SendTo(c, list, "Grass", req);
                }
            }
            else
            {
                if (objBase.Type == ObjectType.Item)
                {
                    Item item = (Item)objBase;
                    List<__menuItem> items = TileMenus.getGroundItemMenu(item);
                    Packets.PopupMenu.SendTo(c, items, item.wiki, req);
                }
                else if (objBase.Type == ObjectType.NPC)
                {
                    ActorBase npc = (ActorBase)objBase;
                }
                else if (objBase.Type == ObjectType.Wall)
                {
                    Wall w = (Wall)objBase;
                }
                else
                {
                    List<Object> l = Scripting.ScriptingInterface.runGameHook("OnRequestMenu", new object[] { c, objBase });
                    foreach (Object o in l)
                    {
                        int ID = (int)o;
                        localItems.Add(new __menuItem((short)ID, mItems[ID].name));
                    }

                    Packets.PopupMenu.SendTo(c, localItems, "Grass", req);
                }
            }
        }
    }
}
