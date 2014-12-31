using System;
using System.Collections.Generic;
using System.Text;

using WOEmu.Packets;

namespace WOEmu.Objects
{
    /// <summary>
    /// Base class - inherit this to make NPCs, players, etc...
    /// </summary>
    public class ActorBase : ObjectBase 
    {
        public ActorBase() : base()
        {
            ID = IDGenerator.GetID();
            inventory = new List<InventoryItem>();
            Health = 100.0f;
            MaxHealth = 100.0f;
            isPlayer = false;
        }

        //public float Rotation;

        /// <summary>
        /// The model string of this actor
        /// </summary>
        public string Model;
        public float Speed;

        public float Health;
        public float MaxHealth;
        public Boolean isPlayer;

        public List<InventoryItem> inventory;
    }
}
