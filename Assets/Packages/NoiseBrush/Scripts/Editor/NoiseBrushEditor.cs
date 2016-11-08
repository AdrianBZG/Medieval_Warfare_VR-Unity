using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

//using Plugins;

namespace NoiseBrushPlugin
{
[CustomEditor(typeof(NoiseBrush))]
public class ErosionBrushEditor : Layout
{
	private System.Type terrainType;
	private object terrainInstance;
	private PropertyInfo toolProp;
	
	public NoiseBrush script; //public: save preset window uses it
	Preset preset;

	private Vector2 oldMousePos = new Vector2(0,0); //checks dist before DrawBrush
	private Vector3 oldBrushPos = new Vector3(0,0,0); //checks in Edit to perform brush spacing

	public bool test = false;

	public int unity5terrainRefreshCounter = 0;

	GUIStyle aboutFoldoutStyle;
	GUIStyle linkStyle;

	GUIContent[] presetContents = new GUIContent[0];

	int presetSelectedFromKeyboard = -1;


	public void OnEnable ()
	{
		if (script==null) script = (NoiseBrush) target;
		if (script==null || !script.enabled) return;
			
		UnityEditor.EditorApplication.update -= EditorUpdate;
		UnityEditor.EditorApplication.update += EditorUpdate;
	}
	
	public void OnDisable ()
	{
		if (script==null) script = (NoiseBrush)target;
		if (script==null) return;
		UnityEditor.EditorApplication.update -= EditorUpdate;
		if (script.moveTfm!=null) DestroyImmediate(script.moveTfm.gameObject);
	}

	#region Inspector

