using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

//using Plugins;

namespace NoiseBrushPlugin
{
	[System.Serializable]
	public class Preset
	{
		//splat preset
		[System.Serializable]
		public struct SplatPreset
		{
			public bool apply;
			public float opacity;
			public int num;
		}

		//main brush params
		public float brushSize = 10;
		public float brushFallof = 0.6f;
		public float brushSpacing = 0.15f;
		public int downscale = 1;
		public float blur = 0.1f;

		public bool isErosion;
		public bool isNoise { get{return !isErosion;} set{isErosion=!value;} }

		//noise brush
		public int noise_seed = 12345;
		public float noise_amount = 20f;
		public float noise_size = 200f;
		public float noise_detail = 0.55f;
		public float noise_uplift = 0.6f;
		public float noise_ruffle = 1f;

		//erosion brush
		public int erosion_iterations = 3;
		public float erosion_durability = 0.9f;
		public int erosion_fluidityIterations = 3;
		public float erosion_amount = 1f; //quantity of erosion made by iteration. Lower values require more iterations, but will give better results
		public float sediment_amount = 0.8f; //quantity of sediment that was raised by erosion will drop back to land. Lower values will give eroded canyons with washed-out land, but can produce artefacts
		public float wind_amount = 0.75f;
		public float erosion_smooth = 0.15f;

		//painting
		public SplatPreset foreground = new SplatPreset() { opacity=1 };
		public SplatPreset background = new SplatPreset() { opacity=1 };
		public bool paintSplat
		{get{
			return  (foreground.apply && foreground.opacity>0.01f) ||
					(background.apply && background.opacity>0.01f);
		}}
		
		//save-load
		public string name;
		public bool saveBrushSize;
		public bool saveBrushParams;
		public bool saveErosionNoiseParams;
		public bool saveSplatParams;
		public Preset Copy() { return (Preset) this.MemberwiseClone(); }
	}
	
	[ExecuteInEditMode]
	public class NoiseBrush : MonoBehaviour 
	{
		private Terrain _terrain;
		public Terrain terrain { get{ if (_terrain==null) _terrain=GetComponent<Terrain>(); return _terrain; } set {_terrain=value;} }

		public Preset preset = new Preset(); 
		public Preset[] presets = new Preset[0];
		public int guiSelectedPreset = 0;

		public bool paint = false;
		public bool wasPaint = false;
		public bool moveDown = false;

		public Transform moveTfm;
		public bool gen;

		public bool undo; 

		[System.NonSerialized] public Texture2D guiHydraulicIcon;
		[System.NonSerialized] public Texture2D guiWindIcon;
		[System.NonSerialized] public Texture2D guiPluginIcon;
		public int guiApplyIterations = 1;
		public int[] guiChannels;
		public string[] guiChannelNames;
		public Color guiBrushColor = new Color(1f,0.7f,0.3f);
		public float guiBrushThickness = 4;
		public int guiBrushNumCorners = 32;
		public bool recordUndo = true;
		public bool unity5positioning = false;
		public bool focusOnBrush = true;
		public bool preserveDetail = true;
	
		public bool guiShowPreset = true;
		public bool guiShowBrush = true;
		public bool guiShowGenerator = true;
		public bool guiShowTextures = true;
		public bool guiShowGlobal = false;
		public bool guiShowSettings = false;
		public bool guiShowAbout = false;
		public int guiMaxBrushSize = 100;
		public bool guiSelectPresetsUsingNumkeys = true;

		[System.NonSerialized] Matrix.Stacker heights;
		[System.NonSerialized] Matrix.Stacker splats;
		[System.NonSerialized] Matrix.Stacker sediments;


