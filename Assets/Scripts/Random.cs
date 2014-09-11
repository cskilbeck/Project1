using System;

namespace Util
{
	public class Random
	{
		private UInt32 x, y, z, w, v;

		public Random()
		{
			Seed((UInt32)DateTime.Now.Ticks);
		}
		
		public Random(UInt32 seed)
		{
			Seed(seed);
		}

		public void Seed(UInt32 seed)
		{
			x = 123456789;
			y = 362436069;
			z = 521288629;
			w = 88675123;
			v = seed;
		}

		public UInt32 Next()
		{
			UInt32 t = (x ^ (x >> 7));
			x = y;
			y = z;
			z = w;
			w = v;
			v = (v ^ (v << 6)) ^ (t ^ (t << 13));
			return ((y + y + 1) * v) >> 16;
		}
		
		public float NextFloat()
		{
			return Next() / (float)0xffffffff;
		}
	}
}

