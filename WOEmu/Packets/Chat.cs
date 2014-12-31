using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// Chat class
    /// </summary>
    public static class Chat
    {
        public static void broadcastChat(string txt)
        {
            foreach (Client c in Program.clients)
            {
                c.SendChat(txt, ":Local");
            }
        }

        public static void announceNewPlayerLocal(Client cli)
        {
            foreach (Client c in Program.clients)
            {
                if (c != cli)
                {
                    JoinPlayer(c, ":Local", cli.player.Name);
                    JoinPlayer(cli, ":Local", c.player.Name);

                    if (cli.Dev == true)
                        Packets.Chat.SendTo(c, ":Event", "Developer" + cli.player.Name + " joined the game!", new Color(255.0f, 0.0f, 0.0f));
                    else
                        Packets.Chat.SendTo(c, ":Event", cli.player.Name + " joined the game!", new Color(0.0f, 255.0f, 0.0f));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cli">The player that leaves</param>
        public static void announceLeavePlayerLocal(Client cli)
        {
            foreach (Client c in Program.clients)
            {
                if (c != cli)
                {
                    LeavePlayer(c, ":Local", cli.player.Name);
                }
            }
        }

        public static void LeavePlayer(Client c, string dest, string name)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(60);

            w.PushByte((byte)dest.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(dest));

            w.PushByte((byte)name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(name));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void JoinPlayer(Client c, string dest, string name)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(59);

            w.PushByte((byte)dest.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(dest));

            w.PushByte((byte)name.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(name));

            w.PushLong(100);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Send a chat message.
        /// </summary>
        /// <param name="c">Client to send to.</param>
        /// <param name="Dest">The chat window (:Event, etc.)</param>
        /// <param name="Message">The message to send.</param>
        public static void SendTo(Client c, string Dest, string Message)
        {
            PacketWriter _writer = new PacketWriter();

            _writer.PushShort(0); //len
            _writer.PushByte(82); //chat (82)

            _writer.PushByte((byte)Dest.Length);
            _writer.PushBytes(Encoding.ASCII.GetBytes(Dest));

            _writer.PushByte(255); //R
            _writer.PushByte(255); //G
            _writer.PushByte(255); //B

            _writer.PushShort((short)Message.Length);
            _writer.PushBytes(Encoding.ASCII.GetBytes(Message));
            byte[] Tosend = _writer.Finish();

            c.Encrypt(Tosend, 0, Tosend.Length);
            c.Send(Tosend);
        }

        /// <summary>
        /// Sends a chat message, colored.
        /// </summary>
        /// <param name="c">Client to send to.</param>
        /// <param name="Dest">The destination window.</param>
        /// <param name="Message">The message.</param>
        /// <param name="col">The color which is shown.</param>
        public static void SendTo(Client c, string Dest, string Message, Color col)
        {
            PacketWriter _writer = new PacketWriter();

            _writer.PushShort(0); //len
            _writer.PushByte(82); //chat (82)

            _writer.PushByte((byte)Dest.Length);
            _writer.PushBytes(Encoding.ASCII.GetBytes(Dest));

            _writer.PushByte((byte)col.R); //R
            _writer.PushByte((byte)col.G); //G
            _writer.PushByte((byte)col.B); //B

            _writer.PushShort((short)Message.Length);
            _writer.PushBytes(Encoding.ASCII.GetBytes(Message));
            byte[] Tosend = _writer.Finish();

            c.Encrypt(Tosend, 0, Tosend.Length);
            c.Send(Tosend);
        }
    }
}
