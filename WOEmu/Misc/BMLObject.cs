using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WOEmu.Misc
{
    public class BMLObject
    {
        public BMLObject()
        {
            R = 163.0f;
            G = 195.0f;
            B = 186.0f;
        }

        /// <summary>
        /// Overloaded constructor - will load BML from a file.
        /// </summary>
        /// <param name="filename">The relative file path</param>
        public BMLObject(string filename)
        {
            if (File.Exists(filename))
            {
                FileStream fs = File.Open(filename, FileMode.Open);
                if (fs != null)
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);

                    WO.Core.Logger.Logger.printDebug("BML File is loaded: " + fs.Length + " bytes.");

                    Body = Encoding.ASCII.GetString(buffer);
                }
            }

            R = 163.0f;
            G = 195.0f;
            B = 186.0f;
        }

        public string Caption;

        public string Body;

        public float R;
        public float G;
        public float B;

        public short X;
        public short Y;

        public bool Closable;
        public bool Resizable;
    }
}
