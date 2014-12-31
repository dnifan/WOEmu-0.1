using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Packets;
using System.Text;

namespace WOEmu.Misc
{
    public static class NewPlayer
    {
        /// <summary>
        /// Handle the event of when a new client joins
        /// </summary>
        /// <param name="newClient">The new client</param>
        public static void Handle(Client newClient)
        {
            foreach (Client cli in Program.clients)
            {
                if (cli == newClient)
                    continue;

                //Send the other client to the newClient
                AddActor.SendTo(cli, newClient.player);
                //Send the newClient to the other client
                AddActor.SendTo(newClient, cli.player);
            }
        }
    }
}
