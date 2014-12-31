using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Objects;
using System.Text;

namespace WOEmu.Packets
{
    /// <summary>
    /// Popup menu - the menu you get when right-clicking something.
    /// </summary>
    public static class PopupMenu
    {
        /// <summary>
        /// Send a "popup menu" to someone.
        /// </summary>
        /// <param name="c">Client to send to.</param>
        /// <param name="items">List of strings</param>
        /// <param name="wikiref">The wiki Title of the object</param>
        /// <param name="request">The byte the client sent earlier.</param>
        public static void SendTo(Client c, List<Menus.__menuItem> items, string wikiref, byte request)
        {
            PacketWriter w = new PacketWriter();

            w.PushShort(0);
            w.PushByte(37);

            byte newEntry = 0;

            //
            w.PushByte(request);
            w.PushByte((byte)items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].submenu == true)
                    w.PushShort((short)(items[i].ID*-1));
                else
                    w.PushShort((short)items[i].ID);
                w.PushByte((byte)items[i].name.Length);
                w.PushBytes(Encoding.ASCII.GetBytes(items[i].name));

                if (items[i].parented == true)
                {
                    //just send the original byte..
                    //send the original, linked to the, (if exists, DUMB DEVELOPERS HERE :P), previous
                    //submenu.
                    w.PushByte(newEntry);
                }
                else
                {
                    //flip the byte, but only if it is not a submenu ^_^
                    //if (items[i].submenu == false)
                        newEntry = (byte)(newEntry == 1 ? 0 : 1);
                    w.PushByte(newEntry);
                }
            }

            w.PushByte((byte)wikiref.Length);
            w.PushBytes(Encoding.ASCII.GetBytes(wikiref));

            byte[] t = w.Finish();
            c.Encrypt(t, 0, t.Length);
            c.Send(t);
        }
    }
}
