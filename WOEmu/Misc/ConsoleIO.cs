using System;
using System.Collections.Generic;
using System.Text;

namespace WOEmu.Misc
{
    public class ConsoleIO
    {
        /// <summary>
        /// Thread
        /// </summary>
        public void Run()
        {
            while (true)
            {
                string cmd = System.Console.ReadLine();
                string[] Tokens = cmd.Split(new char[] { ' ' });

                if (Tokens[0] == "exit" || Tokens[0] == "quit")
                {
                    WO.Core.Logger.Logger.printInfo("Shutting down on demand...");

                    foreach (Client c in Program.clients)
                    {
                        Packets.ServerShuttingDown.SendTo(c);
                        c.SaveData();
                        c.tThread.Abort();
                    }

                    //save heightmap blah blah blah
                    Terrain.MapSaver.SaveMap();
                    Environment.Exit(1);
                }
                else if (Tokens[0] == "say")
                {
                    foreach (Client c in Program.clients)
                    {
                        Packets.Chat.SendTo(c, ":Local", "[CONSOLE]: " + Tokens[1]);
                    }
                }
                else if (Tokens[0] == "who")
                {
                    WO.Core.Logger.Logger.printInfo("Users currently online:");
                    foreach (Client c in Program.clients)
                    {
                        System.Console.WriteLine(" - " + c.player.Name);
                    }
                }
                else if (Tokens[0] == "kick")
                {
                    if (Tokens.Length != 2)
                    {
                        WO.Core.Logger.Logger.printInfo("Syntax error: kick <player name>");
                        continue;
                    }

                    string name = Tokens[1];
                    foreach (Client c in Program.clients)
                    {
                        if (c.player.Name == name)
                        {
                            Packets.ServerShuttingDown.SendTo(c);
                        }
                    }
                }
                else if (Tokens[0] == "scriptinfo")
                {
                    WO.Core.Logger.Logger.printInfo("Loaded game scripts:");
                    foreach (string s in Scripting.ScriptingInterface.gameModuleList)
                    {
                        WO.Core.Logger.Logger.printInfo("* " + s);
                    }
                    WO.Core.Logger.Logger.printInfo("Loaded object scripts:");
                    foreach (string s in Scripting.ScriptingInterface.objectModuleList)
                    {
                        WO.Core.Logger.Logger.printInfo("* " + s);
                    }
                }
                else if (Tokens[0] == "savemap")
                {
                    Terrain.MapSaver.SaveMap();
                }
            }
        }
    }
}