	public override void OnInspectorGUI () 
	{
		script = (NoiseBrush) target;
		preset = script.preset;
		if (script.terrain==null || script.terrain.terrainData==null) return;
		
		GetInspectorField();
		margin = 0;
		rightMargin = 0;

		//evaluation version
		//bool paintDisabled = false;
		//if (script.terrain.terrainData.heightmapResolution-1 > 512 ||
		//	script.terrain.terrainData.alphamapResolution > 512)
		//{
		//	Par(62); 
		//	EditorGUI.HelpBox(Inset(), "Evaluation version.\nTerrain maximum resolution is limited to 512 pixels.\n", MessageType.Warning);
		//	Par(20);
		//	lastPos.y -= 24;
		//	Inset(0.2f);
		//	if (GUI.Button(Inset(0.6f), "Switch resolution to 512"))
		//		if (EditorUtility.DisplayDialog("Warning", "Changing resolution will remove all terrain data. " +
		//			"This operation is not undoable. Please make a backup copy of your terrain data (not scene, but terrain .asset file).", "Switch", "Cancel"))
		//				{ script.terrain.terrainData.alphamapResolution = 512; script.terrain.terrainData.heightmapResolution = 513; }
		//	Inset(0.2f);
		//	lastPos.y += 24;
		//	lastHeight = 1;
		//	paintDisabled = true;
		//}

		//drawing toolbar
		if (script.guiHydraulicIcon==null) script.guiHydraulicIcon = Resources.Load("ErosionBrushHydraulic") as Texture2D;
		if (script.guiWindIcon==null) script.guiWindIcon = Resources.Load("ErosionBrushNoise") as Texture2D;

		Par(5);
		Par(22);

		//paint button
		Button(ref script.paint, "Paint", toggle:true, width:0.35f, tooltip:"A checkbutton that turns erosion or noise painting on/off. When painting is on it is terrain editing with standard Unity tools is not possible, so terrain component is disabled when “Paint” is checked. To enable terrain editing turn off paint mode.");

		//mode selector
		Inset(0.05f);
		if (GUI.Toolbar(Inset(0.6f), preset.isErosion ? 0 : 1, new GUIContent[] { 
			new GUIContent(" Erosion", script.guiHydraulicIcon, ""),
			new GUIContent(" Noise", script.guiWindIcon, "") }) == 0)
				preset.isErosion = true;
		else preset.isNoise = true;
		
		margin += 7;


		#region Preset
		Par(5); Par(); Foldout(ref script.guiShowPreset, "Preset");
		if (script.guiShowPreset)
		{
			//calculating a need to re-create preset array
			bool reCreate = false;
			if (presetContents.Length != script.presets.Length) reCreate = true;
			else 
				for (int i=0; i<presetContents.Length; i++) 
					if (presetContents[i].text != script.presets[i].name) { reCreate=true; break; }

			//re-creating presets contents
			if (reCreate)
			{
				presetContents = new GUIContent[script.presets.Length];
				for (int i=0; i<presetContents.Length; i++) 
				{
					string postfix = "";
					//if (i==0) postfix = " (1)";
					if (i<8) postfix = " (key " + (i+3) + ")";
					presetContents[i] = new GUIContent(script.presets[i].name + postfix);
				}
			}
			
			//selecting preset
			margin += 10;
			Par();
			int tempSelectedPreset = EditorGUI.Popup(Inset(), script.guiSelectedPreset, presetContents);
			if (presetSelectedFromKeyboard >= 0) { tempSelectedPreset = presetSelectedFromKeyboard; presetSelectedFromKeyboard = -1; }
			if (tempSelectedPreset != script.guiSelectedPreset && tempSelectedPreset < script.presets.Length)
			{
				LoadPreset(tempSelectedPreset);
				script.guiSelectedPreset = tempSelectedPreset;
			}

			//save, add, remove
			Par(); disabled = script.presets.Length==0;
			if (Button("Save", tooltip:"Save current preset changes", width:0.3333f) &&
				EditorUtility.DisplayDialog("Overwrite Preset", "Overwrite currently selected preset?", "Save", "Cancel") )
					SavePreset(script.guiSelectedPreset);
			
			disabled = false;
			if (Button("Save As...", tooltip:"Save current settings as new preset", width:0.3333f))
			{
				SavePresetWindow window = new SavePresetWindow();
				window.titleContent = new GUIContent("Save Erosion Brush Preset");
				window.position = new Rect(window.position.x, window.position.y, window.windowSize.x, window.windowSize.y);
				window.main = this;
				window.ShowUtility();
			}

			disabled =script.presets.Length==0;
			if (Button("Remove", tooltip:"Remove currently selected preset", width:0.3333f) &&
				EditorUtility.DisplayDialog("Remove Preset", "Are you sure you wish to remove currently selected preset?", "Remove", "Cancel"))
					RemovePreset(script.guiSelectedPreset);
			disabled = false;

			//DrawLabel(script.preset.name + " " + script.guiSelectedPreset.ToString() + "/" + script.presets.Length.ToString());

			margin -= 10;
		}
		#endregion

		#region brush settings
		Par(5); Par(); Foldout(ref script.guiShowBrush, "Brush Settings");
		if (script.guiShowBrush)
		{
			margin += 10;
			
			Quick<float>(ref preset.brushSize, "Brush Size", min:1, max:script.guiMaxBrushSize, tooltip:"Size of the brush in Unity units. Bigger brush size gives better terrain quality, but too big values can slow painting. Brush size is displayed as brighter circle in scene view. Brush could be resized with [ and ] keys.",  quadratic:true);
			Quick<float>(ref preset.brushFallof, "Brush Falloff", min:0.01f, max:0.99f, tooltip:"Decrease of brush opacity from center to rim. This parameter is specified in percent of the brush size. It is displayed as dark blue circle in scene view. Brush inside of the circle has the full opacity, and gradually decreases toward the bright circle.");
			Quick<float>(ref preset.brushSpacing, "Brush Spacing", min:0, max:1, tooltip:"When pressing and holding mouse button brush goes on making stamps. Script will not place brush at the same position where old brush was placed, but in a little distance. This parameter specifies how far from old brush stamp will be placed new one (while mouse is still pressed). It  is specified in percent of the brush size.");
			Quick<int>(ref preset.downscale, "Downscale", min:1, max:4, tooltip:"To perform quick operation on heightmaps of large size brush resolution could be scaled down. This will give less detail, but faster stamp.", quadratic:true);
			preset.downscale = Mathf.ClosestPowerOfTwo(preset.downscale);
			//Quick<float>(ref preset.blur, "Blur", min:0, max:1, tooltip:"The amount brush stamp should be blurred before apply. This parameter is very useful together with the donscale: faceted downscaled data could be blurred to give smooth result");
			
			margin -= 10;
		}
		#endregion

		#region generator settings
		Par(5); Par(); Foldout(ref script.guiShowGenerator, preset.isErosion ? "Erosion Parameters" : "Noise Parameters");
		if (script.guiShowGenerator)
		{
			margin += 10;

			if (preset.isErosion)
			{
				Par(30); Label("Noise Brush is a free version \nof Erosion Brush plugin."); 
				Par(45); Label("To generate both erosion and \nnoise with the same tool \nconsider using Erosion Brush");
				Par(5);

				Par(); Url("https://www.assetstore.unity3d.com/en/#!/content/27389", "Asset Store link");
				Par(); Url("https://www.youtube.com/watch?v=bU88tkrBbb0", "Video");
				Par(); Url("http://www.denispahunov.ru/ErosionBrush/eval.html", "Evaluation Version");
			}

			else
			{
				int tempSeed = preset.noise_seed;
				Quick<int>(ref tempSeed, "Seed", "Number to initialize random generator. With the same brush size, noise size and seed the noise value will be constant for each heightmap coordinate.", slider:false);
				if (preset.noise_seed != tempSeed) { Noise.seed = tempSeed; preset.noise_seed = tempSeed; UnityEngine.Random.seed = tempSeed; }
				
				Quick<float>(ref preset.noise_amount, "Amount", tooltip:"Magnitude. How much noise affects the surface", quadratic:true, max:100f);
				Quick<float>(ref preset.noise_size, "Size", tooltip:"Wavelength. Sets the size of the highest iteration of fractal noise. High values will create more irregular noise. This parameter represents the percentage of brush size.", max:1000, quadratic:true);
				Quick<float>(ref preset.noise_detail, "Detail", "Defines the bias of each fractal. Low values sets low influence of low-sized fractals and high influence of high fractals. Low values will give smooth terrain, high values - detailed and even too noisy.", max:1);
				Quick<float>(ref preset.noise_uplift, "Uplift", "When value is 0, noise is subtracted from terrain. When value is 1, noise is added to terrain. Value of 0.5 will mainly remain terrain on the same level, lifting or lowering individual areas.", max:1);
				//Quick<float>(ref preset.noise_ruffle, "Ruffle", "Adds additional shallow (1-unit) noise to the resulting heightmap", max:2);
			}

			margin -= 10;
		}
		#endregion

		#region texture settings
		Par(5); Par(); Foldout(ref script.guiShowTextures, "Textures");
		if (script.guiShowTextures)
		{
			margin += 10;

			SplatPrototype[] splats = script.terrain.terrainData.splatPrototypes;
			Texture2D[] textures = new Texture2D[splats.Length];
			for (int i=0; i<splats.Length; i++) textures[i] = splats[i].texture;

			Par();
			Field<bool>(ref preset.foreground.apply, width:20);
			Label("Crag", width:70);
			Slider<float>(ref preset.foreground.opacity, width:width-130, max:2);
			Field<float>(ref preset.foreground.opacity, width:40);
			Par(42); TextureSelector(ref preset.foreground.num, textures);

			Par(5); Par();
			Field<bool>(ref preset.background.apply, width:20);
			Label("Sediment", width:70);
			Slider<float>(ref preset.background.opacity, width:width-130, max:2);
			Field<float>(ref preset.background.opacity, width:40);
			Par(42); TextureSelector(ref preset.background.num, textures);

			margin -= 10;
		}
		#endregion

		#region apply to whole terrain
		Par(5); Par(); Foldout(ref script.guiShowGlobal, "Global Brush", "Apply Erosion Brush to whole terrain at once");
		if (script.guiShowGlobal)
		{
			margin += 10; Par();
			if (Button("Apply to Whole Terrain"))
			{
				//TerrainData data = script.terrain.terrainData;
				script.ApplyBrush(new Rect(0,0,1,1),useFallof:false);
				if (script.recordUndo) 
				{ 
					UnityEditor.Undo.RecordObject(script,"Erosion Brush Global"); 
					script.undo = !script.undo; UnityEditor.EditorUtility.SetDirty(this); //setting object change
				}

			}
			Quick<int>(ref script.guiApplyIterations, "Iterations", max:20);
			margin -= 10;
		}
		#endregion

		#region settings
		Par(5); Par(); Foldout(ref script.guiShowSettings, "Settings");
		if (script.guiShowSettings)
		{
			margin += 10;
			Quick<Color>(ref script.guiBrushColor, "Brush Color", "Visual representation of the brush.");
			Quick<float>(ref script.guiBrushThickness, "Brush Thickness", "Visual representation of the brush.", slider:false);
			Quick<int>(ref script.guiBrushNumCorners, "Brush Num Corners", "Visual representation of the brush.", slider:false);
			//Quick<bool>(ref script.unity5positioning, "Fix Unity5 Brush Positioning", "Unity5 Beta has incorrect terrain brush positioning (Both in Erosion Brush and Standard Terrain sculpting). Turn toggle on to fix it. WARNING: This fix is a crutch that bypasses known Unity's bug, turning it on causes some lag.");
			Quick<bool>(ref script.recordUndo, "Record Undo", "Disabling can increase performance a bit, but will make undo unavailable");
			Quick<bool>(ref script.focusOnBrush, "G Focuses on Brush", "Analog of F button, but it will focus camera not on the whole terrain, but on current brush position.");
			Quick<bool>(ref script.preserveDetail, "Preserve Detail on Downscale", "All the terrain detail edited with Downscale parameter will be returned on upscale");
			Quick<int>(ref script.guiMaxBrushSize, "Max Brush Size", "Brush size slider maximum. Note that increasing brush size will reduce performance in the quadratic dependence.", slider:false);
			if (script.guiMaxBrushSize > 100) { Par(40); EditorGUI.HelpBox(Inset(), "Increasing brush size will reduce performance in the quadratic dependence.", MessageType.Warning); }
			margin -= 10;
		}
		#endregion

		#region about
		Par(5); Par(); Foldout(ref script.guiShowAbout, "About");
		if (script.guiShowAbout)
		{
			
			Par(50+2);
			if (script.guiPluginIcon==null) script.guiPluginIcon = Resources.Load("ErosionBrushIcon") as Texture2D;
			EditorGUI.DrawPreviewTexture(Inset(50+2), script.guiPluginIcon); 
			cursor.y -= 50; cursor.y -= 7;
			
			margin = 70; 
			
			Par(); Label("Noise Brush v1.3");
			Par(); Label("by Denis Pahunov");
			Par(5);
			Par(); Label("Useful Links:");
			Par(); Url("http://www.denispahunov.ru/ErosionBrush/doc.html", " - Online Documentation");
			Par(); Url("http://www.youtube.com/watch?v=bU88tkrBbb0", " - Video Tutorial");
			Par(); Url("http://forum.unity3d.com/threads/erosion-brush-a-tool-to-paint-terrain-with-noise-and-erosion.290257/", " - Forum Thread", "Question and answers");
			Par(); Url("https://www.facebook.com/ErosionBrush", " - Facebook", "News, anounces, contests");

			Par(); Label("On any issues related with plugin");
			Par(); Label("functioning you can contact the");
			Par(); Label("author by mail:");
			Par(); Url("mailto:mail@denispahunov.ru", "mail@denispahunov.ru"); 

			//margin = 10;
			//Par(1); lastRect.y -= 208; lastRect.height = 50;
			//if (script.guiPluginIcon==null) script.guiPluginIcon = Resources.Load("ErosionBrushIcon") as Texture2D;
			//EditorGUI.DrawPreviewTexture(Inset(50), script.guiPluginIcon);
		}
		#endregion

		SetInspectorField();
	}


