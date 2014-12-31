using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Objects;
using System.Text;
using WOEmu.Packets;
using WOEmu.Terrain;
using WOEmu.Misc;


namespace WOEmu.PacketHandlers
{
    /// <summary>
    /// This handles the request for a menu (right-click)
    /// </summary>
    public static class ObjectInfo
    {
        public static void Read(Client c, byte[] Packet)
        {
            PacketReader r = new PacketReader(Packet);

            r.PopByte(); //packet ID

            byte request_byte = r.PopByte();
            long activatedItem = r.PopLong();

            long itemID = r.PopLong();
            ObjectBase b = ObjectPool.GetObject(itemID);

            Menus.MenuProxy.sendMenu(itemID, c, request_byte, activatedItem);
        }
    }
}
