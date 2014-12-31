using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WO.Core;

namespace WOEmu.Objects
{
    public static class MenuPool
    {
        public static void Init()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT MAX(ID) FROM menuoptions");
            s.reader.Read();
            long amount = s.reader.GetInt64(0);
            WO.Core.Logger.Logger.printInfo(amount + " menu options.");

            Pool = new List<MenuOption>((int)amount);

            s.ExecuteQuery("SELECT * FROM menuoptions");
            while (s.reader.Read())
            {
                MenuOption o = new MenuOption(s.reader.GetString("Name"), s.reader.GetInt64("ID"));
                Add(o);
            }

            s.Dispose();
        }

        public static void Add(MenuOption m)
        {
            WO.Core.Logger.Logger.printDebug(m.Name);
            Pool.Add(m);
        }

        public static MenuOption Get(long ID)
        {
            foreach (MenuOption m in Pool)
            {
                if (m.ID == ID)
                    return m;
            }
            return null;
        }

        public static List<MenuOption> Pool;
    }
}
