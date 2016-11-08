using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NoiseBrushPlugin 
{
	public static class Noise
	{
		private static int extSeed;
		private static int fastSeed;

		private static float[] floats;
		private static int current;

		#region Random

			public static int[] factorial = new int[] {1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600};
			
			public static int seed
			{
				get { return extSeed; }
				set { if (value!=extSeed) {extSeed = value; Init();} }
			}
			
			public static void Init ()
			{
				fastSeed = extSeed;
			
				//permutation = new byte[256];
				//for (int i=0; i<permutation.Length; i++) permutation[i] = (byte)(fast*256);

				floats = new float[10000];
				for (int i=0; i<floats.Length; i++) floats[i] = FastRandom();
			}

			public static bool initialized { get { return floats != null; } }

			public static float FastRandom ()
			{ 
				fastSeed = 214013*fastSeed + 2531011; 
				return ((fastSeed>>16)&0x7FFF) / 32768f;
			}

			//random coordinates cannot be negative!!!! (If use see out of range exeption - they are)

			public static float Random (int x, int z)
			{
				if (floats==null) Init();
				z+=991; x+=1999;
				current = (x*x)%5451 + Mathf.Abs((z*x)%2673) + (z*z)%1873;
				return floats[current];
			}

			public static float Random (int x, int z, float tempSize, float tempDetail) { return Random(x,z); } //ruffle noise algorithm

			public static float Random (int x, int y, int z)
			{
				if (floats==null) Init();
				z+=991; y+= 591; x+=1999;
				//return floats[ ( x + permutation[ (y + permutation[z%255])%255 ] )%999 ];
				current = (x*x)%3091 + (z*x)%2551 + (z*z)%1673 + (x*y)%1101 + (y*z)%991 + (y*y)%591;
				return floats[current]; 
			}

			public static float Random (int x, int y, int z, int w) //too slow
			{
				if (floats==null) Init();
				z+=99; y+= 59; w+= 113;
				//return floats[ ( x + permutation[ (y + permutation[z%255])%255 ] )%999 ];
				current = (x*x)%1309 + (z*x)%1255 + (z*z)%1967 + (x*y)%999 + (y*z)%499 + (y*y)%1059 + (w*w)%341 + (w*x)%811 + (w*y)%1001 + (w*z)%555;
				return floats[current]; 
			}

			public static float Random (Vector3 coord, byte w) 
			{ 
				switch (w)
				{
					case 1: return Random((int)coord.y+333, (int)coord.z, (int)coord.x);
					case 2: return Random((int)coord.z, (int)coord.x+111, (int)coord.y);
					case 3: return Random((int)coord.x+123, (int)coord.z, (int)coord.y+333);
					case 4: return Random((int)coord.y, (int)coord.x+555, (int)coord.z+111);
					case 5: return Random((int)coord.z+999, (int)coord.y+777, (int)coord.x);
					case 6: return Random((int)coord.x+333, (int)coord.z, (int)coord.y+123);
					default: return Random((int)coord.x, (int)coord.y, (int)coord.z);
				}
			}

			public static float MultipleRandom (int steps)
			{
				float random = FastRandom();

				//float result = 0;
				//for (int i=1; i<=steps; i++)
				//	result += Mathf.Pow(random,i);

				return (1-Mathf.Pow(random,steps+1)) / (1-random) - 1;
			}

			public static float NextRandom () { current++; return floats[current]; }

			public static bool Conjecture (float chance) { if (chance > FastRandom()) return true; else return false; }
			public static int ConjectureToInt (float input)
			{
				int integer = (int)input;
				float remain = input - integer;
				if (Conjecture(remain)) integer++;
				return integer;
			}

			public static int GetRandomNumber ()
			{
				return 4;	//chosen by fair dice roll
							//guaranteed to be random (XKCD)
			}

		#endregion
		
		#region Noise

			public static float Perlin (int ix, int iz, float size, float detail) { return Perlin(ix,iz,size); }
			public static float Perlin (int ix, int iz, float size)
			{	
				float x = ix/(size+1); 
				float z=iz/(size+1);

				return Mathf.PerlinNoise(x,z);

				/*int x0 = (int)x; int x1 = x0+1;
				int z0 = (int)z; int z1 = z0+1;

				if (!initialized) seed = 12345;

				float d_x0z0 = (Random(x0,z0)*2-1)*(x0-x) + (NextRandom()*2-1)*(z0-z);
				float d_x1z0 = (Random(x1,z0)*2-1)*(x1-x) + (NextRandom()*2-1)*(z0-z);
				float d_x0z1 = (Random(x0,z1)*2-1)*(x0-x) + (NextRandom()*2-1)*(z1-z);
				float d_x1z1 = (Random(x1,z1)*2-1)*(x1-x) + (NextRandom()*2-1)*(z1-z);

				float deltaX = x-x0;
				float deltaZ = z-z0;
				float percentX = 3*deltaX*deltaX - 2*deltaX*deltaX*deltaX;
				float percentZ = 3*deltaZ*deltaZ - 2*deltaZ*deltaZ*deltaZ;

				float d_x0 = Mathf.Lerp(d_x0z0, d_x0z1, percentZ);
				float d_x1 = Mathf.Lerp(d_x1z0, d_x1z1, percentZ);
				return Mathf.Clamp01((Mathf.Lerp(d_x0,d_x1, percentX) + 0.8f)*0.625f);*/
			}

			public static float Fractal (int x, int z, float size, float detail=0.5f)
			{
				float result = 0.5f;
				float curSize = size;
				float curAmount = 1;
				x += 10000;
				z += 10000;

				//get number of iterations
				int numIterations = 1; //max size iteration included
				for (int i=0; i<100; i++)
				{
					curSize = curSize/2;
					if (curSize<1) break;
					numIterations++;
				}

				//applying noise
				curSize = size;
				for (int i=0; i<numIterations;i++)
				{
					float perlin = Perlin(x,z, curSize);// * (detail + curAmount*(1-detail));
					perlin = (perlin-0.5f)*curAmount + 0.5f;

					//Debug.Log(curSize);

					//applying overlay
					if (perlin > 0.5f) result = 1 - 2*(1-result)*(1-perlin); //(1 - (1-2*(perlin-0.5f)) * (1-result));
					else result = 2*perlin*result;

					curSize *= 0.5f;
					curAmount *= detail; //detail is 0.5 by default
				}

				result = Mathf.Clamp01(result);

				//if (result<0) result = -result; //inverting result to avoid negative values
				return result;
			}

		#endregion

	}//noise class
}//namespace