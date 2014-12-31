using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WO.Core;
using WOEmu.PacketHandlers;
using WOEmu.Packets;

//INFO:
//'unfinished' before an item makes it render differently..



namespace WOEmu
{
    public class Client
    {
        #region Constructor
        public Client(TcpClient t)
        {
            sock = t;
            player = new WOEmu.Objects.Player("model.creature.humanoid.human.player.male.jenn.", "Unknown");
            this.player.Position = new Vector3(0, 0, 0);
        }

        #endregion

        #region Thread stuff
        public void ListenThread()
        {
            random_1 = new WO.Core.Encryption.WORandom(0x63eb66fL);
            random_2 = new WO.Core.Encryption.WORandom(0x63eb66fL);

            try
            {
                while (true)
                {
                    stream = sock.GetStream();
                    byte[] PacketLength = new byte[2];
                    stream.Read(PacketLength, 0, 2);

                    Decrypt(PacketLength);
                    PacketReader _len_reader = new PacketReader(PacketLength);

                    short size = _len_reader.PopShort();

                    byte[] Packet = new byte[size];
                    stream.Read(Packet, 0, size);
                    Decrypt(Packet);

                    HandlePacket(Packet);
                }
            }
            catch (System.IO.IOException)
            {
                WO.Core.Logger.Logger.printInfo("Client '" + player.Name + "' disconnected.");
                SaveData();
                Program.clients.Remove(this);
                removeNullClients();
                Chat.announceLeavePlayerLocal(this);
                this.tThread.Abort();
            }
            catch (Exception exc)
            {
                System.Console.WriteLine("Unhandled exception: {0}", exc.ToString());

                WO.Core.Logger.Logger.AppendLine(exc.ToString());

                SaveData();
                Program.clients.Remove(this);
                removeNullClients();

                //Player leaves local chat.
                Chat.announceLeavePlayerLocal(this);

                this.tThread.Abort();
            }
        }

        private void removeNullClients()
        {
            List<Client> newClients = new List<Client>();
            foreach (Client c in Program.clients)
            {
                if (c != null)
                {
                    newClients.Add(c);
                }
            }
            Program.clients = newClients;
        }

        public void StartListening()
        {
            tThread = new Thread(ListenThread);
            tThread.Start();
        }
        #endregion

        #region Packet Handling

        public void HandlePacket(byte[] Packet)
        {
            switch (Packet[0])
            {
                case 0:
                    //login
                    if (!LoginHandler.Read(this, Packet))
                    {
                        WO.Core.Logger.Logger.printWarning("User logged in with wrong password!");

                        //sock.Close();
                        Program.clients.Remove(this);
                        removeNullClients();
                        return;
                    }
                    else
                    {
                        //send terrain stuff
                        //TerrainPacket.SendTo(this);
                        Terrain.Map.sendAll(this);
                        StructurePacket.SendAll(this);
                        Walls.SendAll(this);
                        GroundItem.SendAll(this);
                        AddActor.SendAll(this);
                        PlayerStats.SendTo(this);
                        Time.SendTo(this);                          //sends the current time.

                        Inventory.sendInitialInventory(this);
                        Inventory.loadFromDB(this);

                        PlayerStats.SetSpeed(this, 1.0f);

                        //send all clients in the chat channel thing
                        Chat.announceNewPlayerLocal(this);

                        //multi-player visuals.
                        AddActor.handleNewPlayer(this);

                        InterfaceOptions.SendCompass(this);

                        Chat.SendTo(this, ":Event", "There are " + Program.clients.Count + " clients online.", new Color(0.0f, 255.0f, 0.0f));
                    }
                    break;

                case 16:
                    //request info on tile
                    ObjectInfo.Read(this, Packet);
                    break;

                case 29:
                    //Closes the trade window.
                    break;

                case 39:
                    //movement
                    Movement.Read(this, Packet);
                    break;

                case 50:
                    //Player has confirmed teleportation.
                    break;

                case 51:
                    //Player has confirmed trading.
                    //Send this to the other player?
                    break;

                case 52:
                    //Player moves items around (either in inventory or somewhere else)
                    MoveItemsHandler.Read(this, Packet);
                    break;

                case 61:
                    //means client has acknowledged its speed.
                    break;

                case 62:
                    //means client has successfully altered weather.
                    break;

                case 64:
                    System.Console.WriteLine("stance set packet!");
                    StanceHandler.Read(this, Packet);
                    break;

                case 70:
                    //I dont know what this is, but it seems rather useless
                    //probably some "Iam alive lolz!" packet ;-)
                    break;

                case 71:
                    //BML response

                    break;

                case 82:
                    ChatHandler.Read(this, Packet);
                    break;

                case 83:
                    //examine, double click.
                    //ExamineHandler.Read(this, Packet);
                    ObjectInteraction.Read(this, Packet);
                    break;

                case 94:
                    //Means client has acknowledged wind speed.
                    break;

                case 96:
                    //Means client has acknowledged rowing speed.
                    break;

                default:
                    Console.WriteLine("Unknown packet {0} received...", Packet[0]);
                    break;
            }
        }

        #endregion

        #region Some methods

        public void SaveData()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("UPDATE accounts SET posX = '" + player.Position.X + "', posY = '" + player.Position.Y + "', posZ = '" + player.Position.Z + "' WHERE User = '" + player.Name + "';");
            s.Dispose();
        }

        public void Send(byte[] Data)
        {
            stream.Write(Data, 0, Data.Length);
        }

        public void UpdateStats()
        {
            PlayerStats.SendHealthStamina(this);
            PlayerStats.SendFood(this);
            PlayerStats.SendWater(this);
        }

        /// <summary>
        /// Allows for easy chat sending
        /// </summary>
        /// <param name="chat">Msg to send</param>
        /// <param name="chann">Channel, prefixed with ':'</param>
        public void SendChat(string chat, string chann)
        {
            Packets.Chat.SendTo(this, chann, chat);
        }
        #endregion

        #region Encryption

        public void Encrypt(byte[] Data, int k, int l)
        {
            for (; k < l; k++)
            {
                if (--e < 0)
                {
                    e = random_2.nextInt(100) + 1;
                    f = (byte)random_2.nextInt(256);
                    g = (byte)random_2.nextInt(256);
                }
                Data[k] = (byte)(Data[k] - g);
                Data[k] ^= (byte)f;
            }
        }

        public void Decrypt(byte[] Data)
        {
            for (int x = 0; x < Data.Length; x++)
            {
                if (--h < 0)
                {
                    h = random_1.nextInt(100) + 1;
                    i = (byte)random_1.nextInt(256);
                    j = (byte)random_1.nextInt(256);
                }
                Data[x] ^= (byte)i;
                Data[x] = (byte)(Data[x] + j);
            }
        }

        #endregion

        internal Thread tThread;
        internal TcpClient sock;
        internal NetworkStream stream;

        private int e, f, g, h, i, j;

        private WO.Core.Encryption.WORandom random_1;
        private WO.Core.Encryption.WORandom random_2;

        //--
        public Objects.Player player;

        public long accountID;

        public short Health;
        public short Stamina;
        public short Food;
        public short Water;

        public bool Dev;

        public void SetWater(short num)
        {
            Water = num;
            Packets.PlayerStats.SendWater(this);
        }
    }
}
