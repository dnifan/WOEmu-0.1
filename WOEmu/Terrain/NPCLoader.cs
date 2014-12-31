using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.Terrain
{
    public static class NPCLoader
    {
        public static int LoadFromDB()
        {
            Sql s = new Sql(Program.sqlData);

            s.ExecuteQuery("SELECT * FROM npcs");

            int c = 0;

            while (s.reader.Read())
            {
                c++;
                long ID = s.reader.GetInt64("ID");

                float xpos = s.reader.GetFloat("posX");
                float ypos = s.reader.GetFloat("posY");
                float zpos = s.reader.GetFloat("posZ");
                float rot = s.reader.GetFloat("rotation");
                string wiki = s.reader.GetString("wiki");
                string model = s.reader.GetString("model");
                string name = s.reader.GetString("name");
                float health = s.reader.GetFloat("Health");
                float maxhealth = s.reader.GetFloat("MaxHealth");
                string examine = "You see " + name;

                examine = "You see " + examine;
                ActorBase act = new ActorBase();

                act.Type = ObjectType.NPC;
                act.Position = new Vector3(xpos, ypos, zpos);
                act.Rotation = rot;
                act.Name = name;
                act.Model = model;
                act.wiki = wiki;
                act.examine = examine;
                act.Health = health;
                act.MaxHealth = maxhealth;
                ObjectPool.AddObject(act);
                
            }

            return c;
        }
    }
}
