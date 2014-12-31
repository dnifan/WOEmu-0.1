using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WO.Core
{
    /// <summary>
    /// Class to wrap RGB and Alpha values.
    /// </summary>
    public class Color
    {
        public Color(float a, float b, float c)
        {
            R = a;
            G = b;
            B = c;
            Alpha = 0.0f;
        }

        public Color(float a, float b, float c, float p)
        {
            R = a;
            G = b;
            B = c;
            Alpha = p;
        }

        public float R, G, B, Alpha;
    }
}
