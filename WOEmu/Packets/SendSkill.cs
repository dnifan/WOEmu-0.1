using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// The skills
    /// </summary>
    public static class SendSkill
    {
        public static void UpdateSkill(Client c, long skillID, float newval)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(42);

            w.PushLong(skillID);
            w.PushFloat(newval);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        /// <summary>
        /// Sends a skill to a player
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="parent">The parent skill of this skill</param>
        /// <param name="sID">ID - THIS MAY NOT INTERFERE WITH OTHER IDs!</param>
        /// <param name="SkillName">Name of the skill</param>
        /// <param name="skillValue">Skill value</param>
        /// <param name="skillMax">Highest skill</param>
        public static void SendTo(Client c, long parent, long sID, string SkillName, float skillValue, float skillMax)
        {
            PacketWriter writer = new PacketWriter();

            writer.PushShort(0); //len
            writer.PushByte(65);

            writer.PushLong(parent);
            writer.PushLong(sID);

            writer.PushByte((byte)SkillName.Length);
            writer.PushBytes(Encoding.ASCII.GetBytes(SkillName));

            writer.PushFloat(skillValue);
            writer.PushFloat(skillMax);

            writer.PushByte(0);

            byte[] reply = writer.Finish();

            c.Encrypt(reply, 0, reply.Length);
            c.Send(reply);
        }
    }
}
