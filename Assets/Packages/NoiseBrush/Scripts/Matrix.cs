using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NoiseBrushPlugin 
{
	[System.Serializable]
	public struct Coord
	{
		public int x;
		public int z;

		public Coord (int x, int z) { this.x=x; this.z=z; }

		public static bool operator > (Coord c1, Coord c2) { return c1.x>c2.x && c1.z>c2.z; }
		public static bool operator < (Coord c1, Coord c2) { return c1.x<c2.x && c1.z<c2.z; }
		public static bool operator == (Coord c1, Coord c2) { return c1.x==c2.x && c1.z==c2.z; }
		public static bool operator != (Coord c1, Coord c2) { return c1.x!=c2.x && c1.z!=c2.z; }
		public static Coord operator + (Coord c, int s) { return  new Coord(c.x+s, c.z+s); }
		public static Coord operator + (Coord c1, Coord c2) { return  new Coord(c1.x+c2.x, c1.z+c2.z); }
		public static Coord operator - (Coord c, int s) { return  new Coord(c.x-s, c.z-s); }
		public static Coord operator - (Coord c1, Coord c2) { return  new Coord(c1.x-c2.x, c1.z-c2.z); }
		public static Coord operator * (Coord c, int s) { return  new Coord(c.x*s, c.z*s); }
		//public static Coord operator * (Coord c, float s) { return  new Coord(Mathf.RoundToInt(c.x*s), Mathf.RoundToInt(c.z*s)); }
		public static Coord operator / (Coord c, int s) { return  new Coord(c.x/s, c.z/s); }

		public override bool Equals(object obj) { return base.Equals(obj); }
		public override int GetHashCode() {return x*10000000 + z;}

		public int Minimal {get{ return Mathf.Min(x,z); } }
		public int SqrMagnitude {get{ return x*x + z*z; } }

		/*public void Divide (float val, bool ceil=false) 
		{ 
			if (ceil) { x = Mathf.FloorToInt(x/val); z = Mathf.FloorToInt(z/val); }
			else { x = Mathf.CeilToInt(x/val); z = Mathf.CeilToInt(z/val); }
		}*/

		public void Round (int val, bool ceil=false) //make a coord divisible by val
		{ 
			x = (ceil ? Mathf.CeilToInt(1f*x/val) : Mathf.FloorToInt(1f*x/val)) * val;
			z = (ceil ? Mathf.CeilToInt(1f*z/val) : Mathf.FloorToInt(1f*z/val)) * val;
		}
		public void Round (Coord c, bool ceil=false)
		{ 
			x = (ceil ? Mathf.FloorToInt(1f*x/c.x) : Mathf.CeilToInt(1f*x/c.x)) * c.x;
			z = (ceil ? Mathf.FloorToInt(1f*z/c.z) : Mathf.CeilToInt(1f*z/c.z)) * c.z;
		}

		static public Coord Min (Coord c1, Coord c2) { return new Coord(Mathf.Min(c1.x,c2.x), Mathf.Min(c1.z,c2.z)); }
		static public Coord Max (Coord c1, Coord c2) { return new Coord(Mathf.Max(c1.x,c2.x), Mathf.Max(c1.z,c2.z)); }

		public override string ToString()
		{
			return (base.ToString() + " x:" + x + " z:" + z);
		}
	}
	
	[System.Serializable]
	public struct CoordRect
	{
		public Coord offset;
		public Coord size;

		//public int radius; //not related with size, because a clamped CoordRect should have non-changed radius

		public CoordRect (Coord offset, Coord size) { this.offset = offset; this.size = size; }
		public CoordRect (int offsetX, int offsetZ, int sizeX, int sizeZ) { this.offset = new Coord(offsetX,offsetZ); this.size = new Coord(sizeX,sizeZ);  }
		public CoordRect (float offsetX, float offsetZ, float sizeX, float sizeZ) { this.offset = new Coord((int)offsetX,(int)offsetZ); this.size = new Coord((int)sizeX,(int)sizeZ);  }
		public CoordRect (Rect r) { offset = new Coord((int)r.x, (int)r.y); size = new Coord((int)r.width, (int)r.height); }

		public Coord Max { get { return offset+size; } set { offset = value-size; } }
		public Coord Min { get { return offset; } set { offset = value; } }

		public static bool operator > (CoordRect c1, CoordRect c2) { return c1.size>c2.size; }
		public static bool operator < (CoordRect c1, CoordRect c2) { return c1.size<c2.size; }
		public static bool operator == (CoordRect c1, CoordRect c2) { return c1.offset==c2.offset && c1.size==c2.size; }
		public static bool operator != (CoordRect c1, CoordRect c2) { return c1.offset!=c2.offset || c1.size!=c2.size; }
		public static CoordRect operator * (CoordRect c, int s) { return  new CoordRect(c.offset*s, c.size*s); }
		//public static CoordRect operator * (CoordRect c, float s) { return  new CoordRect(c.offset*s, c.size*s); }
		public static CoordRect operator / (CoordRect c, int s) { return  new CoordRect(c.offset/s, c.size/s); }

		public override bool Equals(object obj) { return base.Equals(obj); }
		public override int GetHashCode() {return offset.x*100000000 + offset.z*1000000 + size.x*1000+size.z;}

		//public float SqrDistFromCenter (Coord c) { return (Center-c).SqrMagnitude; }
		//public bool IsInRadius (Coord c) { return SqrDistFromCenter(c) < radius*radius; }
		//public void CalcRadius () { radius = size.Minimal/2; }
		public void Round (int val, bool inscribed=false) { offset.Round(val, ceil:inscribed); size.Round(val, ceil:!inscribed); } //inscribed parameter will shrink rect to make it lay inside original rect
		public void Round (CoordRect r, bool inscribed=false) { offset.Round(r.offset, ceil:inscribed); size.Round(r.size, ceil:!inscribed); }
		//public void Divide (int val, bool inscribed=false) { offset.Round(val, ceil:inscribed); size.Round(val, ceil:!inscribed); } //inscribed parameter will shrink rect to make it lay inside original rect

		public void Clamp (Coord min, Coord max)
		{
			Coord oldMax = Max;
			offset = Coord.Max(min, offset);
			size = Coord.Min(max-offset, oldMax-offset);
			
		}

		public static CoordRect Intersect (CoordRect c1, CoordRect c2) { c1.Clamp(c2.Min, c2.Max); return c1; }

		public int this[int x, int z] { get { return (z-offset.z)*size.x + x - offset.x; } } //gets the pos of the coordinate
		public int this[Coord c] { get { return (c.z-offset.z)*size.x + c.x - offset.x; } }

		public bool CheckInRange (int x, int z)
		{
			return (x- offset.x >= 0 && x- offset.x < size.x &&
			        z- offset.z >= 0 && z- offset.z < size.z);
		}

		public bool Divisible (float factor) { return offset.x%factor==0 && offset.z%factor==0 && size.x%factor==0 && size.z%factor==0; }

		public override string ToString()
		{
			return (base.ToString() + ": offsetX:" + offset.x + " offsetZ:" + offset.z + " sizeX:" + size.x + " sizeZ:" + size.z);
		}
	}

	[System.Serializable]
	public class Matrix
	{
		public float[] array;
		public CoordRect rect;
		public int pos;

		#region Creation

			public Matrix (CoordRect rect)
			{
				this.rect = rect;
				array = new float[rect.size.x*rect.size.z];
			}

			public Matrix (Coord offset, Coord size)
			{
				this.rect = new CoordRect(offset, size);
				array = new float[rect.size.x*rect.size.z];
			}

			public void ChangeRect (CoordRect newRect) //will re-create array only if it is needed
			{
				rect.offset.x = newRect.offset.x; 
				rect.offset.z = newRect.offset.z;
				if (rect.size.x != newRect.size.x || rect.size.z != newRect.size.z)
				{
					rect = newRect;
					array = new float[rect.size.x*rect.size.z];
				}
			}

			public Matrix Clone (Matrix result=null)
			{
				if (result==null) result = new Matrix(rect);
			
				//copy params
				result.rect = rect;
				result.pos = pos;
			
				//copy array
				//result.array = (float[])array.Clone(); //no need to create it any time
				if (result.array.Length != array.Length) result.array = new float[array.Length];
				for (int i=0; i<array.Length; i++)
					result.array[i] = array[i];

				return result;
			}

		#endregion
		
		public float this[int x, int z] 
		{
			get { return array[(z-rect.offset.z)*rect.size.x + x - rect.offset.x]; } //rect fn duplicated to increase performance
			set { array[(z-rect.offset.z)*rect.size.x + x - rect.offset.x] = value; }
		}

		public float this[Coord c] 
		{
			get { return array[(c.z-rect.offset.z)*rect.size.x + c.x - rect.offset.x]; }
			set { array[(c.z-rect.offset.z)*rect.size.x + c.x - rect.offset.x] = value; }
		}

		public float this[Vector3 pos]
		{
			get { return array[((int)pos.z-rect.offset.z)*rect.size.x + (int)pos.x - rect.offset.x]; }
			set { array[((int)pos.z-rect.offset.z)*rect.size.x + (int)pos.x - rect.offset.x] = value; }
		}




		#region Quick Pos

			public void SetPos(int x, int z) { pos = (z-rect.offset.z)*rect.size.x + x - rect.offset.x; }
			public void SetPos(int x, int z, int s) { pos = (z-rect.offset.z)*rect.size.x + x - rect.offset.x  +  s*rect.size.x*rect.size.z; }

			public void MoveX() { pos++; }
			public void MoveZ() { pos += rect.size.x; }
			public void MovePrevX() { pos--; }
			public void MovePrevZ() { pos -= rect.size.x; }

			//public float current { get { return array[pos]; } set { array[pos] = value; } }
			public float nextX { get { return array[pos+1]; } set { array[pos+1] = value; } }
			public float prevX { get { return array[pos-1]; } set { array[pos-1] = value; } }
			public float nextZ { get { return array[pos+rect.size.x]; } set { array[pos+rect.size.x] = value; } }
			public float prevZ { get { return array[pos-rect.size.x]; } set { array[pos-rect.size.x] = value; } }
			public float nextXnextZ { get { return array[pos+rect.size.x+1]; } set { array[pos+rect.size.x+1] = value; } }
			public float prevXnextZ { get { return array[pos+rect.size.x-1]; } set { array[pos+rect.size.x-1] = value; } }
			public float nextXprevZ { get { return array[pos-rect.size.x+1]; } set { array[pos-rect.size.x+1] = value; } }
			public float prevXprevZ { get { return array[pos-rect.size.x-1]; } set { array[pos-rect.size.x-1] = value; } }

		#endregion

		#region Conversion

			public float[,] ReadHeighmap (TerrainData data, float height=1)
			{
				CoordRect intersection = CoordRect.Intersect(rect, new CoordRect(0,0,data.heightmapResolution, data.heightmapResolution));
				
				//get heights
				float[,] array = data.GetHeights(intersection.offset.x, intersection.offset.z, intersection.size.x, intersection.size.z); //returns x and z swapped

				//reading 2d array
				Coord min = intersection.Min; Coord max = intersection.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
						this[x,z] = array[z-min.z, x-min.x] * height;

				//removing borders
				RemoveBorders(intersection);

				return array;
			}

			public void WriteHeightmap (TerrainData data, float[,] array=null, float brushFallof=0.5f)
			{
				CoordRect intersection = CoordRect.Intersect(rect, new CoordRect(0,0,data.heightmapResolution, data.heightmapResolution));
				
				//checking ref array
				if (array == null || array.Length != intersection.size.x*intersection.size.z) array = new float[intersection.size.z,intersection.size.x]; //x and z swapped

				//write to 2d array
				Coord min = intersection.Min; Coord max = intersection.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
				{
					float fallofFactor = Fallof(x,z,brushFallof);
					if (Mathf.Approximately(fallofFactor,0)) continue;
					array[z-min.z, x-min.x] = this[x,z]*fallofFactor + array[z-min.z, x-min.x]*(1-fallofFactor);
					//array[z-min.z, x-min.x] += this[x,z];
				}

				data.SetHeights(intersection.offset.x, intersection.offset.z, array);
			}

			public float[,,] ReadSplatmap (TerrainData data, int channel, float[,,] array=null)
			{
				CoordRect intersection = CoordRect.Intersect(rect, new CoordRect(0,0,data.alphamapResolution, data.alphamapResolution));
				
				//get heights
				if (array==null) array = data.GetAlphamaps(intersection.offset.x, intersection.offset.z, intersection.size.x, intersection.size.z); //returns x and z swapped

				//reading array
				Coord min = intersection.Min; Coord max = intersection.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
						this[x,z] = array[z-min.z, x-min.x, channel];

				//removing borders
				RemoveBorders(intersection);

				return array;
			}

			static public void AddSplatmaps (TerrainData data, Matrix[] matrices, int[] channels, float[] opacity, float[,,] array=null, float brushFallof=0.5f)
			{
				int numChannels = data.alphamapLayers;
				bool[] usedChannels = new bool[numChannels];
				for (int i=0; i<channels.Length; i++) usedChannels[channels[i]] = true;
				float[] slice = new float[numChannels];

				Coord dataSize = new Coord(data.alphamapResolution, data.alphamapResolution);
				CoordRect dataRect = new CoordRect(new Coord(0,0), dataSize);
				CoordRect intersection = CoordRect.Intersect(dataRect, matrices[0].rect);
				
				if (array==null) array = data.GetAlphamaps(intersection.offset.x, intersection.offset.z, intersection.size.x, intersection.size.z);

				Coord min = intersection.Min; Coord max = intersection.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
				{
					//calculating fallof and opacity
					float fallofFactor = matrices[0].Fallof(x,z,brushFallof);
					if (Mathf.Approximately(fallofFactor,0)) continue;

					//reading slice
					for (int c=0; c<numChannels; c++) slice[c] = array[z-min.z, x-min.x, c];

					//converting matrices to additive
					for (int i=0; i<matrices.Length; i++) matrices[i][x,z] = Mathf.Max(0, matrices[i][x,z] - slice[channels[i]]);

					//apply fallof
					for (int i=0; i<matrices.Length; i++) matrices[i][x,z] *= fallofFactor * opacity[i];

					//calculating sum of adding values
					float addedSum = 0; //the sum of adding channels
					for (int i=0; i<matrices.Length; i++) addedSum += matrices[i][x,z];
					//if (addedSum < 0.00001f) continue; //no need to do anything

					//if addedsum exceeds 1 - equalizing matrices
					if (addedSum > 1f) 
						{ for (int i=0; i<matrices.Length; i++) matrices[i][x,z] /= addedSum; addedSum=1; }

					//multiplying all values on a remaining amount
					float multiplier = 1-addedSum;
					for (int c=0; c<numChannels; c++) slice[c] *= multiplier;

					//adding matrices
					for (int i=0; i<matrices.Length; i++) slice[channels[i]] += matrices[i][x,z];

					//saving slice
					for (int c=0; c<numChannels; c++) array[z-min.z, x-min.x, c] = slice[c];
				}

				data.SetAlphamaps(intersection.offset.x, intersection.offset.z, array);
			}

			public void ToTexture (Texture2D texture=null, Color[] colors=null, float rangeMin=0, float rangeMax=1, bool resizeTexture=false)
			{
				//creating or resizing texture
				if (texture == null) texture = new Texture2D(rect.size.x, rect.size.z);
				if (resizeTexture) texture.Resize(rect.size.x, rect.size.z);
				
				//finding matrix-texture intersection
				Coord textureSize = new Coord(texture.width, texture.height);
				CoordRect textureRect = new CoordRect(new Coord(0,0), textureSize);
				CoordRect intersection = CoordRect.Intersect(textureRect, rect);
				
				//checking ref color array
				if (colors == null || colors.Length != intersection.size.x*intersection.size.z) colors = new Color[intersection.size.x*intersection.size.z];

				//filling texture
				Coord min = intersection.Min; Coord max = intersection.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
				{
					float val = this[x,z];

					//adjusting value to range
					val -= rangeMin;
					val /= rangeMax-rangeMin;

					//making color gradient
					float byteVal = val * 256;
					int flooredByteVal = (int)byteVal;
					float remainder = byteVal - flooredByteVal;

					float flooredVal = flooredByteVal/256f;
					float ceiledVal = (flooredByteVal+1)/256f;
					
					//saving to colors
					int tx = x-min.x; int tz = z-min.z;
					colors[tz*(max.x-min.x) + tx] = new Color(flooredVal, remainder>0.333f ? ceiledVal : flooredVal, remainder>0.666f ? ceiledVal : flooredVal);
				}
			
				texture.SetPixels(intersection.offset.x, intersection.offset.z, intersection.size.x, intersection.size.z, colors);
				texture.Apply();
			}

			public void FromTexture (Texture2D texture, bool fillBorders=false)
			{
				Coord textureSize = new Coord(texture.width, texture.height);
				CoordRect textureRect = new CoordRect(new Coord(0,0), textureSize);
				CoordRect intersection = CoordRect.Intersect(textureRect, rect);

				Color[] colors = texture.GetPixels(intersection.offset.x, intersection.offset.z, intersection.size.x, intersection.size.z);

				Coord min = intersection.Min; Coord max = intersection.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
				{
					int tx = x-min.x; int tz = z-min.z;
					Color col = colors[tz*(max.x-min.x) + tx];

					this[x,z] = (col.r+col.g+col.b)/3;
				}

				if (fillBorders) RemoveBorders(intersection);
			}

			public Texture2D SimpleToTexture (Texture2D texture=null, Color[] colors=null, float rangeMin=0, float rangeMax=1, string savePath=null)
			{
				if (texture == null) texture = new Texture2D(rect.size.x, rect.size.z);
				if (texture.width != rect.size.x || texture.height != rect.size.z) texture.Resize(rect.size.x, rect.size.z);
				if (colors == null || colors.Length != rect.size.x*rect.size.z) colors = new Color[rect.size.x*rect.size.z];

				for (int i=0; i<array.Length; i++) 
				{
					float val = array[i];
					val -= rangeMin;
					val /= rangeMax-rangeMin;
					colors[i] = new Color(val, val, val);
				}
			
				texture.SetPixels(colors);
				texture.Apply();
				return texture;
			}

			public void SimpleFromTexture (Texture2D texture)
			{
				ChangeRect(new CoordRect(rect.offset.x, rect.offset.z, texture.width, texture.height));
				
				Color[] colors = texture.GetPixels();

				for (int i=0; i<array.Length; i++) 
				{
					Color col = colors[i];
					array[i] = (col.r+col.g+col.b)/3;
				}
			}

		#endregion

		#region Borders

			public void RemoveBorders ()
			{
				Coord min = rect.Min; Coord last = rect.Max - 1;
			
				for (int x=min.x; x<=last.x; x++)
					{ SetPos(x,min.z); array[pos] = nextZ; }

				for (int x=min.x; x<=last.x; x++)
					{ SetPos(x,last.z); array[pos] = prevZ; }

				for (int z=min.z; z<=last.z; z++)
					{ SetPos(min.x,z); array[pos] = nextX; }

				for (int z=min.z; z<=last.z; z++)
					{ SetPos(last.x,z); array[pos] = prevX; }
			}

			public void RemoveBorders (int borderMinX, int borderMinZ, int borderMaxX, int borderMaxZ)
			{
				Coord min = rect.Min; Coord max = rect.Max;
			
				if (borderMinZ != 0)
				for (int x=min.x; x<max.x; x++)
				{
					float val = this[x, min.z+borderMinZ];
					for (int z=min.z; z<min.z+borderMinZ; z++) this[x,z] = val;
				}

				if (borderMaxZ != 0)
				for (int x=min.x; x<max.x; x++)
				{
					float val = this[x, max.z-borderMaxZ];
					for (int z=max.z-borderMaxZ; z<max.z; z++) this[x,z] = val;
				}

				if (borderMinX != 0)
				for (int z=min.z; z<max.z; z++)
				{
					float val = this[min.x+borderMinX, z];
					for (int x=min.x; x<min.x+borderMinX; x++) this[x,z] = val;
				}
				
				if (borderMaxX != 0)
				for (int z=min.z; z<max.z; z++)
				{
					float val = this[max.x-borderMaxX, z];
					for (int x=max.x-borderMaxX; x<max.x; x++) this[x,z] = val;
				}
			}

			public void RemoveBorders (CoordRect centerRect)
			{ 
				RemoveBorders(
					Mathf.Max(0,centerRect.offset.x-rect.offset.x), 
					Mathf.Max(0,centerRect.offset.z-rect.offset.z), 
					Mathf.Max(0,rect.Max.x-centerRect.Max.x+1), 
					Mathf.Max(0,rect.Max.z-centerRect.Max.z+1) ); 
			}

		#endregion

		#region Resize

			public Matrix Resize (CoordRect newRect, float smoothness=1, Matrix result=null)
			{
				//calculating ratio
				int upscaleRatio = newRect.size.x / rect.size.x;
				int downscaleRatio = rect.size.x / newRect.size.x;

				//checking if rect could be rescaled
				if (upscaleRatio > 1 && !newRect.Divisible(upscaleRatio)) Debug.LogError("Matrix rect " + rect + " could not be upscaled to " + newRect + " with factor " + upscaleRatio);
				if (downscaleRatio > 1 && !rect.Divisible(downscaleRatio)) Debug.LogError("Matrix rect " + rect + " could not be downscaled to " + newRect + " with factor " + downscaleRatio);

				//scaling
				if (upscaleRatio > 1) result = Upscale(upscaleRatio, result:result);
				if (downscaleRatio > 1) result = Downscale(downscaleRatio, smoothness:smoothness, result:result);

				//returning clone if all ratios are 1
				if (upscaleRatio <= 1 && downscaleRatio <= 1) return Clone(result);
				else return result;
			}
			
			public Matrix Upscale (int factor, Matrix result=null) //scaling both size AND offset
			{
				//preparing resulting array
				if (result == null) result = new Matrix(rect*factor);
				result.ChangeRect(rect*factor);

				//returning clone if ratio is 1
				if (factor == 1) return Clone(result);

				//resizing
				Coord min = rect.Min; Coord last = rect.Max-1;
				float step = 1f/factor;

				for (int x=min.x; x<last.x; x++)
					for (int z=min.z; z<last.z; z++)
				{
					float current = this[x,z];
					float nextX = this[x+1,z];
					float nextZ = this[x,z+1];
					float nextXZ = this[x+1,z+1];

					for (int ix=0; ix<factor; ix++)
						for (int iz=0; iz<factor; iz++)
					{
						float percentX = ix*step;
						float percentZ = iz*step;

						//percentX = 3*percentX*percentX - 2*percentX*percentX*percentX;
						//percentZ = 3*percentZ*percentZ - 2*percentZ*percentZ*percentZ;

						float firstRow = Mathf.Lerp(current, nextZ, percentZ);
						float lastRow = Mathf.Lerp(nextX, nextXZ, percentZ);
						result[x*factor + ix, z*factor + iz] = Mathf.Lerp(firstRow, lastRow, percentX);
					}
				}

				//removing borders
				result.RemoveBorders(0,0,factor+1,factor+1);

				return result;
			}

			public Matrix Downscale (int factor=2, float smoothness=1, Matrix result=null)
			{
				//preparing resulting array
				if (!rect.Divisible(factor)) Debug.LogError("Matrix rect " + rect + " could not be downscaled with factor " + factor);
				if (result == null) result = new Matrix(rect/factor);
				result.ChangeRect(rect/factor);

				//returning clone if ratio is 1
				if (factor == 1) return Clone(result);
			
				//work coords
				Coord min = rect.Min; //Coord max = rect.Max;
				Coord rmin = result.rect.Min; Coord rmax = result.rect.Max;

				//scaling nearest neightbour
				if (smoothness < 0.0001f)
				for (int x=rmin.x; x<rmax.x; x++)
					for (int z=rmin.z; z<rmax.z; z++)
				{
					int sx = (x-rmin.x)*factor + min.x;
					int sz = (z-rmin.z)*factor + min.z;

					result[x,z] = this[sx, sz];
				}

				//scaling bilinear
				else
				for (int x=rmin.x; x<rmax.x; x++)
					for (int z=rmin.z; z<rmax.z; z++)
				{
					int sx = (x-rmin.x)*factor + min.x;
					int sz = (z-rmin.z)*factor + min.z;

					float sum = 0;
					for (int ix=sx; ix<sx+factor; ix++)
						for (int iz=sz; iz<sz+factor; iz++)
							sum += this[ix,iz];

					result[x,z] = sum/(factor*factor)*smoothness + this[sx, sz]*(1-smoothness);
				}

				return result;
			}

		#endregion

		#region Procedural resize

			public class Stacker
			{
				public CoordRect smallRect;
				public CoordRect bigRect;

				public bool preserveDetail = true;
				
				Matrix downscaled;
				Matrix upscaled;
				Matrix difference;

				bool isDownscaled;

				public Stacker (CoordRect smallRect, CoordRect bigRect)
				{
					this.smallRect = smallRect; this.bigRect = bigRect;
					isDownscaled = false;

					//do not create additional matrices if rect sizes are the same
					if (bigRect==smallRect)
					{
						upscaled = downscaled = new Matrix(bigRect);
					}

					else
					{
						downscaled = new Matrix(smallRect);
						upscaled = new Matrix(bigRect);
						difference = new Matrix(bigRect);
						//once arrays created they should not be resized
					}
				}

				public Matrix matrix
				{
					get { if (isDownscaled) return downscaled; else return upscaled; }
					//set { if (isDownscaled) downscaled=value; else upscaled=value; }
				}

				public void ToSmall ()
				{
					if (bigRect==smallRect) return;
					
					//calculating factor
					//int downscaleRatio = newSize.x / rect.size.x;

					//scaling
					downscaled = upscaled.Resize(smallRect, result:downscaled);

					//difference
					if (preserveDetail)
					{
						difference = downscaled.Resize(bigRect, result:difference);
						difference.Blur();
						difference.InvSubtract(upscaled); //difference = original - difference
					}

					isDownscaled = true;
				}

				public void ToBig ()
				{
					if (bigRect==smallRect) return;
					
					upscaled = downscaled.Resize(bigRect, result:upscaled);
					upscaled.Blur();
					if (preserveDetail) upscaled.Add(difference);

					isDownscaled = false;
				}

			}


		#endregion

		#region Blur

			public void Cavity (float strength)
			{
				System.Func<float,float,float,float> cavityFn = delegate(float prev, float curr, float next) 
				{
					float c = curr - (next+prev)/2;
					return (c*c*(c>0?1:-1))*strength*1000;
				};

				Matrix horizontalCavity = new Matrix(this.rect);
				horizontalCavity.Blur(cavityFn, intensity:1, reference:this, horizontal:true, vertical:false);

				Matrix verticalCavity = new Matrix(this.rect);
				verticalCavity.Blur(cavityFn, intensity:1, reference:this, horizontal:false, vertical:true);

				//copy vertical and horizonatal to this
				for (int i=0; i<array.Length; i++) this.array[i] = horizontalCavity.array[i] + verticalCavity.array[i];
			}

			public void OverBlur (int iterations=20)
			{
				Matrix blurred = this.Clone();

				for (int i=1; i<=iterations; i++)
				{
					if (i==1 || i==2) blurred.Blur(step:1);
					else if (i==3) { blurred.Blur(step:1); blurred.Blur(step:1); }
					else blurred.Blur(step:i-2); //i:4, step:2

					for (int p=0; p<array.Length; p++) 
					{
						float b = blurred.array[p] * i;
						float a = array[p];

						array[p] = a + b + a*b;
					}
				}
			}

			public void Blur (System.Func<float,float,float,float> blurFn=null, //prev, curr, next = output
				float intensity=0.666f, int step=1, Matrix reference=null, bool horizontal=true, bool vertical=true)
			{
				Coord min = rect.Min; Coord max = rect.Max;
				
				if (reference==null) reference = this;
				int lastX = max.x-1;
				int lastZ = max.z-1;

				if (horizontal)
				for (int z=min.z; z<=lastZ; z++)
				{
					float next = reference[min.x,z];
					float curr = next;
					float prev = next;

					float blurred = next;
					float lastBlurred = next;
					
					for (int x=min.x+step; x<=lastX; x+=step) 
					{
						//blurring
						if (blurFn==null) blurred = (prev+next)/2f;
						else blurred = blurFn(prev, curr, next);
						blurred = curr*(1-intensity) + blurred*intensity;
						
						//shifting values
						prev = curr; //this[x,z];
						curr = next; //this[x+step,z];
						try { next = reference[x+step*2,z]; } //this[x+step*2,z];
						catch { next = reference[lastX,z]; }

						//filling between-steps distance
						if (step==1) this[x,z] = blurred;
						else for (int i=0; i<step; i++) 
						{
							float percent = 1f * i / step;
							this[x-step+i,z] = blurred*percent + lastBlurred*(1-percent);
						}
						lastBlurred = blurred;
					}
				}

				if (vertical)
				for (int x=min.x; x<=lastX; x++)
				{
					float next = reference[x,min.z];
					float curr = next;
					float prev = next;

					float blurred = next;
					float lastBlurred = next;
					
					for (int z=min.z+step; z<=lastZ; z+=step) 
					{
						//blurring
						if (blurFn==null) blurred = (prev+next)/2f;
						else blurred = blurFn(prev, curr, next);
						blurred = curr*(1-intensity) + blurred*intensity;
						
						//shifting values
						prev = curr;
						curr = next;
						try { next = reference[x,z+step*2]; }
						catch { next = reference[x,lastZ]; }

						//filling between-steps distance
						if (step==1) this[x,z] = blurred;
						else for (int i=0; i<step; i++) 
						{
							float percent = 1f * i / step;
							this[x,z-step+i] = blurred*percent + lastBlurred*(1-percent);
						}
						lastBlurred = blurred;
					}
				}
			}

		#endregion

		#region Other

			public float GetOnTerrain (Vector2 worldPos, Terrain terrain)
			{
				float relativeX = (worldPos.x - terrain.transform.position.x) / terrain.terrainData.size.x;
				float relativeZ = (worldPos.y - terrain.transform.position.z) / terrain.terrainData.size.z;
				int posX = Mathf.RoundToInt( relativeX*rect.size.x + rect.offset.x );
				int posZ = Mathf.RoundToInt( relativeZ*rect.size.z + rect.offset.z );
				posX = Mathf.Clamp(posX,rect.Min.x+1,rect.Max.x-1); posZ = Mathf.Clamp(posZ,rect.Min.z+1,rect.Max.z-1);

				return this[posX,posZ];
			}

			static public void BlendLayers (Matrix[] matrices, float[] opacity) //changes splatmaps in photoshop layered style so their summary value does not exceed 1
			{
				Coord min = matrices[0].rect.Min; Coord max = matrices[0].rect.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
				{
					float sum = 0;
					for (int i=matrices.Length-1; i>=0; i--) //layer 0 is background
					{
						float val = matrices[i][x,z];
						float overly = Mathf.Clamp01(sum + val - 1);
						matrices[i][x,z] = val - overly;
						sum += val - overly;
					}
				}
			}

			static public void NormalizeLayers (Matrix[] matrices, float[] opacity) //changes splatmaps in photoshop layered style so their summary value does not exceed 1
			{
				Coord min = matrices[0].rect.Min; Coord max = matrices[0].rect.Max;
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
				{
					float sum = 0;
					for (int i=0; i<matrices.Length; i++) sum += matrices[i][x,z];
					if (sum > 1f) for (int i=0; i<matrices.Length; i++) matrices[i][x,z] /= sum;
				}
			}

			public float Fallof (int x, int z, float fallof) //returns the relative dist from circle (with radius = size/2 * fallof) located at the center
			{
				if (fallof < 0) return 1;
				
				//relative distance from center
				float radiusX = rect.size.x/2f-1; float relativeX = (x - (rect.offset.x+radiusX)) / radiusX; // (x - center) / radius
				float radiusZ = rect.size.z/2f-1; float relativeZ = (z - (rect.offset.z+radiusZ)) / radiusZ;
				float dist = Mathf.Sqrt(relativeX*relativeX + relativeZ*relativeZ);

				//percent
				float percent = Mathf.Clamp01( (1-dist) / (1-fallof) );
				return 3*percent*percent - 2*percent*percent*percent;

				//advanced control over percent
				//float pinPercent = percent*percent; //*percent;
				//float bubblePercent = 1-(1-percent)*(1-percent) ; //*(1-percent);
				//if (percent > 0.5f) percent = bubblePercent*2 - 1f; //bubblePercent*4 - 3f;
				//else percent = pinPercent*2; //pinPercent*4;
				//return percent;
			}

		#endregion

		#region Arithmetic

			public void Add (Matrix add) { for (int i=0; i<array.Length; i++) array[i] += add.array[i]; }
			public void Add (float add) { for (int i=0; i<array.Length; i++) array[i] += add; }
			public void Subtract (Matrix m) { for (int i=0; i<array.Length; i++) array[i] -= m.array[i]; }
			public void InvSubtract (Matrix m) { for (int i=0; i<array.Length; i++) array[i] = m.array[i] - array[i]; }
			public void ClampSubtract (Matrix m) { for (int i=0; i<array.Length; i++) array[i] = Mathf.Clamp01(array[i] - m.array[i]); } //useful for subtracting layers
			public void Multiply (Matrix m) { for (int i=0; i<array.Length; i++) array[i] *= m.array[i]; }
			public void Multiply (float m) { for (int i=0; i<array.Length; i++) array[i] *= m; }
			public bool CheckRange (float min, float max) { for (int i=0; i<array.Length; i++) if (array[i]<min || array[i]>max) return false; return true; } 
			public void Clear () { for (int i=0; i<array.Length; i++) array[i] = 0; }

		#endregion
	}
}//namespace