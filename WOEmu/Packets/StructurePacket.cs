using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Terrain;
using WOEmu.Misc;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// Major feature - Structures
    /// N.B. I call them structures myself, as they have no official name.
    /// </summary>
    public static class StructurePacket
    {
        public static void RemoveStructure(Client c, Structure s)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(88);

            w.PushLong(s.ID);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// This is AFTER the structure got a new name!
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        public static void RenameStructure(Client c, Structure s)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(27);

            w.PushInt(2);
            w.PushLong(s.ID);
            w.PushByte((byte)s.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(s.Name));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void closeDoor(Client c, StructureNode n)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(17);

            w.PushLong(n.parent.ID);
            w.PushShort((short)(n.relativeOffset.X + n.parent.Position.X));
            w.PushShort((short)(n.relativeOffset.Y + n.parent.Position.Y));
            if (n.flipped == true)
                w.PushByte(0);
            else
                w.PushByte(1);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void openDoor(Client c, StructureNode n)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(32);

            w.PushLong(n.parent.ID);
            w.PushShort((short)(n.relativeOffset.X + n.parent.Position.X));
            w.PushShort((short)(n.relativeOffset.Y + n.parent.Position.Y));
            if (n.flipped == true)
                w.PushByte(0);
            else
                w.PushByte(1);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void ClipNode(Client c, StructureNode n, bool clip)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(18);

            w.PushLong(n.parent.ID);
            w.PushShort((short)(n.relativeOffset.X + n.parent.Position.X));
            w.PushShort((short)(n.relativeOffset.Y + n.parent.Position.Y));

            if (n.flipped == true)
                w.PushByte(1);
            else
                w.PushByte(0);

            w.PushByte((byte)(clip == true ? 1 : 0));
            
            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void MarkTile(Client c, Structure s)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(86);

            w.PushLong(s.ID);

            w.PushByte(1); //for now, just 1 tile.. :<

            w.PushShort((short)s.Position.X);
            w.PushShort((short)s.Position.Y);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Sends a structure packet,
        /// this also sends the StructureNodes involved.
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="s">Structure to send</param>
        public static void SendAddStructure(Client c, Structure s)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(47);

            w.PushLong(s.ID);
            w.PushByte((byte)s.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(s.Name));

            w.PushByte(0); //unused

            w.PushShort((short)s.Position.X);
            w.PushShort((short)s.Position.Y);

            byte[] t = w.Finish();

            c.Encrypt(t, 0, t.Length);
            c.Send(t);

            foreach (StructureNode n in s.Nodes)
            {
                SendStructureNode(c, n);
            }

            MarkTile(c, s);
        }

        /// <summary>
        /// Sends a structure packet,
        /// this also sends the StructureNodes involved.
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="s">Structure to send</param>
        public static void SendAddStructure_Bare(Client c, Structure s)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(47);

            w.PushLong(s.ID);
            w.PushByte((byte)s.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(s.Name));

            w.PushByte(0); //unused

            w.PushShort((short)s.Position.X); //?
            w.PushShort((short)s.Position.Y); //?

            byte[] t = w.Finish();

            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Send a structurenode (wall, etc.)
        /// </summary>
        /// <param name="c">Client</param>
        /// <param name="node">Node to send.</param>
        public static void SendStructureNode(Client c, StructureNode node)
        {
            if (node == null)
                return;

            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(89);

            w.PushLong(node.parent.ID);
            w.PushShort((short)(node.parent.Position.X + node.relativeOffset.X));
            w.PushShort((short)(node.parent.Position.Y + node.relativeOffset.Y));

            if (node.flipped)
                w.PushByte(1);
            else
                w.PushByte(0);

            w.PushByte((byte)node.type);

            w.PushByte((byte)node.material.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(node.material));

            if (node.coloured)
            {
                w.PushByte(1);

                w.PushByte(node.colour.R);
                w.PushByte(node.colour.G);
                w.PushByte(node.colour.B);
            }
            else
            {
                w.PushByte(0);
            }

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);

            node.SetLocked(node.locked);
        }

        /// <summary>
        /// Sends all the structures in the pool
        /// </summary>
        /// <param name="c">Client to send to</param>
        public static void SendAll(Client c)
        {
            foreach (ObjectBase b in ObjectPool.Pool)
            {
                if (b.Type == ObjectType.Structure)
                {
                    Structure s = (Structure)b;
                    SendAddStructure(c, s);

                    Sprite.Add(c, 8932, 2, s.Position);
                }
            }
        }
    }
}
