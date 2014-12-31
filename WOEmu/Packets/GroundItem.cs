using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// Every item that can lie on the ground, is called a 'ground item'.
    /// </summary>
    public static class GroundItem
    {
        public static void SendRemove(Client c, Item i)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(69);

            w.PushLong(i.ID);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendRotate(Client c, Item i)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(95);

            w.PushLong(i.ID);
            w.PushFloat(i.Rotation);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SendRename(Client c, Item i)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(53);

            w.PushLong(i.ID);
            w.PushByte((byte)i.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(i.Name));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Sends a ground item to a player
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="i">Item to send</param>
        public static void SendTo(Client c, Item i)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(81);

            w.PushLong(i.ID);
            w.PushFloat(0.0f); //? rotation?

            w.PushByte((byte)i.Model.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(i.Model));

            w.PushByte((byte)i.Name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(i.Name));

            w.PushFloat(i.Position.Z);
            w.PushFloat(i.Position.X);
            w.PushFloat(i.Position.Y);

            w.PushByte(0);

            byte[] t = w.Finish();

            c.Encrypt(t, 0, t.Length);
            c.Send(t);

            #region Afterwards...
            if (i.OnFire)
            {
                ObjectParams.Fire(c, i, i.fireRadius);
            }
            if (i.emitLight)
            {
                WO.Core.Logger.Logger.printDebug("Emitting light!");
                ObjectParams.EmitLight(c, i, new byte[] { 255, 255, 255 });
            }
            #endregion
        }

        /// <summary>
        /// Send all the Items from the Pool
        /// </summary>
        /// <param name="c">Client to send to.</param>
        public static void SendAll(Client c)
        {
            foreach (ObjectBase b in ObjectPool.Pool)
            {
                if (b.Type == ObjectType.Item)
                {
                    Item i = (Item)b;
                    SendTo(c, i);
                }
            }
        }
    }
}
