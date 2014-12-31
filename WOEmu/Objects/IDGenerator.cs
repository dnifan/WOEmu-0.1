using System;
using System.Collections.Generic;
using System.Text;
using WOEmu.Objects;
using WO.Core;

namespace WOEmu.Objects
{
    public static class IDGenerator
    {
        public static long GetID()
        {
            System.Console.WriteLine("----[IDGen] -> Return object ID {0}", Counter + 1);
            Counter++;
            return Counter;
        }

        internal static long Counter = 0;
    }
}
