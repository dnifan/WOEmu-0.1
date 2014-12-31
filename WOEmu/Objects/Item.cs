using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Misc;
using System.Text;
using WOEmu.Terrain;

namespace WOEmu.Objects
{
    public class Item : ObjectBase
    {
        public Item(string name)
        {
            Name = name;
            ID = IDGenerator.GetID();
            Type = ObjectType.Item;
            wiki = wiki;
            template = null;
        }

        public void SetModel(string m)
        {
            Model = m;
        }

        public void SetPos(Vector3 pos)
        {
            Position = pos;
        }

        public string Model;
        public int templateID;

        public ItemTemplate template;
    }
}
