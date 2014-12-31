using System;
using System.Collections.Generic;
using System.Text;
using WO.Core;

namespace WOEmu.Packets
{
    /// <summary>
    /// NON-FUNCTIONAL! MATH NEEDED! :P
    /// </summary>
    public static class UpdatePosition
    {
        /// <summary>
        /// Sends a movement packet to a client.
        /// </summary>
        /// <param name="c">Client to send to.</param>
        /// <param name="rotation">Rotation of the player</param>
        /// <param name="relativeOffset">X and Y relative offset</param>
        public static void SendTo(Client c, Objects.ActorBase b, sbyte rotation, Vector3 relativeOffset)
        {
            PacketWriter writer = new PacketWriter();

            writer.PushShort(0);
            writer.PushByte(39);

            writer.PushLong(b.ID);

            writer.PushSByte(rotation);
            writer.PushSByte((sbyte)relativeOffset.Y);
            writer.PushSByte((sbyte)relativeOffset.X);
           
            byte[] Tosend = writer.Finish();
            c.Encrypt(Tosend, 0, Tosend.Length);
            
            //do announce or osmething
            c.Send(Tosend);
        }

        public static void Broadcast(Objects.ActorBase b, sbyte rot, Vector3 relativeOffset)
        {
            foreach (Client t in Program.clients)
            {
                if (t.player.ID == b.ID)
                    continue;

                SendTo(t, b, rot, relativeOffset);
            }
        }
    }
}