	public void SavePreset (int num, string name="", bool saveBrushSize=true, bool saveBrushParams=true, bool saveErosionNoiseParams=true, bool saveSplatParams=true)
	{
		Preset presetCopy = script.preset.Copy();

		if (num<0 || num>=script.presets.Length)
		{
			//setting save params for a new preset
			presetCopy = preset.Copy();
			presetCopy.name = name;
			presetCopy.saveBrushSize = saveBrushSize;
			presetCopy.saveBrushParams = saveBrushParams;
			presetCopy.saveErosionNoiseParams = saveErosionNoiseParams;
			presetCopy.saveSplatParams = saveSplatParams;

			//extending array if num is negative
			Array.Resize(ref script.presets, script.presets.Length+1);
			num = script.presets.Length - 1;
		}

		script.presets[num] = presetCopy;

		LoadPreset(num); //loading name, save params. And just to make sure preset was saved.
	}

	public void LoadPreset (int num)
	{
		if (num < 0 || num > script.presets.Length-1) return;
		
		Preset preset = script.presets[num];
		script.guiSelectedPreset = num;

		script.preset.name = preset.name;
		script.preset.saveBrushSize = preset.saveBrushSize;
		script.preset.saveBrushParams = preset.saveBrushParams;
		script.preset.saveErosionNoiseParams = preset.saveErosionNoiseParams;
		script.preset.saveSplatParams = preset.saveSplatParams;

		if (preset.saveBrushSize) script.preset.brushSize = preset.brushSize;

		if (preset.saveBrushParams)
		{
			script.preset.brushFallof = preset.brushFallof;
			script.preset.brushSpacing = preset.brushSpacing;
			script.preset.downscale = preset.downscale;
			script.preset.blur = preset.blur;
		}

		if (preset.saveErosionNoiseParams)
		{
			script.preset.isErosion = preset.isErosion;
			
			script.preset.noise_seed = preset.noise_seed;
			script.preset.noise_amount = preset.noise_amount;
			script.preset.noise_size = preset.noise_size;
			script.preset.noise_detail = preset.noise_detail;
			script.preset.noise_uplift = preset.noise_uplift;
			script.preset.noise_ruffle = preset.noise_ruffle;

			script.preset.erosion_iterations = preset.erosion_iterations;
			script.preset.erosion_durability = preset.erosion_durability;
			script.preset.erosion_fluidityIterations = preset.erosion_fluidityIterations;
			script.preset.erosion_amount = preset.erosion_amount;
			script.preset.sediment_amount = preset.sediment_amount;
			script.preset.wind_amount = preset.wind_amount;
			script.preset.erosion_smooth = preset.erosion_smooth;
		}

		if (preset.saveSplatParams)
		{
			script.preset.foreground = preset.foreground;
			script.preset.background = preset.background;
		}

		this.Repaint();
	}

