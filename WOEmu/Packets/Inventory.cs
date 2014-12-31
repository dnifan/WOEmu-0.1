using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    public class InventoryItem
    {
        public InventoryItem(string n, long id, long parentID)
        {
            Name = n;
            ID = id;
            parent = parentID;

            QL = 0.0f;
            DMG = 0.0f;
            Weight = 0;
        }

        public float QL;
        public float DMG;
        public int Weight;

        public string Name;
        public long ID;
        public long parent;
        public long templateID;
    }

    public static class Inventory
    {
        public static void sendInitialInventory(Client c)
        {
            InventoryItem bodyInv = new InventoryItem("Body", 5000, 0);
            InventoryItem headInv = new InventoryItem("Head", 5001, 5000);
            InventoryItem faceInv = new InventoryItem("Face", 5002, 5001);
            InventoryItem torsoInv = new InventoryItem("Torso", 5003, 5000);
            InventoryItem rightArmInv = new InventoryItem("Right Arm", 5004, 5000);
            InventoryItem rightHandInv = new InventoryItem("Right Hand", 5005, 5004);
            InventoryItem leftArmInv = new InventoryItem("Left Arm", 5006, 5000);
            InventoryItem leftHandInv = new InventoryItem("Left Hand", 5007, 5006);
            InventoryItem legsInv = new InventoryItem("Legs", 5008, 5000);
            InventoryItem rightFootInv = new InventoryItem("Right Foot", 5009, 5008);
            InventoryItem leftFootInv = new InventoryItem("Left Foot", 5010, 5008);

            InventoryItem invInv = new InventoryItem("Inventory", 5100, 0);

            Console.WriteLine("Inventory ID - " + invInv.ID);

            Add(c, bodyInv);
            Add(c, headInv);
            Add(c, faceInv);
            Add(c, torsoInv);
            Add(c, rightArmInv);
            Add(c, rightHandInv);
            Add(c, leftArmInv);
            Add(c, leftHandInv);
            Add(c, legsInv);
            Add(c, rightFootInv);
            Add(c, leftFootInv);
            Add(c, invInv);
        }

        private static InventoryItem constructInvItem(long keyID)
        {
            Sql s = new Sql(Program.sqlData);

            s.ExecuteQuery("SELECT * FROM item_templates WHERE ID = '" + keyID + "';");
            InventoryItem ret = null;

            while (s.reader.Read())
            {
                ret = new InventoryItem(s.reader.GetString("Name"), Objects.IDGenerator.GetID(), 5100);
            }

            return ret;
        }

        public static InventoryItem getInventoryItem(Client c, long ID)
        {
            foreach (InventoryItem i in c.player.inventory)
            {
                if (i.ID == ID)
                {
                    System.Console.WriteLine("Found: " + i.Name);
                    return i;
                }
            }
            return null;
        }

        public static void loadFromDB(Client c)
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT * FROM inventory WHERE accountID = '" + c.accountID + "';");

            while (s.reader.Read())
            {
                InventoryItem item = constructInvItem(s.reader.GetInt64("itemTemplate"));

                item.ID = s.reader.GetInt64("keyID");
                item.parent = s.reader.GetInt64("parentID");

                WO.Core.Logger.Logger.printDebug("Item(" + item.ID + "), " + item.Name);
                c.player.inventory.Add(item);

                Add(c, item);
            }
        }

        public static void Remove(Client c, InventoryItem i)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(84);

            w.PushLong(i.parent);
            w.PushLong(i.ID);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void Update(Client c, InventoryItem i, long Window)
        {
            // This method always worked correctly. Stupid me.
            PacketWriter w = new PacketWriter();
            w.PushShort(0);
            w.PushByte(36);

            // Start the actual packet
            w.PushLong(-1); //-1 means, this is inventory.
            w.PushLong(i.ID);
            w.PushLong(i.parent); // The ID of the parent window, -1 = inventory

            string Desc = "test";

            w.PushByte((byte)Desc.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(Desc)); // This seems to be the description of the item.

            w.PushByte((byte)Desc.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(Desc)); // This seems to be the description of the item.

            w.PushFloat(50); // This seems to be the quality
            w.PushFloat(50); // This seems to be the damage

            w.PushInt(2000); // This is the weight of the item.

            w.PushByte(0); // Is the item painted?

            /*
            w.PushByte(0); // Colour (red)
            w.PushByte(0); // Color (green)
            w.PushByte(0); // Color (blue)
            */

            w.PushByte(0); // This one is another boolean, not entirely sure what it does.

            /*
            w.PushInt(100);
            */

            w.PushByte(0); // Yet another boolean. Not sure what it does.

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);

        }

        public static void Add(Client c, InventoryItem i)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(41);

            w.PushLong(-1);
            
            w.PushLong(i.parent);
            w.PushLong(i.ID); //This is this item's ID
            Console.WriteLine(i.Name + ", ID - " + i.ID + ", Parent=" + i.parent);

            w.PushShort(0);
            w.PushByte((byte)i.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(i.Name));

            w.PushByte(0); //this string is not included, but a string can be put in here?!
            //therefore, the string len is 0
            //this is probably the icon filename.

            w.PushFloat(i.QL);
            w.PushFloat(i.DMG);
            w.PushInt(i.Weight);

            w.PushByte(0); //color
            w.PushByte(0);
            w.PushByte(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
