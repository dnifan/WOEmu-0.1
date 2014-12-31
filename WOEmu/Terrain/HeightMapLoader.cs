using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Xml;
using System.Xml.Schema;

namespace WOEmu.Terrain
{
    public static class HeightMapLoader
    {
        public static Terrain Load(string file, short xoffset, short yoffset)
        {
            Image i = Image.FromFile(file);
            Bitmap asd = (Bitmap)i;
            Bitmap m = asd;
            
            Terrain t = new Terrain((short)m.Width, (short)m.Height, xoffset, yoffset);

            int mx = m.Width;
            int my = m.Height;

            foreach (WOEmu.Misc.Tile tile in t.Tiles)
            {
                Color c = m.GetPixel((int)tile.X, (int)tile.Y);

                c = Color.FromArgb((c.R + c.G + c.B) / 3, (c.R + c.G + c.B) / 3, (c.R + c.G + c.B) / 3);
                int val = int.Parse(c.R.ToString());

                tile.Height = (short)(val-20);
            }

            return t;
        }
    }
}
