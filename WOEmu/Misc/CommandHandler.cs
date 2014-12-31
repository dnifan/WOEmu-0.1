using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;
using WOEmu.Objects;

using WOEmu.Packets;

namespace WOEmu.Misc
{
    /// <summary>
    /// This class handles all commands
    /// </summary>
    public static class CommandHandler
    {
        /// <summary>
        /// Handles a command
        /// </summary>
        /// <param name="sender">Client who issued the command</param>
        /// <param name="commandline">The line the client sent</param>
        /// <returns>Whether successful or not</returns>
        public static bool HandleCommand(Client sender, string commandline)
        {
            commandline = commandline.Substring(1);
            string[] Tokens = commandline.Split(new char[] {' '});

            if (sender.Dev == true)
            {
                switch (Tokens[0])
                {
                    #region /getPosition
                    case "getpos":
                        sender.SendChat(sender.player.Position.ToString(), ":Event");
                        return true;
                    #endregion
                    #region /setspeed
                    case "setspeed":
                        if (Tokens.Length != 2)
                        {
                            sender.SendChat("Syntax error.", ":Event");
                            return true;
                        }

                        float newspeed = float.Parse(Tokens[1]);
                        Packets.PlayerStats.SetSpeed(sender, newspeed);
                        return true;
                    #endregion
                    #region /weathergen
                    case "weathergen":
                        Packets.Weather.GenerateWeather();
                        Packets.Weather.SendTo(sender);
                        return true;
                    #endregion
                    #region /placesign
                    case "placesign":
                        if (Tokens.Length < 2)
                        {
                            sender.SendChat("Syntax error.", ":Event");
                            return true;
                        }

                        string signLine = "";
                        for (int c = 1; c < Tokens.Length; c++)
                        {
                            signLine += (" " + Tokens[c]);
                        }

                        Item i = new Item(signLine);
                        i.SetModel("model.sign.large.wooden.pinewood");
                        i.SetPos(sender.player.Position);
                        i.examine = signLine;
                        i.wiki = "sign";
                        i.templateID = 0;
                        ObjectPool.AddObject(i);

                        foreach (Client c in Program.clients)
                        {
                            GroundItem.SendTo(c, i);
                        }

                        return true;
                    #endregion
                    #region /timetest
                    case "timetest":
                        if (Tokens.Length != 2)
                            return false;

                        long sun = long.Parse(Tokens[1]);

                        Packets.Time.SendTo(sender, ((1000 * (8 * sun)) / 24));
                        return true;
                    #endregion
                    #region /recall
                    case "recall":
                        if (Tokens.Length != 2)
                        {
                            sender.SendChat("Syntax error.", ":Event");
                            return true;
                        }
                        foreach (Client rc in Program.clients)
                        {
                            if (rc.player.Name == Tokens[1])
                                continue;

                            rc.player.Position.X = sender.player.Position.X;
                            rc.player.Position.Y = sender.player.Position.Y;
                            rc.player.Position.Z = sender.player.Position.Z;
                            rc.SendChat("You have been summoned by a powerful force.", ":Event");
                            PlayerInformation.SendTo(rc, rc.player);//Fuck it, couldn't get it to update properly.
                            //Teleport.SendTo(
                        }
                        return true;
                    #endregion
                    #region /bmltest
                    case "bmltest":
                        BMLObject o = new BMLObject("Test.bml");
                        o.Caption = "Lol";
                        o.X = 300;
                        o.Y = 300;
                        o.Closable = true;
                        o.Resizable = true;
                        GUI.SendBML(sender, o);
                        return true;
                    #endregion
                    #region /soundtest
                    case "soundtest":
                        Sound.SendPlay(sender, "sound.birdsong.bird2");
                        return true;
                    #endregion
                    #region /teleport
                    case "teleport":
                    case "tele":
                        if (Tokens.Length != 4)
                        {
                            sender.SendChat("Syntax error.", ":Event");
                            return true;
                        }

                        float X = float.Parse(Tokens[1]);
                        float Y = float.Parse(Tokens[2]);
                        float Z = float.Parse(Tokens[3]);
                        Vector3 pos = new Vector3(X, Y, Z);

                        Packets.Teleport.SendTo(sender, pos, 0.0f);

                        return true;
                    #endregion
                    #region /tradeopen
                    case "tradeopen":
                        Trade.SendOpen(sender, sender.player);
                        Trade.AddItem(sender, Trade.DestinationWindow.Other_Offer, new InventoryItem("LOL HI!", 8938947, 0));
                        return true;
                    #endregion
                    #region /spawnbl
                    case "spawnbl":
                        Sprite.Add(sender, sender.player.ID, 2, new Vector3(sender.player.Position.X, sender.player.Position.Y, 0.0f));
                        return true;
                    #endregion
                    #region /spawnwl
                    case "spawnwl":
                        Sprite.Add(sender, sender.player.ID, 1, new Vector3(sender.player.Position.X, sender.player.Position.Y, 0.0f));
                        return true;
                    #endregion
                    #region /spawnflag
                    case "spawnflag":
                        Item Ci = new Item("Flag");
                        Ci.SetModel("model.woemu.flag.test");
                        Ci.SetPos(sender.player.Position);
                        Ci.template = null;
                        Ci.templateID = 0;
                        Ci.Type = ObjectType.Item;

                        GroundItem.SendTo(sender, Ci);
                        return true;
                    #endregion
                    #region /windowtest
                    case "windowtest":
                        long ID = GUI.SendOpenContainer(sender, "lollorhax");
                        GUI.SendContainerContent(sender, new InventoryItem("Test Object", IDGenerator.GetID(), 0), ID);
                        return true;
                    #endregion
                    #region /spawntest
                    case "spawntest": //spawns a NPC with color ^^
                        ActorBase ab = new ActorBase();
                        ab.Type = ObjectType.NPC;
                        ab.Position = sender.player.Position;
                        ab.Rotation = 0.0f;
                        ab.Name = "Colored NPC";
                        ab.Model = "model.creature.quadraped.cow.";
                        ab.wiki = "NPC";
                        ab.examine = "lol";
                        ab.Health = 100.0f;
                        ab.MaxHealth = 100.0f;

                        ObjectPool.AddObject(ab);

                        AddActor.SendTo(sender, ab);
                        Creature.SetColor(sender, ab, new Color(0.0f, 0.0f, 0.0f, 0.0f));

                        return true;
                    #endregion
                    default:
                        return false;
                }
            }
            else
            {
                WO.Core.Logger.Logger.printWarning("Client '" + sender.player.Name + "' used command '" + Tokens[0] + "'.");
                return true;
            }
        }
    }
}