	public void RemovePreset (int num) 
	{ 
		ArrayRemoveAt<Preset>(ref script.presets, num); 

		script.guiSelectedPreset = Mathf.Clamp(script.guiSelectedPreset, 0, script.presets.Length-1);
		LoadPreset(script.guiSelectedPreset);
	}
	
	#endregion //Inspector region


	#region Scene

	public void OnSceneGUI ()
	{
		script = (NoiseBrush) target;
		preset = script.preset;
		
		if (!script.paint || (Event.current.mousePosition-oldMousePos).sqrMagnitude<1f) return;

		TerrainData data = script.terrain.terrainData;

		//reading keyboard
		if (Event.current.type == EventType.keyDown)
		{
			//selecting presets with keycode
			if (script.guiSelectPresetsUsingNumkeys) 
			{
				int key = -1;
				switch (Event.current.keyCode)
				{
					//case KeyCode.Alpha1: key = 0; break;
					//case KeyCode.Alpha2: key = 2; break;
					case KeyCode.Alpha3: key = 0; break;
					case KeyCode.Alpha4: key = 1; break;
					case KeyCode.Alpha5: key = 2; break;
					case KeyCode.Alpha6: key = 3; break;
					case KeyCode.Alpha7: key = 4; break;
					case KeyCode.Alpha8: key = 5; break;
					case KeyCode.Alpha9: key = 6; break;
				}
				if (key >= 0 && key < script.presets.Length) { LoadPreset(key); script.guiSelectedPreset=key; } 
			}

			//extending brush size with keykode
			if (Event.current.keyCode == KeyCode.LeftBracket || Event.current.keyCode == KeyCode.RightBracket)
			{
				float step = (script.preset.brushSize / 10);
				step = Mathf.RoundToInt(step);
				step = Mathf.Max(1,step);

				if (Event.current.keyCode == KeyCode.LeftBracket) script.preset.brushSize -= step;
				else script.preset.brushSize += step;

				script.preset.brushSize = Mathf.Min(script.guiMaxBrushSize, script.preset.brushSize);
			}
		}

		//evaluation limitation
		//if (data.heightmapResolution-1 > 512 ||
		//	data.alphamapResolution > 512) return;
		
		//perform undo. Using Voxeland's undo system
		if (Event.current.commandName == "UndoRedoPerformed" && script.undoList.Count!=0) 
		{
			script.allowUndo = !script.allowUndo;
			if (!script.allowUndo) return;

			int lastNum = script.undoList.Count-1;
			for (int i=script.undoList[lastNum].Count-1; i>=0; i--)
			{
				script.undoList[lastNum][i].Perform(script.terrain.terrainData);
				script.undoList[lastNum].RemoveAt(i);
			}
			script.undoList.RemoveAt(lastNum);
		}
		
		//disabling selection
		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

		//finding aiming ray
		Vector2 mousePos = Event.current.mousePosition;
		mousePos.y = Screen.height - mousePos.y - 40;
		Camera cam = UnityEditor.SceneView.lastActiveSceneView.camera;
		if (cam==null) return;
		Ray aimRay = cam.ScreenPointToRay(mousePos);

		//aiming terrain
		Collider terrainCollider = script.terrain.GetComponent<Collider>();
		if (terrainCollider==null) { Debug.LogWarning("ErosionBrush: cannot aim terrain because it does not have a collider"); return; }
		RaycastHit hit;
        if (!terrainCollider.Raycast(aimRay, out hit, Mathf.Infinity)) return;
		Vector3 brushPos = hit.point;

		//drawing brush
		DrawBrush(brushPos, preset.brushSize, script.terrain, color:script.guiBrushColor, thickness:script.guiBrushThickness, numCorners:script.guiBrushNumCorners);
		//if (Event.current.type == EventType.MouseDrag) Handles.DrawAAPolyLine(script.guiBrushThickness, new Vector3[] { oldBrushPos, brushPos } );
		DrawBrush(brushPos, preset.brushSize*preset.brushFallof, script.terrain, color:script.guiBrushColor/2, thickness:script.guiBrushThickness, numCorners:script.guiBrushNumCorners);
		

		//moving brush (needed an object to perform move)
		if (script.moveTfm==null) { script.moveTfm = new GameObject().transform; script.moveTfm.hideFlags = HideFlags.HideInHierarchy; }
		script.moveTfm.position = hit.point;

		//focusing on brush
		if (script.focusOnBrush && Event.current.keyCode == KeyCode.G && Event.current.type == EventType.KeyDown) 
		{ 
			UnityEditor.SceneView.lastActiveSceneView.LookAt( 
				brushPos, 
				UnityEditor.SceneView.lastActiveSceneView.rotation,
				preset.brushSize*3, 
				UnityEditor.SceneView.lastActiveSceneView.orthographic, 
				false);
		}

		//returning if no key was pressed or distance from old pos is less then spacing
		if (Event.current.type == EventType.MouseUp && Event.current.button == 0) oldBrushPos = new Vector3(-65000,0,-65000);
		if (!(Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) || Event.current.button != 0) return;
		if (Event.current.type == EventType.MouseDrag && preset.brushSpacing>0.001f && (new Vector3(brushPos.x,0,brushPos.z)-oldBrushPos).magnitude < preset.brushSpacing*preset.brushSize) return;
		if (Event.current.alt) return;
		oldBrushPos = new Vector3(brushPos.x,0,brushPos.z);

		//finding heightmap-spaced and splat-spaced coordinates
		Vector3 terrainSpaceCoords = brushPos - script.terrain.transform.position; //terrain.transform.InverseTransformPoint(worldSpaceCoords);

		//applying brush
		//script.ApplyBrush(heightCoords, splatCoords, useFallof:true);

		script.ApplyBrush(new Rect(
			(terrainSpaceCoords.x-preset.brushSize)/data.size.x,  
			(terrainSpaceCoords.z-preset.brushSize)/data.size.z, 
			preset.brushSize/data.size.x*2f, 
			preset.brushSize/data.size.z*2f),  
			useFallof:true, newUndo:Event.current.type==EventType.MouseDown);
		if (script.recordUndo) 
		{ 
			UnityEditor.Undo.RecordObject(script,"Erosion Brush Stroke"); 
			script.undo = !script.undo; UnityEditor.EditorUtility.SetDirty(this); //setting object change
		}

		//refreshin terrain for Unity5
		if (script.unity5positioning && (Event.current.type == EventType.mouseDown || unity5terrainRefreshCounter==10))
		{
			//data.size += Vector3.up*0.01f;
			//data.size -= Vector3.up*0.01f;
			//unity5terrainRefreshCounter=0;
		}
		unity5terrainRefreshCounter++;
	}

