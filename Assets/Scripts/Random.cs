using System;

namespace Util
{
	public class Random
	{
		private UInt32 Z;
		private UInt32 W;

		public Random()
		{
			Int64 t = DateTime.Now.Ticks;
			Z = (UInt32)t;
			W = (UInt32)(t >> 32);
		}

		public Random(UInt32 seed)
		{
			Seed(seed);
		}

		public void Seed(UInt32 seed)
		{
			Z = seed;
			W = ~seed;
		}

		public UInt32 Next()
		{
			Z = 36969 * (Z & 65535) + (Z >> 16);
			W = 18000 * (W & 65535) + (W >> 16);
			return (Z << 16) + W;
		}
		
		public float NextFloat()
		{
			return Next() / (float)0xffffffff;
		}
	}
}

