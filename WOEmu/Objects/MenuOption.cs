using System;
using System.Collections.Generic;
using System.Text;

namespace WOEmu.Objects
{
    /// <summary>
    /// Represents a option in a menu
    /// </summary>
    public class MenuOption
    {
        public MenuOption(string n, long i)
        {
            Name = n;
            ID = i;
        }

        public string Name;
        public long ID;
    }
}
