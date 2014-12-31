using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.PacketHandlers
{
    public static class ChatHandler
    {
        public static void Read(Client c, byte[] Packet)
        {
            PacketReader reader = new PacketReader(Packet);

            reader.PopByte();
            byte len1 = reader.PopByte();
            byte[] buf1 = reader.PopBytes(len1);

            byte len2 = reader.PopByte();
            byte[] buf2 = reader.PopBytes(len2);

            string s1 = Encoding.ASCII.GetString(buf1);
            string s2 = Encoding.ASCII.GetString(buf2);

            if (s2[0] == '\\' || s2[0] == '/')
            {
                //command issued
                if (!Misc.CommandHandler.HandleCommand(c, s2))
                {
                    //unknown command
                    c.SendChat("Unknown command.", ":Event");
                }
                return;
            }

            Scripting.ScriptingInterface.runGameHook("OnChat", new object[] { c, s1, s2 });

            //--

            foreach (Client t in Program.clients)
            {
                if (c.Dev == true)
                    Packets.Chat.SendTo(t, s1, c.player.Name + ": " + s2, new Color(189.0f, 189.0f, 0.0f));
                else
                    Packets.Chat.SendTo(t, s1, c.player.Name + ": " + s2);
            }
        }
    }
}
