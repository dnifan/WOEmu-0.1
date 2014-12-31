using System;
using System.Collections.Generic;
using System.Text;

namespace WOEmu.Objects
{
    public class NPCBase : ActorBase
    {
        public NPCBase(string model, string name)
            : base()
        {
            this.Model = model;
            this.Name = name;
            this.isPlayer = false;
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
