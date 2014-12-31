using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace WOEmu.Misc
{
    /*public class Timer
    {
        /// <summary>
        /// Timer constructor
        /// </summary>
        /// <param name="seconds">Seconds from current time this timer will expire</param>
        public Timer(short seconds)
        {
            Secs = seconds;
            System.Timers.Timer = new System.Timers.Timer(
        }

        /// <summary>
        /// The method to call when this Timer is finished.
        /// </summary>
        public virtual void OnFinish()
        {
            
        }

        /// <summary>
        /// A list of params, which the inherited timer can use.
        /// </summary>
        public List<object> Params;

        public short Secs;
    }*/

    public class WTimer : Timer
    {
        public WTimer(double secs)
            : base(secs)
        {
        }

        public void AddArg(object arg)
        {
            Argument = arg;
        }

        public object Argument;
    }

    /// <summary>
    /// Contains all timers
    /// </summary>
    public static class TimerPool
    {
        public static void Initialize()
        {
            Timers = new List<WTimer>();
        }

        public static void GarbageCollect()
        {
            foreach (WTimer t in Timers)
            {
                if (t.Enabled == false)
                {
                    Timers.Remove(t);
                }
            }
        }

        public static void AddTimer(WTimer t)
        {
            Timers.Add(t);
            t.Start();
            t.Enabled = true;
            System.Console.WriteLine("[TimerPool]: Added timer, with {0} interval.", t.Interval);
        }

        public static void Step()
        {
            foreach (WTimer t in Timers)
            {
                
            }
        }

        public static List<WTimer> Timers;
    }
}
