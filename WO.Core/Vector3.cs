using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WO.Core
{
    /// <summary>
    /// Class to wrap a vector.
    /// </summary>
    public class Vector3
    {
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";            
        }

        public void Set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        //Operators:

        public static Vector3 operator +(Vector3 v, Vector3 v2)
        {
            v.X += v2.X;
            v.Y += v2.Y;
            v.Z += v2.Z;
            return v;
        }

        public static Vector3 operator -(Vector3 v, Vector3 v2)
        {
            v.X -= v2.X;
            v.Y -= v2.Y;
            v.Z -= v2.Z;
            return v;
        }

        public static Vector3 operator *(Vector3 v, Vector3 v2)
        {
            v.X *= v2.X;
            v.Y *= v2.Y;
            v.Z *= v2.Z;
            return v;
        }

        public static Vector3 operator /(Vector3 v, Vector3 v2)
        {
            v.X /= v2.X;
            v.Y /= v2.Y;
            v.Z /= v2.Z;
            return v;
        }

        public float X;
        public float Y;
        public float Z;
    }
}
