using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// Sends the initial data of the game to a player
    /// </summary>
    public static class PlayerInformation
    {
        /// <summary>
        /// Sends
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="p">Player?</param>
        public static void SendTo(Client c, Objects.Player p)
        {
            PacketWriter _writer = new PacketWriter();

            string WelcomeMessage = Program.configFile.GetValue("WelcomeMsg");

            System.Console.WriteLine(p.Name + " is spawning at " + p.Position.ToString());

            _writer.PushShort(0); //len
            _writer.PushByte(0); //id, which is 0

            _writer.PushByte(1); //success byte
            _writer.PushShort((short)WelcomeMessage.Length);              //length of welcome message
            _writer.PushBytes(Encoding.ASCII.GetBytes(WelcomeMessage));   //welcome message

            _writer.PushFloat(0.0f);
            _writer.PushFloat(c.player.Position.X);
            _writer.PushFloat(c.player.Position.Z);
            _writer.PushFloat(c.player.Position.Y);

            _writer.PushByte(0); //tile or something lulz

            _writer.PushLong(0x0000000014DEC241L); //
            _writer.PushLong(0); //Time offset.

            _writer.PushShort((short)SkeletonLine.Length);                //length of the player's model
            _writer.PushBytes(Encoding.ASCII.GetBytes(SkeletonLine));     //player's model

            _writer.PushByte((byte)(c.Dev == true ? 1 : 0));
            _writer.PushByte(0); //another boolean
            
            _writer.PushShort((short)0);

            byte[] Tosend = _writer.Finish();
            c.Encrypt(Tosend, 0, Tosend.Length);
            c.Send(Tosend);
        }

        public static string SkeletonLine = "model.creature.humanoid.human.player.male.jenn.";
    }
}