	public void DrawBrush (Vector3 pos, float radius, Terrain terrain, Color color, float thickness=3f, int numCorners=32)
	{
		//incline is the height delta in one unit distance
		Handles.color = color;
		
		Vector3[] corners = new Vector3[numCorners+1];
		float step = 360f/numCorners;
		for (int i=0; i<=corners.Length-1; i++)
		{
			corners[i] = new Vector3( Mathf.Sin(step*i*Mathf.Deg2Rad), 0, Mathf.Cos(step*i*Mathf.Deg2Rad) ) * radius + pos;
			corners[i].y = terrain.SampleHeight(corners[i]);
		}
		Handles.DrawAAPolyLine(thickness, corners);
	}

	public void EditorUpdate ()
	{
		if (script==null) return; //in case of re-assigning missing script
		
		//finding terrain
		if (script.terrain==null) 
			try { script.terrain = script.GetComponent<Terrain>(); }
			catch (Exception e) { UnityEditor.EditorApplication.update -= EditorUpdate; e.GetType(); } //get type to disable warinng 'never used'
		if (script.terrain==null) return;
		
		RefreshTerrainGui();
	}

	public void RefreshTerrainGui ()
	{
		//returning components order to finish refresh
		if (script.moveDown) 
		{ 
			script.moveDown=false;
			UnityEditorInternal.ComponentUtility.MoveComponentDown(script); 
		}

		//disabling terrain tool if pain is turned on
		if (script.paint && !script.wasPaint)
		{
			script.wasPaint = true;

			//finding terrain reflections
			System.Type terrainType = null;
			System.Type[] tmp = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetTypes();
				for (int i=tmp.Length-1; i>=0; i--) 
			{
				//if (tmp[i].Name.ToLower().Contains("terrain"))
				//	Debug.Log(tmp[i]);
				if (tmp[i].Name=="TerrainInspector") 
						{ terrainType=tmp[i]; break; } //GetType just by name do not work
			}

			object[] editors = Resources.FindObjectsOfTypeAll(terrainType);
			for (int i=0; i<editors.Length; i++)
			{
				PropertyInfo toolProp = terrainType.GetProperty("selectedTool", BindingFlags.Instance | BindingFlags.NonPublic);	

				toolProp.SetValue(editors[i], -1, null);

				//moving component up to refresh terrain tool state
				UnityEditorInternal.ComponentUtility.MoveComponentUp(script); 
				script.moveDown=true;
			}

			script.terrain.hideFlags = HideFlags.NotEditable;
		}

		//enabling terrain if pain was turned off
		if (!script.paint && script.wasPaint)
		{
			script.wasPaint = false;
			script.terrain.hideFlags = HideFlags.None; 
		}
	}
	#endregion //Scene region

}//EB editor

public class SavePresetWindow : EditorWindow
{
	public ErosionBrushEditor main;
	public readonly Vector2 windowSize = new Vector2(300, 120);
	
