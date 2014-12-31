using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Terrain;
using System.Text;
using WOEmu.Misc;

namespace WOEmu.Packets
{
    /// <summary>
    /// Another packet which ate off my head.
    /// </summary>
    public static class TerrainPacket
    {
        /// <summary>
        /// Sends the terrain data to a client
        /// </summary>
        /// <param name="c">Client to send to</param>
        public static void SendTo(Client c, Terrain.Terrain terr)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(35);

            w.PushShort(terr.cornerX);
            w.PushShort(terr.cornerY);
            w.PushShort(terr.Width);
            w.PushShort(terr.Height);

            foreach (Tile t2 in terr.Tiles)
            {
                w.PushInt(t2.TileFlags());
            }

            byte[] t = w.Finish();

            c.Encrypt(t, 0, t.Length);
            c.Send(t);

            w.Dispose();
        }

        public static void UpdateTile(Client c, Tile t)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(35);

            w.PushShort((short)(t.X));
            w.PushShort((short)(t.Y));
            w.PushShort(1);
            w.PushShort(1);

            w.PushInt(t.TileFlags());

            byte[] te = w.Finish();
            c.Encrypt(te, 0, te.Length);
            c.Send(te);
        }
    }
}
