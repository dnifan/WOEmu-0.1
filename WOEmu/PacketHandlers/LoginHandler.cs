using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Packets;
using WOEmu.Misc;
using System.Text;
using System.Security.Cryptography;

//THIS HANDLER NEEDS CLEANING!!
//E.G. : SKILLS NEED BETTER HANDLING

namespace WOEmu.PacketHandlers
{
    public static class LoginHandler
    {
        public static bool Read(Client c, byte[] Packet)
        {
            #region Packet
            PacketReader _reader = new PacketReader(Packet);

            _reader.PopByte(); //ID
            int magic = _reader.PopInt(); //should be 0xC24FE373 is C22FE3
                                          //real: 0xC2, 0x2F, 0xE3, 0x73, some version change!? :)

            byte len = _reader.PopByte();
            byte[] UserBuf = new byte[len];
            UserBuf = _reader.PopBytes(len);
            byte plen = _reader.PopByte();
            byte[] PassBuf = new byte[plen];
            PassBuf = _reader.PopBytes(plen);

            byte zero = _reader.PopByte();

            string username = Encoding.ASCII.GetString(UserBuf);
            string password = Encoding.ASCII.GetString(PassBuf);

            SHA1CryptoServiceProvider sha1Provider = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(sha1Provider.ComputeHash(PassBuf)).Replace("-", "");
            #endregion

            WO.Core.Logger.Logger.printInfo("Client '" + username + "' logging in.");

            c.player.Name = username;

            Sql query = new Sql(Program.sqlData);
            query.ExecuteQuery("SELECT ID FROM accounts WHERE User LIKE '" + c.player.Name + "'");
            query.reader.Read();
            c.player.AccountID = query.reader.GetInt64(0);

            //STUFF TO DO AFTER LOGIN...asdasdasasd
            #region POST-LOGIN
            Chat.SendTo(c, ":Event", "You will now fight defensively.");
            PlayerStats.SetFightMode(c, 2);
            PlayerStats.SetStance(c, "Playing WOEmu!");
            PlayerStats.SetSpeed(c, 0.0f);
            
            //Stub. 
        SendSkill.SendTo(c, 18L, 9223372028264841234, "Characteristics", 20.0f, 20.0f);
        SendSkill.SendTo(c, 18L, 9223372015379939346, "Religion", 20.0f, 20.0f);
        SendSkill.SendTo(c, 18L, 9223372032559808530, "Skills", 20.0f, 20.0f);
        SendSkill.SendTo(c, 18L, 9223372006790004754, "Spell effects", 20.0f, 20.0f);
        SendSkill.SendTo(c, 9223372028264841234L, 4294967314, "Body", 220.0f, 20.0f);
        SendSkill.SendTo(c, 9223372028264841234L, 8589934610, "Mind", 20.0f, 20.0f);
        SendSkill.SendTo(c, 9223372028264841234L, 12884901906, "Soul", 20.0f, 20.0f);
        SendSkill.SendTo(c, 8589934610L, 429496729618, "Mind logic", 20.0f, 20.0f);
        SendSkill.SendTo(c, 8589934610L, 433791696914, "Mind speed", 20.0f, 20.0f);
        SendSkill.SendTo(c, 4294967314L, 438086664210, "Body strength", 20.0f, 20.0f);
        SendSkill.SendTo(c, 4294967314L, 442381631506, "Body stamina", 20.0f, 20.0f);
        SendSkill.SendTo(c, 4294967314L, 446676598802, "Body control", 20.0f, 20.0f);
        SendSkill.SendTo(c, 12884901906L, 450971566098, "Soul strength", 20.0f, 20.0f);
        SendSkill.SendTo(c, 12884901906L, 455266533394, "Soul depth", 20.0f, 20.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4294967296018, "Swords", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4299262263314, "Knives", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4303557230610, "Shields", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4307852197906, "Axes", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4312147165202, "Mauls", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4316442132498, "Carpentry", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4325032067090, "WoodCutting", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4329327034386, "Mining", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4333622001682, "Digging", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4337916968978, "Firemaking", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4342211936274, "Pottery", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4346506903570, "Tailoring", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4350801870866, "Masonry", 50000.0f, 60000.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4355096838162, "Ropemaking", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4359391805458, "Smithing", 1.0f, 1.0f);
        SendSkill.SendTo(c, 4359391805458L, 4363686772754, "Weapon smithing", 1.0f, 1.0f);
        SendSkill.SendTo(c, 4359391805458L, 4367981740050, "Armour smithing", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4372276707346, "Cooking", 1.0f, 1.0f);
        SendSkill.SendTo(c, 9223372032559808530L, 4376571674642, "Nature", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4380866641938, "Miscellaneous items", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4385161609234, "Alchemy", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4389456576530, "Toys", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4393751543826, "Fighting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4398046511122, "Healing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4402341478418, "Clubs", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4406636445714, "Religion", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4415226380306, "Thievery", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4419521347602, "War machines", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 4423816314898, "Archery", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4316442132498L, 4428111282194, "Bowyery", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4316442132498L, 4432406249490, "Fletching", 1.0f, 1.0f);
            //
		SendSkill.SendTo(c, 4307852197906L, 42953967927314, "SmallAxe", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 42958262894610, "Shovel", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4307852197906L, 42962557861906, "Hatchet", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 42966852829202, "Rake", 1.0f, 1.0f);//
		SendSkill.SendTo(c, 4294967296018L, 42971147796498, "Longsword", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4303557230610L, 42975442763794, "Medium shield", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4299262263314L, 42979737731090, "Carving knife", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 42984032698386, "Saw", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 42988327665682, "Pickaxe", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4363686772754L, 42992622632978, "Blades smithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4363686772754L, 42996917600274, "Weapon heads smithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4367981740050L, 43001212567570, "Chain armour smithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4367981740050L, 43005507534866, "Plate armour smithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4367981740050L, 43009802502162, "Shield smithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4359391805458L, 43014097469458, "Blacksmithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4346506903570L, 43018392436754, "Cloth tailoring", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4346506903570L, 43022687404050, "Leatherworking", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 43026982371346, "Tracking", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4303557230610L, 43035572305938, "Medium wooden shield", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4307852197906L, 43052752175122, "Large axe", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4307852197906L, 43057047142418, "Huge axe", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 43061342109714, "Hammer", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4294967296018L, 43065637077010, "Shortsword", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4294967296018L, 43069932044306, "Two handed sword", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4299262263314L, 43074227011602, "Butchering knife", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 43078521978898, "Stone chisel", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 43082816946194, "Paving", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 43087111913490, "Prospecting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43091406880786, "Fishing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4359391805458L, 43095701848082, "Locksmithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 43099996815378, "Repairing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 43104291782674, "Coal-making", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4372276707346L, 43108586749970, "Dairy food making", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4372276707346L, 43112881717266, "Hot food cooking", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4372276707346L, 43117176684562, "Baking", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 43121471651858, "Milling", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4359391805458L, 43125766619154, "Metallurgy", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4385161609234L, 43130061586450, "Natural substances", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4359391805458L, 43134356553746, "Jewelry smithing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4316442132498L, 43138651521042, "Fine carpentry", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43142946488338, "Gardening", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 43147241455634, "Sickle", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4380866641938L, 43151536422930, "Scythe", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43155831390226, "Forestry", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43160126357522, "Farming", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4389456576530L, 43164421324818, "Yoyo", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4393751543826L, 43173011259410, "Weaponless fighting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4393751543826L, 43177306226706, "Aggressive fighting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4393751543826L, 43181601194002, "Defensive fighting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4393751543826L, 43185896161298, "Normal fighting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4398046511122L, 43190191128594, "First aid", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4393751543826L, 43194486095890, "Taunting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4393751543826L, 43198781063186, "Shield bashing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4372276707346L, 43203076030482, "Butchering", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43207370997778, "Milking", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4312147165202L, 43211665965074, "Large maul", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4312147165202L, 43215960932370, "Medium maul", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4402341478418L, 43224550866962, "Huge club", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4406636445714L, 43228845834258, "Preaching", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4406636445714L, 43233140801554, "Prayer", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4406636445714L, 43241730736146, "Exorcism", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43254615638034, "Foraging", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43258910605330, "Botanizing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 9223372032559808530L, 43263205572626, "Climbing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4350801870866L, 43267500539922, "Stone cutting", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4415226380306L, 43271795507218, "Stealing", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4415226380306L, 43276090474514, "Lock picking", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4419521347602L, 43280385441810, "Catapults", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4376571674642L, 43284680409106, "Animal taming", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4423816314898L, 43288975376402, "Short bow", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4423816314898L, 43293270343698, "Medium bow", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4423816314898L, 43297565310994, "Long bow", 1.0f, 1.0f);
		SendSkill.SendTo(c, 4316442132498L, 43301860278290, "Ship building", 1.0f, 1.0f);


            //Send MOTD to user:
            BMLObject motd = new BMLObject("Motd.bml");
            motd.Caption = "Message of the day";
            motd.X = 300;
            motd.Y = 300;

            motd.Closable = true;
            motd.Resizable = false;
            GUI.SendBML(c, motd);            

            //Get stuff from database!
            #region Database
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("SELECT * FROM accounts WHERE User = '" + username + "';");
            if (!s.reader.HasRows)
            {
                WO.Core.Logger.Logger.printWarning("USER DOES NOT EXIST!");
                return false;
            }
            while (s.reader.Read())
            {
                c.accountID = s.reader.GetInt64("ID");

                c.Health = s.reader.GetInt16("health");
                c.Stamina = s.reader.GetInt16("stamina");
                c.Food = s.reader.GetInt16("food");
                c.Water = s.reader.GetInt16("water");
                c.Dev = (s.reader.GetInt16("Dev") == 1 ? true : false);

                c.player.Position.X = s.reader.GetFloat("posX");
                if (c.player.Position.X == 0)
                    c.player.Position.X = float.Parse(Program.configFile.GetValue("spawnX"));
                c.player.Position.Y = s.reader.GetFloat("posY");
                if (c.player.Position.Y == 0)
                    c.player.Position.Y = float.Parse(Program.configFile.GetValue("spawnY"));
                c.player.Position.Z = s.reader.GetFloat("posZ");
                if (c.player.Position.Z == 0)
                    c.player.Position.Z = float.Parse(Program.configFile.GetValue("spawnZ"));
            }
            s.Dispose();
            #endregion

            WOEmu.Packets.PlayerInformation.SendTo(c, c.player);
            #endregion

            return true;
        }
    }
}
