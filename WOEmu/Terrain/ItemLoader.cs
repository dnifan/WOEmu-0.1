using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Misc;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.Terrain
{
    public static class ItemLoader
    {
        public static int LoadStatic()
        {
            Sql s = new Sql(Program.sqlData);

            s.ExecuteQuery("SELECT * FROM static_items");

            int ctr = 0;

            while (s.reader.Read())
            {
                ctr++;
                int index = s.reader.GetInt32("ID");
                long staticID = s.reader.GetInt64("staticID");

                if (s.reader.GetBoolean("custom_item") == false)
                {
                    long itemID = s.reader.GetInt64("item_template");
                }

                Item i = new Item("staticitem_" + ctr);
                i.Type = ObjectType.StaticItem;
                i.wiki = s.reader.GetString("wiki");
                i.examine = s.reader.GetString("examine");
                i.templateID = 0;

                ObjectPool.AddObject(i);
            }

            return ctr;
        }

        public static int LoadFromDB()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT * FROM item_templates");
            templates = new List<WOEmu.Misc.ItemTemplate>();

            while (s.reader.Read())
            {
                ItemTemplate t = new ItemTemplate();

                t.ItemID = (int)s.reader.GetInt64("ID");
                t.Name = s.reader.GetString("Name");
                t.Wiki = s.reader.GetString("Wiki");
                t.Examine = s.reader.GetString("Examine"); //Loads Examine string
                t.Model = s.reader.GetString("Model");

                templates.Add(t);
            }

            s.ExecuteQuery("SELECT * FROM items");

            int c = 0;

            while (s.reader.Read())
            {
                long template_id = s.reader.GetInt64("itemID");
                float xpos = s.reader.GetFloat("posX");
                float ypos = s.reader.GetFloat("posY");
                float zpos = s.reader.GetFloat("posZ");
                bool onfire = s.reader.GetBoolean("onFire");
                bool emitLight = s.reader.GetBoolean("emitLight");
                byte radius = 0;

                if (onfire)
                {
                    radius = (byte)s.reader.GetInt16("fireRadius");
                }

                ItemTemplate ourTemplate = getTemplate((int)template_id);

                if (ourTemplate == null)
                    continue;

                Item i = new Item(ourTemplate.Name);
                i.SetModel(ourTemplate.Model);
                i.wiki = ourTemplate.Wiki;
                i.examine = ourTemplate.Examine;
                i.templateID = (int)template_id;
                i.emitLight = emitLight;
                i.template = ourTemplate;

                i.OnFire = onfire;
                if (onfire)
                {
                    i.fireRadius = radius;
                }

                i.Position = new Vector3(xpos, ypos, zpos);

                ObjectPool.AddObject(i);
                c++;
            }

            return c;
        }

        public static ItemTemplate getTemplate(int ID)
        {
            foreach (ItemTemplate t in templates)
            {
                if (t.ItemID == ID)
                    return t;
            }

            return null;
        }

        public static List<ItemTemplate> templates;
    }
}
