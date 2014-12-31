using System;
using System.Collections.Generic;
using System.Text;
using WO.Core.Logger;

namespace WOEmu.Objects
{
    public static class ObjectPool
    {
        public static void Initialize()
        {
            Pool = new List<ObjectBase>();
        }

        public static void AddObject(ObjectBase b)
        {
            Pool.Add(b);
        }

        public static ObjectBase GetObject(long ID)
        {
            foreach (ObjectBase b in Pool)
            {
                if (b.ID == ID)
                {
                    return b;
                }
            }

            return null;
        }

        public static void RemoveObject(ObjectBase b)
        {
            Pool.Remove(b);
        }

        public static List<ObjectBase> Pool;
    }
}
