using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace NoiseBrushPlugin
{
	public class Layout : Editor
	{
		#region Layout Instruments

			public float size = 1;
			public float minSize = 0.5f;
			public bool change = false;
			public bool disabled=false;
			public bool useEvents=true;
			
			//sizes not dependant from scale (as if it is 1)
			public int margin;
			public int rightMargin;
			public int lineHeight = 18;
			public float width 
			{ 
				get { return field.width/size - margin - rightMargin; }
				set { field.width = (value + margin + rightMargin)*size; }
			}
			public float start { get{return field.x+margin*size;}}

			//rects as they appear on screen (scaled)
			public Rect field; //background rect of layout. Height increases every new line
			public Rect cursor; //rect with zero width


			public void Par (int height=0) //NewLine
			{ 
				//replacing height with default
				if (height==0) height = lineHeight;

				//setting rects
				cursor = new Rect(start, cursor.y+cursor.height, 0, height*size);
				field = new Rect(field.x, field.y, field.width, Mathf.Max(field.height, cursor.y+cursor.height-field.y));
			}
		
			public Rect Inset (float width=1f) //AutoRect
			{ 
				if (Event.current.type == EventType.Layout) return new Rect();
				
				//replacing width to pixels if it is less 1
				if (width < 1.0001f) width *= this.width;

				//scaling it
				float scaledWidth = width*size;

				//automatically creating new line if width exceeds field
				//if (cursor.x + width > field.x + field.width + 0.001f) Par();
				
				cursor.x += scaledWidth;
				return new Rect (cursor.x-scaledWidth, cursor.y, scaledWidth-3*size, cursor.height-2*size);
			}

			public void ToLineStart ()
			{
				cursor = new Rect(field.x+margin*size, cursor.y, 0, cursor.height);
			}
		
			public void GetInspectorField ()
			{
				UnityEditor.EditorGUI.indentLevel = 0;
				field = EditorGUILayout.GetControlRect(GUILayout.Height(0));
				//EditorGUI.
				 //GUILayoutUtility.GetRect(1, 0);
				//EditorGUILayout.LabelField("", GUILayout.Height(0));
				//GUIUtility.;

				//GUI.Button(rect, "");

				//Debug.Log(EditorGUIUtility.);
				//field = new Rect(rect.x, rect.y, rect.width, 0); //15 pixels on scrollbar, 15 on a margin
				cursor = new Rect(field.x, field.y, 0,0);
			}

			public void SetInspectorField ()
			{
				//if (Event.current.type == EventType.Layout) GUILayoutUtility.GetRect(1, field.height+UnityEditor.EditorGUIUtility.singleLineHeight, "TextField");
				if (Event.current.type == EventType.Layout) GUILayoutUtility.GetRect(1, field.height, "TextField");
			}

		#endregion

		#region Draw

			[System.NonSerialized] private GUIStyle _labelStyle = null;
			[System.NonSerialized] private GUIStyle _foldoutStyle = null;
			[System.NonSerialized] private GUIStyle _fieldStyle = null;
			[System.NonSerialized] private GUIStyle _buttonStyle = null;
			[System.NonSerialized] private GUIStyle _textButtonStyle = null;
			[System.NonSerialized] private GUIStyle _enumZoomStyle = null;
			[System.NonSerialized] private GUIStyle _urlStyle = null;
			//privates could be serialized. Took me an hour to find out why styles do not display

			public GUIStyle labelStyle { get{ if (_labelStyle == null) _labelStyle = new GUIStyle(EditorStyles.label); _labelStyle.fontSize = Mathf.RoundToInt(11 * size); return _labelStyle; } }
			public GUIStyle foldoutStyle { get{ if (_foldoutStyle == null) _foldoutStyle = new GUIStyle(EditorStyles.foldout); _foldoutStyle.fontStyle = FontStyle.Bold; _foldoutStyle.fontSize = Mathf.RoundToInt(11 * size); return _foldoutStyle; } }
			public GUIStyle fieldStyle { get{ if (_fieldStyle == null) _fieldStyle = new GUIStyle(EditorStyles.numberField); _fieldStyle.fontSize = Mathf.RoundToInt(14 * size * 0.8f); return _fieldStyle; } }
			public GUIStyle buttonStyle { get{ if (_buttonStyle == null) _buttonStyle = new GUIStyle("Button"); _buttonStyle.fontSize = Mathf.RoundToInt(11 * size); return _buttonStyle; } }
			public GUIStyle textButtonStyle { get{ if (_textButtonStyle == null) _textButtonStyle = new GUIStyle(EditorStyles.label); _textButtonStyle.fontSize = Mathf.RoundToInt(11 * size); return _textButtonStyle; } }
			public GUIStyle enumZoomStyle { get{ if (_enumZoomStyle == null) _enumZoomStyle = new GUIStyle(EditorStyles.miniButton); 
				_enumZoomStyle.alignment = TextAnchor.MiddleLeft; _enumZoomStyle.fontSize = Mathf.RoundToInt(14 * size * 0.8f); return _enumZoomStyle; } }
			public GUIStyle urlStyle { get{ if (_urlStyle == null) _urlStyle = new GUIStyle(EditorStyles.label); _labelStyle.fontSize = Mathf.RoundToInt(11 * size); 
				_urlStyle.normal.textColor = new Color(0.3f, 0.5f, 1f);  return _urlStyle; } }
			
			public void Label (string label, string tooltip="", float width=1f, bool prefix=false, bool bold=false, TextAnchor align=TextAnchor.LowerLeft, string url=null)
			{
				if (size < minSize) { Inset(width); return; }
				
				//setting style
				labelStyle.normal.textColor = url!=null ? new Color(0.3f, 0.5f, 1f) : Color.black;
				labelStyle.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
				labelStyle.alignment = align;
				
				Rect rect = Inset(width);
				GUIContent guiContent = new GUIContent(label, tooltip);
				if (disabled) EditorGUI.BeginDisabledGroup(true);

				if (prefix) EditorGUI.PrefixLabel(rect, guiContent, labelStyle);
				else if (url != null) { if (GUI.Button(rect, guiContent, labelStyle)) Application.OpenURL(url); }
				else UnityEditor.EditorGUI.LabelField(rect, guiContent, labelStyle);

				if (disabled) EditorGUI.EndDisabledGroup();
			}

			public bool Button (string label, string tooltip="", float width=1f, bool asText=false)
				{ bool result=false; Button (ref result, label, tooltip:tooltip, width:width, toggle:false, asText:asText); return result; }

			public bool Button (ref bool state, string label, string tooltip="", float width=1f, bool toggle=false, bool asText=false)
			{
				if (size < minSize) { Inset(width); return false; }
				
				Rect rect = Inset(width);
				GUIContent guiContent = new GUIContent(label, tooltip);
				if (disabled) EditorGUI.BeginDisabledGroup(true);

				bool newState = state;
				if (toggle) newState = GUI.Toggle(rect, newState, guiContent, buttonStyle);
				else newState = GUI.Button(rect, guiContent, asText ? textButtonStyle : buttonStyle);

				if (disabled) EditorGUI.EndDisabledGroup();
				if (newState != state) { state=newState; change=true; return true; }
				else return false;
			}

			public bool Foldout (ref bool open, string label, string tooltip="", float width=1f)
			{
				Rect rect = Inset(width);
				GUIContent guiContent = new GUIContent(label, tooltip);
				if (disabled) EditorGUI.BeginDisabledGroup(true);

				bool newOpen = EditorGUI.Foldout(rect, open, guiContent, true, foldoutStyle);

				if (disabled) EditorGUI.EndDisabledGroup();
				if (newOpen != open) { open=newOpen; change=true; return true; }
				else return false;
			}

			public void Url (string url, string label, string tooltip="", float width=1f)
			{
				Rect rect = Inset(width);
				GUIContent guiContent = new GUIContent(label, tooltip);
				if (disabled) EditorGUI.BeginDisabledGroup(true);

				if (GUI.Button(rect, guiContent, urlStyle)) Application.OpenURL(url);

				if (disabled) EditorGUI.EndDisabledGroup();
			}

			public bool Slider<T> (ref T param, float min=0, float max=0, bool quadratic=false, float width=1f)
			{
				Rect rect = Inset(width);
				if (size < minSize) return false;
				if (disabled) EditorGUI.BeginDisabledGroup(true);
				
				//uplifting rect for sliders to make it in the center when size != 1
				Rect upliftedRect = rect;
				upliftedRect.y = rect.center.y - 9;

				//drawing sliders
				T newParam = default(T);

				if (typeof(T) == typeof(float)) 
				{
					if (quadratic) newParam = (T)(object)Mathf.Pow(GUI.HorizontalSlider(upliftedRect, Mathf.Pow((float)(object)param,0.5f), Mathf.Pow(min,0.5f), Mathf.Pow(max,0.5f)),2); 
					else newParam = (T)(object)GUI.HorizontalSlider(upliftedRect, (float)(object)param, min, max);
				}
				else if (typeof(T) == typeof(int)) 
				{
					if (quadratic) newParam = (T)(object)Mathf.RoundToInt(Mathf.Pow(GUI.HorizontalSlider(upliftedRect, Mathf.Pow((int)(object)param,0.5f), Mathf.Pow(min,0.5f), Mathf.Pow(max,0.5f)),2)); 
					else newParam = (T)(object)Mathf.RoundToInt(GUI.HorizontalSlider(upliftedRect, (int)(object)param, min, max));
				}
				else if (typeof(T) == typeof(Vector2)) 
				{
					Vector2 vec = (Vector2)(object)param;
					if (quadratic) 
					{
						vec.x = Mathf.Pow(vec.x, 0.5f); vec.y = Mathf.Pow(vec.y, 0.5f);
						EditorGUI.MinMaxSlider(upliftedRect, ref vec.x, ref vec.y, Mathf.Pow(min,0.5f), Mathf.Pow(max,0.5f));
						vec.x = Mathf.Pow(vec.x, 2); vec.y = Mathf.Pow(vec.y, 2);
					}
					else EditorGUI.MinMaxSlider(upliftedRect, ref vec.x, ref vec.y, min, max);
					newParam = (T)(object) vec;
				}

				//using event
				if (useEvents && Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition) )
					Event.current.Use();

				//determining change
				bool curChange = false;
				if (!EqualityComparer<T>.Default.Equals(param, newParam)) { change = true; curChange = true; }
				param = newParam;

				if (disabled) EditorGUI.EndDisabledGroup();

				return curChange;
			}

			public bool Field<T> (ref T param, bool allowSceneObject=false, float width=1f)
			{
				if (size < minSize) { Inset(width); return false; }

				if (disabled) EditorGUI.BeginDisabledGroup(true);

				Rect rect = Inset(width);

				//uplifting rect
				Rect upliftedRect = rect;
				upliftedRect.y = rect.center.y - 9;

				//drawing fields
				T newParam = default(T);

				if (typeof(T) == typeof(float)) newParam = (T)(object)UnityEditor.EditorGUI.FloatField(rect, (float)(object)param, fieldStyle);
				else if (typeof(T) == typeof(int)) newParam = (T)(object)UnityEditor.EditorGUI.IntField(rect, (int)(object)param, fieldStyle);
				else if (typeof(T) == typeof(Vector2)) 
				{
					newParam = (T)(object) new Vector2(
						EditorGUI.FloatField(new Rect(rect.x, rect.y, rect.width/2-1, rect.height), ((Vector2)(object)param).x, fieldStyle),
						EditorGUI.FloatField(new Rect(rect.x + rect.width/2+1, rect.y, rect.width/2-1, rect.height), ((Vector2)(object)param).y, fieldStyle));
				}
				else if (typeof(T) == typeof(bool)) 
				{
					if (size > 0.75f) newParam = (T)(object)UnityEditor.EditorGUI.Toggle(upliftedRect, (bool)(object)param);
					else  { rect.width = 16*size; newParam = (T)(object)UnityEditor.EditorGUI.Toggle(rect, (bool)(object)param, EditorStyles.miniButton); }
				}
				else if (typeof(T) == typeof(string)) newParam = (T)(object)UnityEditor.EditorGUI.TextField(rect, (string)(object)param);
				else if (typeof(T) == typeof(Color)) newParam = (T)(object)UnityEditor.EditorGUI.ColorField(rect, (Color)(object)param);
				else if (typeof(T) == typeof(Texture2D)) newParam = (T)(object)UnityEditor.EditorGUI.ObjectField(rect, (Texture2D)(object)param, typeof(Texture2D), false);
				else if (typeof(T) == typeof(Transform)) newParam = (T)(object)UnityEditor.EditorGUI.ObjectField(rect, (Transform)(object)param, typeof(Transform), false);
				else if (typeof(T).IsEnum) 
				{
					if (size > 0.75f) newParam = (T)(object)UnityEditor.EditorGUI.EnumPopup(upliftedRect, param as System.Enum);
					else newParam = (T)(object)UnityEditor.EditorGUI.EnumPopup(rect, param as System.Enum, enumZoomStyle);
				}
				else if (typeof(T) == typeof(UnityEngine.Object)) newParam = (T)(object)UnityEditor.EditorGUI.ObjectField(rect, (UnityEngine.Object)(object)param, typeof(T), allowSceneObject);
				else if (typeof(T) == typeof(AnimationCurve))
				{
					EditorGUI.BeginChangeCheck();
					param = (T)(object)EditorGUI.CurveField(rect, (AnimationCurve)(object)param, Color.white, new Rect(0,0,1,1));
					if (EditorGUI.EndChangeCheck()) change=true; newParam = param;
				}

				//using event
				if (useEvents && Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition) )
					Event.current.Use();

				//determining change
				bool curChange = false;
				if (!EqualityComparer<T>.Default.Equals(param, newParam)) { change = true; curChange = true; }
				param = newParam;

				if (disabled) EditorGUI.EndDisabledGroup();

				return curChange;
			}

			public void TextureSelector (ref int selected, Texture2D[] textures, float width=1f)
			{
				//saving cursor position
				Rect savedCursor = cursor;

				//drawing background
				Rect rect = Inset(width);
				UnityEditor.EditorGUI.HelpBox(rect, "", UnityEditor.MessageType.None);

				//drawing textures
				cursor = new Rect (savedCursor.x+5, cursor.y+5, 0, cursor.height-10);

				for (int i=0; i<textures.Length; i++) 
				{
					Rect textureRect = Inset(savedCursor.height-10);

					//drawing selected background
					if (i==selected)
						GUI.Box(new Rect(textureRect.x-2, textureRect.y-2, textureRect.width+4, textureRect.height+4), new GUIContent());
					
					//selecting texture
					if (GUI.Button(textureRect, "", "Box")) selected = i;

					//drawing texture
					UnityEditor.EditorGUI.DrawPreviewTexture(textureRect, textures[i]);
				}

				cursor = savedCursor;
			}

		#endregion

		#region Quick Slider

			public float fieldSize = 0.5f;

			public bool Quick<T> (ref T param, string label, string tooltip="", float min=0, float max=0, bool slider=true, bool quadratic=false) 
			{
				Par();

				if (size < minSize) return false;

				Label(label, tooltip, width:fieldSize<1f ? 1-fieldSize : field.width-fieldSize);//, prefix:true);
				
				if ( (typeof(T)==typeof(float) || typeof(T)==typeof(int)) )
				{ 
					if (slider) return Slider<T>(ref param, min, max, width:fieldSize*0.66f, quadratic:quadratic) || Field<T>(ref param, width:fieldSize*0.34f);
					else return Field<T>(ref param, width:fieldSize);
				}

				else if ( typeof(T)==typeof(Vector2) )
				{ 
					Vector2 vec = (Vector2)(object)param;
					bool change = false;
					if (slider) change = Slider<Vector2>(ref vec, min, max, width:fieldSize*0.55f, quadratic:quadratic) ||
							Field<float>(ref vec.x, width:fieldSize*0.225f) ||
							Field<float>(ref vec.y, width:fieldSize*0.225f);
					else change = Field<float>(ref vec.x, width:fieldSize*0.5f) || Field<float>(ref vec.y, width:fieldSize*0.5f);
					param = (T)(object)vec;
					return change;
				}

				else return Field<T>(ref param, width:fieldSize);
			}

		#endregion

		#region Array Instruments

			static public void ArrayRemoveAt<T> (ref T[] array, int num)
			{
				T[] newArray = new T[array.Length-1];
				for (int i=0; i<newArray.Length; i++) 
				{
					if (i<num) newArray[i] = array[i];
					else newArray[i] = array[i+1];
				}
				array = newArray;
			}

			static public void ArrayAdd<T> (ref T[] array, int after, T element=default(T))
			{
				if (array==null || array.Length==0) { array = new T[] {element}; return; }
				if (after > array.Length-1) after = array.Length-1;
				
				T[] newArray = new T[array.Length+1];
				for (int i=0; i<newArray.Length; i++) 
				{
					if (i<=after) newArray[i] = array[i];
					else if (i == after+1) newArray[i] = element;
					else newArray[i] = array[i-1];
				}
				array = newArray;
			}
			static public void ArrayAdd<T> (ref T[] array, T element=default(T)) { ArrayAdd<T>(ref array, array.Length-1, element); }

			static public void ArraySwitch<T> (T[] array, int num1, int num2)
			{
				if (num1<0 || num1>=array.Length || num2<0 || num2 >=array.Length) return;
				
				T temp = array[num1];
				array[num1] = array[num2];
				array[num2] = temp;
			}
			
			public void ArrayButtons<T> (ref T[] array, ref int selected, bool drawUpDown=true, bool drawDelete=true, T element=default(T))
			{
				Par();
				Inset(0.4f);
				if (drawUpDown)
				{
					if (Button("⤴", tooltip:"Move selected up", width:0.15f) && selected != 0)
					{
						ArraySwitch<T> (array, selected-1, selected);
						selected--;
					}
				
					if (Button("⤵", tooltip:"Move selected down", width:0.15f) && selected < array.Length-1)
					{
						ArraySwitch<T> (array, selected+1, selected);
						selected++;
					}
				}
				else Inset(0.3f);
			
				if (Button("+", tooltip:"Add new array element", width:0.15f))
				{
					ArrayAdd<T>(ref array, selected, element);
					selected++;
				}
			
				if (Button("✕", tooltip:"Remove element", width:0.15f))
					{ ArrayRemoveAt<T>(ref array, selected); }
			}

			public void ArrayButtons<T1,T2> (ref T1[] array1, ref T2[] array2, ref int selected, bool drawUpDown=true, bool drawDelete=true, T1 element1=default(T1), T2 element2=default(T2), bool addToStart=false)
			{
				Par();
				Inset(0.4f);
				if (drawUpDown)
				{
					if (Button("⤴", tooltip:"Move selected up", width:0.15f) && selected != 0)
					{
						ArraySwitch<T1> (array1, selected-1, selected);
						ArraySwitch<T2> (array2, selected-1, selected);
						selected--;
					}
				
					if (Button("⤵", tooltip:"Move selected down", width:0.15f) && selected < array1.Length-1)
					{
						ArraySwitch<T1> (array1, selected+1, selected);
						ArraySwitch<T2> (array2, selected+1, selected);
						selected++;
					}
				}
				else Inset(0.3f);
			
				if (Button("+", tooltip:"Add new array element", width:0.15f))
				{
					ArrayAdd<T1>(ref array1, addToStart ? -1:selected, element1);
					ArrayAdd<T2>(ref array2, addToStart ? -1:selected, element2);
					if (!addToStart) selected++; else selected=0;
				}
			
				if (Button("✕", tooltip:"Remove element", width:0.15f))
				{ 
					ArrayRemoveAt<T1>(ref array1, selected); 
					ArrayRemoveAt<T2>(ref array2, selected); 
				}
			}
		#endregion
	
		#region Popup Menu

		public class MenuItem 
		{
			public string name;
			public bool clicked;
			public MenuItem[] subItems = null;
			
			//public delegate void OnClick();
			//public OnClick onClick;
			public System.Action onClick;

			public int Count { get{ return subItems==null ? 0 : subItems.Length; } }
			public bool hasSubs { get{ return subItems!=null;} }
		}

		static public EditorWindow DrawPopup (MenuItem[] items, Vector2 pos)
		{
			Menu popupWindow = new Menu();
			popupWindow.minSize = new Vector2(0,0);
			popupWindow.position = new Rect(pos.x, pos.y, 100, items.Length*16);
			popupWindow.items = items;
			popupWindow.ShowPopup();
			popupWindow.Focus();

			//UnityEditor.EditorApplication.update -= CloseAllMenusIfNotFocused;	
			//UnityEditor.EditorApplication.update += CloseAllMenusIfNotFocused;	

			//Menu.allMenus.Add(popupWindow);
			return popupWindow;
		}

		/*static public void CloseAllMenusIfNotFocused ()
		{
			if (EditorWindow.focusedWindow.GetType() != typeof(Menu))
				for (int i=Menu.allMenus.Count-1; i>=0; i--)
					Menu.allMenus[i].Close();
		}*/

		class Menu : EditorWindow 
		{
			//static public bool newMenuOpened; //ignore OnLostFocus
			//static public List<Menu> allMenus = new List<Menu>();
			
			//public SceneMagicWindow baseWindow;

			static private Texture2D background;
			static private Texture2D highlight;
			
			public MenuItem[] items;

			private MenuItem lastItem;
			private System.DateTime lastTimestart;
			private bool timeUsed = false;

			private EditorWindow expandedWindow = null;
		
			void CloseMenuIfNotFocused () { if (EditorWindow.focusedWindow.GetType() != typeof(Menu)) this.Close(); } 
			void OnEnable () { UnityEditor.EditorApplication.update += CloseMenuIfNotFocused; }
			void OnDisable () { UnityEditor.EditorApplication.update -= CloseMenuIfNotFocused; }

			void OnGUI()
			{
				if (background==null)
				{
					background = new Texture2D(1, 1, TextureFormat.RGBA32, false);
					background.SetPixel(0, 0, new Color(0.98f, 0.98f, 0.98f));
					background.Apply();
				}
			
				if (highlight==null)
				{
					highlight = new Texture2D(1, 1, TextureFormat.RGBA32, false);
					highlight.SetPixel(0, 0, new Color(0.6f, 0.7f, 0.9f));
					highlight.Apply();
				}

				Rect fullRect = new Rect(0,0,this.position.width,this.position.height);
				float lineHeight = this.position.height / items.Length;
			
				//background
				if (Event.current.type == EventType.repaint) GUI.skin.box.Draw(fullRect, false, true, true, false);
				GUI.DrawTexture(new Rect(1,1,fullRect.width-2,fullRect.height-2), background, ScaleMode.StretchToFill);

				//list
				for (int i=0; i<items.Length; i++)
				{
					MenuItem currentItem = items[i];
					
					currentItem.clicked = false;
					Rect lineRect = new Rect(1,i*lineHeight+1,fullRect.width-2,lineHeight-2);
					bool highlighted = lineRect.Contains(Event.current.mousePosition);

					if (highlighted) GUI.DrawTexture(lineRect, highlight);
					
					//labels
					EditorGUI.LabelField(lineRect, currentItem.name);

					if (currentItem.hasSubs)
					{
						char rightChar = '\u25B6';
						Rect rightRect = lineRect; rightRect.width = 12; rightRect.x = lineRect.x + lineRect.width - rightRect.width;
						EditorGUI.LabelField(rightRect, "" + rightChar);
					}

					//pressing
					if (highlighted && Event.current.type == EventType.MouseDown && Event.current.type == 0)
					{
						currentItem.clicked = true;
						this.Close();

						if (currentItem.onClick != null) 
							currentItem.onClick();

						EditorWindow.focusedWindow.Repaint();
					}

					//opening subsmenus
					if (highlighted)
					{
						if (currentItem != lastItem)
						{
							lastTimestart = System.DateTime.Now;
							timeUsed = false;
						}

						if ((System.DateTime.Now-lastTimestart).TotalMilliseconds > 500 && !timeUsed) 
						{
							//closing old expanded window
							if (expandedWindow != null) expandedWindow.Close();

							//opening new one
							if (currentItem.hasSubs)
								expandedWindow = DrawPopup(currentItem.subItems, lineRect.max-new Vector2(0,lineHeight)+this.position.position);
							else
								this.Focus(); //returning focus to this menu

							timeUsed = true;
						}

						lastItem = currentItem;
					}
				}

				//selecting
				/*int selected = GUI.SelectionGrid(fullRect,-1,items,1,EditorStyles.label); 
				if (selected != -1)
				{
					System.Type genType = Generator.types[selected+1];
					Generator newGen = (Generator)System.Activator.CreateInstance(genType);

					Vector2 newGenPos = new Vector2(50-baseWindow.position.x+this.position.x, 10-baseWindow.position.y+this.position.y);
					newGen.guiRect = baseWindow.ToWorldSpace( new Rect(newGenPos.x-Generator.guiRectWidth/2, newGenPos.y-20, Generator.guiRectWidth, 1) );
				
					baseWindow.script.AddGenerator(newGen);
				
					this.Close();
					baseWindow.Repaint();
				}*/
				

				//if (GUI.Button(new Rect(10,10, 40, 20), "Close")) this.Close();
				if (Event.current.rawType == EventType.mouseUp) this.Close();
				
				
				//if (EditorWindow.focusedWindow.GetType() != typeof(Menu)) this.Focus();
				this.Repaint();
			}

			//public void OnLostFocus() { this.Close(); }
		}

		#endregion
	}
}
