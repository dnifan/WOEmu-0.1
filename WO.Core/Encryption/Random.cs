using System;
using System.Collections.Generic;
using System.Text;

namespace WO.Core.Encryption
{
    public class WORandom
    {
        /// <summary>
        /// Initializer for Random function
        /// </summary>
        /// <param name="seed">The seed</param>
        public WORandom(long seed)
        {
            setSeed(seed);
        }

        /// <summary>
        /// Function for resetting the seed for this class
        /// </summary>
        /// <param name="seed">The seed</param>
        public void setSeed(long seed)
        {
            this.seed = (seed ^ 0x5DEECE66DL) & ((1L << 48) - 1);
            this.haveNextGaussian = false;
        }

        /// <summary>
        /// Returns next value
        /// </summary>
        /// <param name="bits">Amount of bits</param>
        /// <returns></returns>
        public int next(int bits)
        {
            this.seed = (seed * 0x5DEECE66DL + 0xBL) & ((1L << 48) - 1);
            return (int)(seed >> (48 - bits));
        }

        public void nextBytes(byte[] bytes)
        {

        }

        /// <summary>
        /// Get next integer
        /// </summary>
        /// <returns>Integer</returns>
        public int nextInt()
        {
            return next(32);
        }

        /// <summary>
        /// blabla
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int nextInt(int n)
        {
            if (n <= 0)
                throw new Exception("n must be positive");

            if ((n & -n) == n)
                return (int)((n * (long)next(31)) >> 31);

            int bits, val;
            do
            {
                bits = next(31);
                val = bits % n;
            } while (bits - val + (n - 1) < 0);
            return val;
        }

        public long nextLong()
        {
            return ((long)next(32) << 32) + next(32);
        }

        public bool nextBoolean()
        {
            return (next(1) != 0);
        }

        public float nextFloat()
        {
            return (next(24) / (float)(1 << 24));
        }

        public double nextDouble()
        {
            return (((long)next(26) << 27) + next(27)) / (double)(1L << 53);
        }

        public double nextGaussian()
        {
            if (haveNextGaussian)
            {
                haveNextGaussian = false;
                return nextNextGaussian;
            }
            else
            {
                double v1, v2, s;
                do
                {
                    v1 = 2 * nextDouble() - 1;
                    v2 = 2 * nextDouble() - 1;
                    s = v1 * v1 + v2 * v2;
                } while (s >= 1 || s == 0);
                double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);
                nextNextGaussian = v2 * multiplier;
                haveNextGaussian = true;
                return v1 * multiplier;
            }
        }

        internal long seed;
        internal bool haveNextGaussian;
        internal double nextNextGaussian;
    }
}
