using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

using WOEmu.Misc;
using WOEmu.Objects;

namespace WOEmu.Menus
{
    public static class TileMenus
    {
        private static void Add(List<__menuItem> l, short num)
        {
            l.Add(new __menuItem(num, MenuProxy.mItems[num].name, MenuProxy.mItems[num].parented, MenuProxy.mItems[num].submenu));
        }

        /// <summary>
        /// Returns a list of menu items for GroundItems
        /// </summary>
        /// <param name="i">The item which is targetted</param>
        /// <returns>List of menus</returns>
        public static List<__menuItem> getGroundItemMenu(Item i)
        {
            List<__menuItem> ret = new List<__menuItem>();

            Add(ret, 2); //Examine

            return ret;
        }
        
        /// <summary>
        /// Returns a list of menu items for Tile borders
        /// </summary>
        /// <param name="i">The item that was activated</param>
        /// <returns>List of menu items</returns>
        public static List<__menuItem> getTileBorderMenu(Item i)
        {
            List<__menuItem> ret = new List<__menuItem>();

            Add(ret, 2); //Examine
            Add(ret, 11); //Make Fence
            Add(ret, 12); //Make Gate

            return ret;
        }

        public static List<__menuItem> getWallMenu(Wall w, Item i)
        {
            List<__menuItem> ret = new List<__menuItem>();

            Add(ret, 2); //Examine
            if (w.WallType == WallTypes.Palisade_Gate || w.WallType == WallTypes.Wooden_Fence_Gate)
            {
                if (w.Open == true)
                    Add(ret, 18); //Close
                else
                    Add(ret, 17); //Open
            }

            return ret;
        }

        public static List<__menuItem> getTileMenu(Tile t, Item i)
        {
            List<__menuItem> ret = new List<__menuItem>();

            //Examine
            Add(ret, 2);

            if (t.Height <= 0)
                Add(ret, 4); //Drink

            Add(ret, 10); //CreateStructure

            Add(ret, 16); //Plant Tree

            switch (t.Type)
            {
                case TileType.Grass:
                    Add(ret, 8); //Pack
                    break;

                case TileType.Dirt:
                    Add(ret, 8); //Pack
                    break;

                case TileType.PackedDirt:
                    //Pave
                    if (i != null) //Check whether a item was activated
                    {
                        if ((i.template.ItemID == 5) || (i.template.ItemID == 6))   //Rock shards OR stone bricks
                            Add(ret, 9);
                    } //Pave

                    break;
            }

            return ret;
        }
    }
}
