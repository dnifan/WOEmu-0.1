using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WO.Core
{
    public static class Ban
    {
        /// <summary>
        /// Opens bans.txt and loads all bans in it.
        /// </summary>
        public static void Initialize()
        {
            FileStream fs = File.Open("Bans.txt", FileMode.OpenOrCreate);
            bannedIPs = new List<string>();
            StreamReader reader = new StreamReader(fs);

            while (!reader.EndOfStream)
            {
                string IP = reader.ReadLine();
                bannedIPs.Add(IP);
            }
        }

        /// <summary>
        /// Checks whether the specified IP is banned.
        /// NOTE: Include the port (:xxxx)
        /// </summary>
        /// <param name="IP">The IP to check.</param>
        /// <returns>Whether banned or not?</returns>
        public static bool IsBanned(string IP)
        {
            string[] t = IP.Split(new char[] {':'}); //to seperate the port.
            return bannedIPs.Contains(t[0]);
        }

        private static List<string> bannedIPs;
    }
}
