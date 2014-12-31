using System;
using System.Collections.Generic;
using WO.Core;
using WOEmu.Misc;
using System.Text;

namespace WOEmu.Objects
{
    public enum ObjectType
    {
        Item,
        Structure,
        NPC,
        Wall,

        //Static items won't be sent to players, BUT has to be in ObjectPool
        StaticItem,
    }

    public class ObjectBase
    {
        public ObjectBase()
        {
            OnFire = false;
            customLighting = false;
            emitLight = false;
            isInvisible = false;
            Scripted = false;
            Rotation = 0.0f;
        }

        public virtual void OnRemove()
        {

        }

        public long ID = 0;
        public string Name = "Unknown";
        public string wiki = "";
        public string examine = ""; //Adds examine here

        public Vector3 Position = null;

        public ObjectType Type;

        public bool Climbing;

        /// <summary>
        /// Whether this object is on fire or not.
        /// </summary>
        public bool OnFire;
        /// <summary>
        /// Whether this object has custom lighting.
        /// </summary>
        public bool customLighting;
        /// <summary>
        /// Whether this object is emitting light.
        /// </summary>
        public bool emitLight;
        /// <summary>
        /// Whether this object is invisible
        /// </summary>
        public bool isInvisible;

        /// <summary>
        /// The radius of the fire, if on fire
        /// </summary>
        public byte fireRadius;
        /// <summary>
        /// The RGB array of the lighting the object has
        /// </summary>
        public byte[] lightingRGB;
        /// <summary>
        /// The RGB array of the emitting light
        /// </summary>
        public byte[] emitRGB;
        /// <summary>
        /// Stuff for invisibility
        /// </summary>
        public byte b1, b2;
        /// <summary>
        /// Indicates whether this object is a scripted one or not.
        /// </summary>
        public bool Scripted;
        public float Rotation;
    }
}
