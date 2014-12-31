using System;
using System.Collections.Generic;
using WO.Core;
using System.Text;

namespace WOEmu.Objects
{
    public enum StructureType
    {
        HousePlan = -1,
        Wall,
        Window,
        Door
    }

    /// <summary>
    /// A node of a structure (walls, doors, etc)
    /// </summary>
    public class StructureNode
    {
        /// <summary>
        /// Used to give the node a parent
        /// </summary>
        /// <param name="p">parent structure</param>
        public StructureNode(Structure p)
        {
            parent = p;

            coloured = false;
            type = StructureType.Wall;
            locked = false;
        }

        public void SetLocked(bool locked)
        {
            if (!locked)
            {
                foreach (Client c in Program.clients)
                {
                    Packets.StructurePacket.openDoor(c, this);
                }
            }
        }

        public void SetOptions(StructureType t, Vector3 offset, string mat, bool f, bool c)
        {
            type = t;
            relativeOffset = offset;
            material = mat;
            flipped = f;
            coloured = c;
        }

        public void SetColour(System.Drawing.Color c)
        {
            colour = c;
        }

        public Structure parent;
        public StructureType type;
        public Vector3 relativeOffset;
        public string material;
        public bool flipped;
        public bool coloured;
        /// <summary>
        /// Whether this structurenode (if it is a door), is locked.
        /// </summary>
        public bool locked;

        public System.Drawing.Color colour;
    }
}
