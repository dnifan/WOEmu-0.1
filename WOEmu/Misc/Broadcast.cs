using System;
using System.Collections.Generic;
using WOEmu;
using System.Text;

namespace WOEmu.Misc
{
    public static class Broadcast
    {
        public static void Send(byte[] pkt)
        {
            if (Program.clients.Count == 0)
                return;

            foreach (Client c in Program.clients)
            {
                byte[] local = new byte[pkt.Length];
                Array.Copy(pkt, local, pkt.Length);
                c.Encrypt(local, 0, local.Length);
                c.Send(local);
            }
        }
    }
}