		public void ApplyBrush (Rect worldRect, bool useFallof=true, bool newUndo=false)
		{
			TerrainData data = terrain.terrainData;

			//preparing useful values
			bool paintSplat = preset.paintSplat;
			if (data.alphamapLayers==0) paintSplat = false;

			//finding minimum resolution
			int smallerRes = Mathf.Min(data.heightmapResolution-1, data.alphamapResolution);
			int largerRes = Mathf.Max(data.heightmapResolution-1, data.alphamapResolution);
			int downscaledRes = largerRes / preset.downscale;
			int minRes = Mathf.Min(smallerRes, downscaledRes);

			//scale factors (relative to min res)
			int heightFactor = (data.heightmapResolution-1) / minRes;
			int splatFactor = data.alphamapResolution / minRes;
			int downscaledFactor = downscaledRes / minRes;

			//creating rects
			CoordRect minRect = new CoordRect(worldRect.x*minRes, worldRect.y*minRes, worldRect.width*minRes, worldRect.height*minRes);
			CoordRect heightsRect = minRect * heightFactor;
			CoordRect splatsRect = minRect * splatFactor;
			CoordRect downscaledRect = minRect * downscaledFactor;

			//checking stackers
			if (heights==null || heights.smallRect!=downscaledRect || heights.bigRect!=heightsRect) 
				heights = new Matrix.Stacker(downscaledRect, heightsRect);

			if (splats==null || splats.smallRect!=downscaledRect || splats.bigRect != splatsRect) 
				splats = new Matrix.Stacker(downscaledRect, splatsRect);

			if (sediments==null || sediments.smallRect != downscaledRect || sediments.bigRect != splatsRect) 
				sediments = new Matrix.Stacker(downscaledRect, splatsRect);

			heights.preserveDetail=preserveDetail; splats.preserveDetail=preserveDetail; sediments.preserveDetail = preserveDetail;

			//creating original arrays
			heights.matrix.ChangeRect(heightsRect);
			float[,] heights2d = heights.matrix.ReadHeighmap(data);

			splats.matrix.ChangeRect(splatsRect); sediments.matrix.ChangeRect(splatsRect);
			float[,,] splats3d = null;
			if (paintSplat)
			{
				splats3d = splats.matrix.ReadSplatmap(data, preset.foreground.num);
				sediments.matrix.ReadSplatmap(data, preset.background.num, splats3d);
			}

			//downscaling arrays
			heights.ToSmall(); 
			if (paintSplat) { splats.ToSmall(); sediments.ToSmall(); }

			//generating
			if (!preset.isErosion)
			{
				Matrix heightsMatrix = heights.matrix; Matrix splatsMatrix = splats.matrix; Matrix sedimentsMatrix = sediments.matrix;

				Coord min = heightsMatrix.rect.Min; Coord max = heightsMatrix.rect.Max; 
				for (int x=min.x; x<max.x; x++)
					for (int z=min.z; z<max.z; z++)
				{
					float noise = Noise.Fractal(x, z, preset.noise_size);
					//noise = 1f*(x-min.x)/(max.x-min.x);
					noise = (noise-(1-preset.noise_uplift)) * preset.noise_amount;
					heightsMatrix[x,z] += noise / data.size.y;

					if (paintSplat)
					{
						float splatNoise = Mathf.Max(0,noise);
						splatsMatrix[x,z] = Mathf.Sqrt(splatNoise)*0.3f;
					
						float sedimentNoise = Mathf.Max(0,-noise);
						sedimentsMatrix[x,z] = Mathf.Sqrt(sedimentNoise)*0.3f;
					}

					//test
					//splatsMatrix[x,z] += 0.5f; //1f * (x-min.x) / (max.x-min.x);
					//sediments.matrix[x,z] += 0.5f;// 1f * (z-min.z) / (max.z-min.z);
				}
			}
			else
			{
				//ErosionBrushPlugin.Erosion.ErosionIteration (heights.matrix.array, paintSplat? splats.matrix.array:null, paintSplat? sediments.matrix.array:null, 
				//	heights.matrix.rect.size.x, heights.matrix.rect.size.z, 
				//	erosionDurability:preset.erosion_durability, erosionAmount:preset.erosion_amount, sedimentAmount:preset.sediment_amount, erosionFluidityIterations:preset.erosion_fluidityIterations);
			
				//blurring heights
				//heights.matrix.Blur(intensity:preset.erosion_smooth);

				//increasing splat
				//splats.matrix.Multiply(1.3f);
				//sediments.matrix.Multiply(1.3f); 
			}

			//upscaling arrays (+blur)
			heights.ToBig(); splats.ToBig(); sediments.ToBig();
			
			//record undo. Undo.RecordObject and SetDirty are done in editor
			if (recordUndo) 
			{
				if (newUndo)
				{
					if (undoList.Count > 10) undoList.RemoveAt(0);
					undoList.Add(new List<UndoStep>());
				}
				if (undoList.Count == 0) undoList.Add(new List<UndoStep>());
				undoList[undoList.Count-1].Add( new UndoStep(heights2d, splats3d, heightsRect.offset.x, heightsRect.offset.z, splatsRect.offset.x, splatsRect.offset.z) );
			}

			//apply
			heights.matrix.WriteHeightmap(data, heights2d, (useFallof ? preset.brushFallof : -1));

			if (paintSplat) Matrix.AddSplatmaps(data, 
				new Matrix[] {splats.matrix, sediments.matrix}, 
				new int[] {preset.foreground.num, preset.background.num}, 
				new float[] {preset.foreground.apply? preset.foreground.opacity:0, preset.background.apply? preset.background.opacity:0},
				brushFallof:(useFallof ? preset.brushFallof : -1), 
				array:splats3d); //note that splat and sediments are additive
		}


		public struct UndoStep
		{
			float[,] heights;
			int heightsOffsetX; int heightsOffsetZ;
			float[,,] splats;
			int splatsOffsetX; int splatsOffsetZ;

			public UndoStep (float[,] heights, float[,,] splats, int heightsOffsetX, int heightsOffsetZ, int splatsOffsetX, int splatsOffsetZ)
			{
				this.heightsOffsetX = heightsOffsetX; this.heightsOffsetZ = heightsOffsetZ;
				this.splatsOffsetX = splatsOffsetX; this.splatsOffsetZ = splatsOffsetZ;
				this.heights = heights.Clone() as float[,]; 
				if (splats!=null) this.splats = splats.Clone() as float[,,];
				else this.splats = null;
			}

			public void Perform (TerrainData data)
			{
				data.SetHeights(heightsOffsetX,heightsOffsetZ,heights);
				if (splats!=null) data.SetAlphamaps(splatsOffsetX,splatsOffsetZ,splats);
			}
		}
		public List< List<UndoStep> > undoList = new List< List<UndoStep> >();	
		public bool allowUndo;

	}
}//namespace



