
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WOEmu.Config;

using WOEmu.Objects;
using WO.Core.Logger;
using WO.Core;

namespace WOEmu
{
    public static class Program
    {
        public static void doExit()
        {
            Terrain.MapSaver.SaveMap();
            //
            foreach (Client c in Program.clients)
            {
                Packets.ServerShuttingDown.SendTo(c);
                c.SaveData();
                c.tThread.Abort();
            }
            WO.Core.Logger.Logger.printInfo("Quit successfully!");
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            Logger.InitLogger("woemu.log");
            Ban.Initialize();

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Blue;
                        System.Console.WriteLine(@"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
                        System.Console.WriteLine(@"\\\           _          __  _____   _____       ___  ___   _   _           \\\");
                        System.Console.WriteLine(@"\\\          | |        / / /  _  \ | ____|     /   |/   | | | | |          \\\");
                        System.Console.WriteLine(@"\\\          | |  __   / /  | | | | | |__      / /|   /| | | | | |          \\\");
                        System.Console.WriteLine(@"\\\          | | /  | / /   | | | | |  __|    / / |__/ | | | | | |          \\\");
                        System.Console.WriteLine(@"\\\          | |/   |/ /    | |_| | | |___   / /       | | | |_| |          \\\");
                        System.Console.WriteLine(@"\\\          |___/|___/     \_____/ |_____| /_/        |_| \_____/          \\\");
                        System.Console.WriteLine(@"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
            Console.BackgroundColor = ConsoleColor.Black;
            System.Console.Write("\n");

            Logger.printInfo("Setting up Signalhandler...");
            SignalHandler.hookKeys();

            Logger.printInfo("Initializing terrain...");
            WOEmu.Terrain.Map.initMap();

            Logger.printInfo("Initializing object pool...");
            ObjectPool.Initialize();

            Logger.printInfo("Initializing timer pool...");
            Misc.TimerPool.Initialize();

            Logger.printInfo("Loading config...");
            #region Config file 
            configFile = new Configuration();
            configFile.Load("woemu.conf");
            sqlData = new WO.Core.SqlData();
            sqlData.host = configFile.GetValue("SQLHost");
            sqlData.user = configFile.GetValue("SQLUser");
            sqlData.pass = configFile.GetValue("SQLPass");
            sqlData.db = configFile.GetValue("SQLName");
            #endregion

            int numStructures = Terrain.StructureLoader.LoadFromDB();
            Logger.printInfo("Loaded " + numStructures + " structures from database!");

            int numItems = Terrain.ItemLoader.LoadFromDB();
            Logger.printInfo("Loaded " + numItems + " items from database!");

            int numStaticItems = Terrain.ItemLoader.LoadStatic();
            Logger.printInfo("Loaded " + numStaticItems + " static items!");

            int numNPCs = Terrain.NPCLoader.LoadFromDB();
            Logger.printInfo("Loaded " + numNPCs + " npcs from database!");

            int numWalls = Terrain.WallLoader.LoadFromDB();
            Logger.printInfo("Loaded " + numWalls + " walls from database!");

            Menus.MenuProxy.Init();

            Scripting.ScriptingInterface.Init("scripts");
            Logger.printInfo("Compiled all scripts!");
            Scripting.ScriptingInterface.runGameHook("Init", null);

            Logger.printInfo("Listening...");

            Misc.ConsoleIO io = new WOEmu.Misc.ConsoleIO();
            Thread consoleThread = new Thread(io.Run);
            consoleThread.Start();

            TcpListener l = new TcpListener(IPAddress.Any, 48000);
            l.Start();

            clients = new List<Client>();

            while (true)
            {
                TcpClient c = l.AcceptTcpClient();
                EndPoint ep = c.Client.RemoteEndPoint;
                string IP = ep.ToString();

                if (Ban.IsBanned(IP))
                {
                    Logger.printWarning("Client " + IP + " is banned! Dropping connection!");
                    c.Close();
                    continue;
                }

                Logger.printInfo("[Connection From]: " + IP);

                Client cli = new Client(c);
                clients.Add(cli);
                cli.StartListening();
            }
        }

        public static List<Client> clients;
        public static Configuration configFile;
        public static WO.Core.SqlData sqlData;
    }
}
