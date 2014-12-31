using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;
using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class Trade
    {
        public enum DestinationWindow
        {
            Offer = 1,
            Demand,
            Other_Offer,
            Other_Demand
        }

        public static void SendConfirm(Client c, bool status)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(51);

            w.PushByte((byte)(status == true ? 1 : 0));
        }

        public static void SendOpen(Client c, ActorBase a)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(30);

            w.PushByte((byte)a.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(a.Name));

            // Is the actor a player? Not sure why, but the real server does this check.

            if (a.isPlayer)
            {
                w.PushLong(1);
                w.PushLong(2);
                w.PushLong(3);
                w.PushLong(4);
            }

            else
            {
                w.PushLong(2);
                w.PushLong(1);
                w.PushLong(4);
                w.PushLong(3);
            }

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendClose(Client c)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(29);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void AddItem(Client c, DestinationWindow window, InventoryItem item)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(41);

            w.PushLong((long)window);
            w.PushLong(0);
            w.PushLong(item.ID); //This is this item's ID

            w.PushShort(0);
            w.PushByte((byte)item.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(item.Name));

            w.PushByte(0); //string (model)

            w.PushFloat(item.QL);
            w.PushFloat(item.DMG);
            w.PushInt(item.Weight);

            w.PushByte(0); //color

            w.PushByte(0);
            w.PushByte(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
