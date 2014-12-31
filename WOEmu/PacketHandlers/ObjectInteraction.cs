using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Misc;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.PacketHandlers
{
    public static class ObjectInteraction
    {
        public static void Read(Client c, byte[] pkt)
        {
            PacketReader r = new PacketReader(pkt);
            long ID = 0;

            r.PopByte();

            long activeItem = r.PopLong(); //is -1 when no item is activated.
            short amount = r.PopShort();

            for (int i = 0; i < amount; i++)
                ID = r.PopLong();

            short selectionID = r.PopShort();

            if (selectionID == -1)
                Menus.MenuProxy.doubleClick(c, ID); //double click
            else
            {
                if (Menus.MenuProxy.Call(selectionID, c, ID, activeItem) == false)
                {
                    c.SendChat("Action failed!", ":Event");
                }
            }
        }
    }
}
