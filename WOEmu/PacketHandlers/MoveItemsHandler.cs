using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.PacketHandlers
{
    public static class MoveItemsHandler
    {
        public static void Read(Client c, byte[] Packet)
        {
            PacketReader r = new PacketReader(Packet);

            r.PopByte(); //packet ID

            short amount = r.PopShort();
            WO.Core.Logger.Logger.printDebug("(TRADE) Amount = " + amount);
            for (int i = 0; i < amount; i++)
            {
                long l = r.PopLong();
                WO.Core.Logger.Logger.printDebug(i + ": " + l);
            }

            //This is 0 if it's on a trade window
            //Otherwise, it's the parent of the inventory window. (or -1 if it's not to be moved)
            long yeah = r.PopLong();
            WO.Core.Logger.Logger.printDebug("" + yeah);
        }
    }
}
