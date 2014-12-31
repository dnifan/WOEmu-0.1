using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// This adds an 'actor' to the game world.
    /// </summary>
    public static class AddActor
    {
        /// <summary>
        /// Sends the packet
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="act">The actor to send</param>
        public static void SendTo(Client c, ActorBase act)
        {
            PacketWriter writer = new PacketWriter();
            
            writer.PushShort(0);
            writer.PushByte(43);

            writer.PushLong(act.ID);
            writer.PushFloat(act.Rotation);
            writer.PushFloat(act.Position.Z);

            writer.PushByte((byte)act.Model.Length);
            writer.PushByte(0);
            writer.PushBytes(Encoding.ASCII.GetBytes(act.Model));

            writer.PushByte((byte)act.Name.Length);
            writer.PushBytes(Encoding.ASCII.GetBytes(act.Name));

            writer.PushFloat(act.Position.X);
            writer.PushFloat(act.Position.Y);

            writer.PushByte(0);
            writer.PushByte(1);
            writer.PushByte(0);

            byte[] tosend = writer.Finish();
            
            c.Encrypt(tosend, 0, tosend.Length);
            c.Send(tosend);

            Creature.SendIFF(c, act, 1);
        }

        /// <summary>
        /// Seems some weird teleport packet
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="b">The actor</param>
        /// <param name="relpos">The relative position</param>
        public static void Update(Client c, ActorBase b, Vector3 relpos)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(78);

            w.PushLong(b.ID);
            w.PushSByte((sbyte)relpos.X);
            w.PushSByte((sbyte)relpos.Y);
            w.PushShort((short)relpos.Z);
            w.PushSByte((sbyte)b.Rotation);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Send from pool
        /// </summary>
        /// <param name="c">Client to send to</param>
        public static void SendAll(Client c)
        {
            foreach (ObjectBase b in ObjectPool.Pool)
            {
                if (b.Type == ObjectType.NPC)
                {
                    ActorBase npc = (ActorBase)b;
                    SendTo(c, npc);
                    Creature.SendDamage(c, npc);
                    Creature.SetColor(c, npc, new Color(128.0f, 244.0f, 112.0f));
                    Creature.SetScale(c, 60, 60, 60, npc);
                    
                    //Packets.UseItem.SendTo(c, npc, "model.weapon.axe.large"); // Test item use by giving them a huge axe.
                    // Here we can attach any models to the NPC needed.
                    
                }
            }
        }

        public static void Broadcast(WOEmu.Objects.ActorBase b)
        {
            foreach (Client t in Program.clients)
            {
                if (t.player.ID == b.ID)
                    continue;
                
                SendTo(t, b);
            }
        }

        public static void handleNewPlayer(Client c)
        {
            foreach (Client t in Program.clients)
            {
                if (c == t)
                    continue;
                
                //send this player to the other client
                SendTo(t, c.player);
                //send the other player's actor to this client
                SendTo(c, t.player);
            }
        }
    }
}
