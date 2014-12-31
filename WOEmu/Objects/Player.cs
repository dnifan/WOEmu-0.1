using System;
using System.Collections.Generic;
using System.Text;
using WO.Core;

namespace WOEmu.Objects
{
    public class Player : ActorBase
    {
        public long AccountID;

        public Player(string model, string name) : base()
        {
            this.Model = model;
            this.Name = name;
            this.isPlayer = true;
        }

        public void SetPosition(float x, float y, float z)
        {
            this.Position.X = x;
            this.Position.Y = y;
            this.Position.Z = z;
            this.Rotation = 0;
        }

    }
}
