using System;
using System.Collections.Generic;
using System.Text;
using WO.Core;

using WOEmu.Misc;
using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class GUI
    {
        public static void SendContainerContent(Client c, InventoryItem i, long windowID)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(41);

            w.PushLong(windowID);
            w.PushLong(i.parent);
            w.PushLong(i.ID);

            w.PushShort(0); //?
            w.PushByte((byte)i.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(i.Name));

            w.PushByte(0);
            //String here

            w.PushFloat(i.QL);
            w.PushFloat(i.DMG);
            w.PushInt(i.Weight);

            w.PushByte(0);
            w.PushByte(0);
            w.PushByte(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static long SendOpenContainer(Client c, string title)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(28);

            long ID = Objects.IDGenerator.GetID();
            w.PushLong(ID);
            w.PushByte((byte)title.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(title));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);

            return ID;
        }

        public static void SendCloseContainer(Client c, long ID)
        {
            PacketWriter w = new PacketWriter();
            
            w.PushShort(0);
            w.PushByte(31);

            w.PushLong(ID);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendBML(Client c, BMLObject o)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(71);

            w.PushByte((byte)o.Caption.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(o.Caption));

            w.PushShort(o.X);
            w.PushShort(o.Y);

            w.PushByte((byte)(o.Closable == true ? 1 : 0)); //closable
            w.PushByte((byte)(o.Resizable == true ? 1 : 0)); //movable

            w.PushByte(255);
            w.PushByte(255);
            w.PushByte(255);

            w.PushShort((short)o.Body.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(o.Body));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
