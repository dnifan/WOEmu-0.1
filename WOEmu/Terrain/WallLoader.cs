using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

using WOEmu.Objects;

namespace WOEmu.Terrain
{
    public static class WallLoader
    {
        public static int LoadFromDB()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT * FROM walls");

            int ctr = 0;
            while (s.reader.Read())
            {
                short X = (short)s.reader.GetInt32("PosX");
                short Y = (short)s.reader.GetInt32("PosY");
                byte type = (byte)s.reader.GetInt16("Type");
                bool flipped = (bool)(s.reader.GetInt16("Flipped") == 1 ? true : false);

                Wall w = new Wall(X, Y, type, flipped);
                w.Type = ObjectType.Wall;
                ObjectPool.AddObject(w);

                ctr++;
            }

            return ctr;
        }
    }
}
