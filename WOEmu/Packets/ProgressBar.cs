using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;
using WOEmu.Misc;

namespace WOEmu.Packets
{
    /// <summary>
    /// The progress bar you see when performing an action.
    /// </summary>
    public static class ProgressBar
    {
        /// <summary>
        /// Send a progress bar
        /// </summary>
        /// <param name="c">Client to send to</param>
        /// <param name="title">Title of the action</param>
        /// <param name="seconds">Seconds the bar should take to finish</param>
        public static void SendTo(Client c, string title, short seconds)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(44);

            w.PushByte((byte)title.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(title));

            w.PushShort((short)(seconds * 10));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);

            //Initialize timer stuff

            WTimer timer = new WTimer((double)seconds);
            timer.AddArg(c);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerFinishEvent);
            TimerPool.AddTimer(timer);
        }

        public static void TimerFinishEvent(object s, System.Timers.ElapsedEventArgs e)
        {
            Client c = (Client)s;
            Finish(c);
        }

        /// <summary>
        /// Make the progress bar finish.
        /// </summary>
        /// <param name="c">Client to send to.</param>
        public static void Finish(Client c)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(44);

            w.PushByte(0);
            w.PushShort(0);

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }

        public static void SetTime(Client c, short secs)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(85);

            w.PushShort((short)(secs * 10));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
