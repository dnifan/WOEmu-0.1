using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;
using WOEmu.Objects;

namespace WOEmu.Packets
{
    public static class ObjectParams
    {
        /// <summary>
        /// Make the object stop emitting light to the environment.
        /// </summary>
        /// <param name="c">Client to send the packet to.</param>
        /// <param name="b">The object to stop emitting.</param>
        public static void StopEmittingLight(Client c, ObjectBase b)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(74);

            w.PushLong(b.ID);
            w.PushByte(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Let the object not be on fire anymore
        /// </summary>
        /// <param name="c">Client to send the packet to.</param>
        /// <param name="b">The object to do</param>
        public static void StopFire(Client c, ObjectBase b)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(74);

            w.PushLong(b.ID);
            w.PushByte(1);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Reset the invisibility status of the object
        /// </summary>
        /// <param name="c"></param>
        /// <param name="b"></param>
        public static void ResetInvisibility(Client c, ObjectBase b)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(74);

            w.PushLong(b.ID);
            w.PushByte(2);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Reset the lighting of the object
        /// </summary>
        /// <param name="c"></param>
        /// <param name="b"></param>
        public static void ResetLighting(Client c, ObjectBase b)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(74);

            w.PushLong(b.ID);
            w.PushByte(3);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Make the object emit light
        /// </summary>
        /// <param name="c">Client to send the packet to</param>
        /// <param name="b">The object to emit light</param>
        /// <param name="rgb">RGB byte array ({R,G,B})</param>
        public static void EmitLight(Client c, ObjectBase b, byte[] rgb)
        {
            PacketWriter w = new PacketWriter();
            w.PushShort(0);
            w.PushByte(73);

            w.PushLong(b.ID);
            w.PushByte(0);
            w.PushByte(rgb[0]);
            w.PushByte(rgb[1]);
            w.PushByte(rgb[2]);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Put the object on fire
        /// </summary>
        /// <param name="c">Client to send the packet to.</param>
        /// <param name="b">The object to set on fire.</param>
        /// <param name="radius">The radius the fire should be in (default: 1)</param>
        public static void Fire(Client c, ObjectBase b, byte radius)
        {
            PacketWriter w = new PacketWriter();
            w.PushShort(0);
            w.PushByte(73);
            
            w.PushLong(b.ID);
            w.PushByte(1);
            
            w.PushByte(radius);
            w.PushByte(0);
            w.PushByte(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Make a object invisible?
        /// </summary>
        /// <param name="c">Client c</param>
        /// <param name="b">Object to make invisible</param>
        /// <param name="b1">Unknown, tested: 100</param>
        /// <param name="b2">Unknown, tested: 50</param>
        public static void Invisible(Client c, ObjectBase b, byte b1, byte b2)
        {
            PacketWriter w = new PacketWriter();
            w.PushShort(0);
            w.PushByte(73);

            w.PushLong(b.ID);
            w.PushByte(2);

            w.PushByte(b1);
            w.PushByte(b2);
            w.PushByte(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Set the lighting on a object.
        /// </summary>
        /// <param name="c">Client to send the packet to</param>
        /// <param name="b">The object to change the lighting of.</param>
        /// <param name="rgb">RGB byte array.</param>
        public static void Lighting(Client c, ObjectBase b, byte[] rgb)
        {
            PacketWriter w = new PacketWriter();
            w.PushShort(0);
            w.PushByte(73);

            w.PushLong(b.ID);
            w.PushByte(3);
            w.PushByte(rgb[0]);
            w.PushByte(rgb[1]);
            w.PushByte(rgb[2]);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