	public new string name;
	public bool saveBrushSize = false;
	public bool saveBrushParams = true;
	public bool saveErosionNoiseParams = true;
	public bool saveSplatParams = true;
	
	public void OnGUI ()
	{
		EditorGUIUtility.labelWidth = 50;
		
		name = EditorGUILayout.TextField("Name:", name);

		EditorGUILayout.Space();
		saveBrushSize = EditorGUILayout.ToggleLeft(new GUIContent("Save Brush Size", "Each time the preset will be selected Brush Size will be set to current one."), saveBrushSize);
		saveBrushParams = EditorGUILayout.ToggleLeft(new GUIContent("Save Brush Parameters", "Brush fallof, spacing, downscale and blur"), saveBrushParams);
		if (main.script.preset.isErosion) saveErosionNoiseParams = EditorGUILayout.ToggleLeft(new GUIContent("Save Erosion Parameters", "Durability, fluidity and amounts"), saveErosionNoiseParams);
		else saveErosionNoiseParams = EditorGUILayout.ToggleLeft(new GUIContent("Save Noise Parameters", "Amount, size, detail, uplift and riffle"), saveErosionNoiseParams);
		saveSplatParams = EditorGUILayout.ToggleLeft(new GUIContent("Save Splat Parameters", "Splats num and opacity"), saveSplatParams);

		EditorGUILayout.Space();
		if (GUILayout.Button(new GUIContent("Save", "Save current splat to list"))) 
		{
			main.SavePreset(-1, name, saveBrushSize, saveBrushParams, saveErosionNoiseParams, saveSplatParams);
			main.script.guiSelectedPreset = main.script.presets.Length-1;
			this.Close();
		}
	}
}


}//namespace