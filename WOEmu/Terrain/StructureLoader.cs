using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.Terrain
{
    public static class StructureLoader
    {
        private static int getAmountOfNodes(long ID)
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT * FROM structure_nodes WHERE structureID = " + ID + ";");

            int ctr = 0;

            while (s.reader.Read())
            {
                ctr++;
            }

            s.Dispose();
            return ctr;
        }

        private static List<StructureNode> getNodes(long ID, Structure parent)
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT * FROM structure_nodes WHERE structureID = " + ID + ";");

            List<StructureNode> ret = new List<StructureNode>();

            while (s.reader.Read())
            {
                StructureNode buf = new StructureNode(parent);

                StructureType t = (StructureType)s.reader.GetInt32("Type");
                string m = s.reader.GetString("Material");

                int o1 = (int)s.reader.GetInt64("rOffsetX");
                int o2 = (int)s.reader.GetInt64("rOffsetY");
                int o3 = (int)s.reader.GetInt64("rOffsetZ");

                bool f = s.reader.GetBoolean("Flipped");

                bool coloured = s.reader.GetBoolean("Coloured");
                byte r = (byte)s.reader.GetInt32("R");
                byte g = (byte)s.reader.GetInt32("G");
                byte b = (byte)s.reader.GetInt32("B");

                buf.SetOptions(t, new Vector3(o1, o2, o3), m, f, coloured);
                if (coloured)
                {
                    
                }

                ret.Add(buf);
            }

            return ret;
        }
        
        /// <summary>
        /// Load all structures from the database
        /// </summary>
        /// <returns>Amount of structures loaded</returns>
        public static int LoadFromDB()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT * FROM structures");
            int c = 0;

            while (s.reader.Read())
            {
                long ID = (long)s.reader.GetUInt64("gID");
                string Name = s.reader.GetString("Name");

                long X = s.reader.GetInt64("originX");
                long Y = s.reader.GetInt64("originY");

                Structure b = new Structure(ID, Name);
                ObjectPool.AddObject(b);

                List<StructureNode> nodes = getNodes(b.ID, b);
                foreach (StructureNode n in nodes)
                {
                    b.SetOrigin(new Vector3(X, Y, 0));
                    b.AddNode(n);
                }

                c++;
            }

            s.Dispose();

            return c;
        }
    }
}
