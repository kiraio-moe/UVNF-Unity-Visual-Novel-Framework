using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;
using UVNF.Entities.Containers.Variables;

namespace UVNF.Core.Story.Dialogue
{
    public class DialogueElement : StoryElement
    {
        public override string ElementName => "Dialogue";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Story();

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public CharacterVariableManager CharacterManager;
        int VariableIndex = 0;
    
        public string CharacterName;
        public Color UniqueColor = new Color(255, 255, 255, 255);
        [TextArea(10, 10)]
        public string Dialogue;

        Vector2 ScrollPosition = Vector2.zero;

        // private GUIStyle textAreaStyle;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            // if (textAreaStyle == null)
            // {
                // Texture2D areaBackground = new Texture2D(1, 1);
                // areaBackground.SetPixel(0, 0, Color.white);
                // areaBackground.Apply();

                // textAreaStyle = new GUIStyle("TextArea");
                // textAreaStyle.richText = true;
                // textAreaStyle.normal.background = areaBackground;
            // }

            GUILayout.Space(8);
            if (CharacterManager == null)
            {
                EditorGUILayout.HelpBox("You can use pre-defined character you have created by right click > Create > UVNF > Define Character and assign them or you can use a custom character below.", MessageType.Info);
                GUILayout.Space(8);
            }
            else if (CharacterManager.CharaVariables.Count <= 0)
            {
                EditorGUILayout.HelpBox("Looks like you haven\'t created any character yet, please create one at UVNF (window menu) > Define Character.", MessageType.Warning);
                GUILayout.Space(8);
            }

            CharacterManager = EditorGUILayout.ObjectField("Variables", CharacterManager, typeof(CharacterVariableManager), false) as CharacterVariableManager;

            if (CharacterManager != null && CharacterManager.CharaVariables.Count > 0)
            {
                VariableIndex = EditorGUILayout.Popup("Character", VariableIndex, CharacterManager.CharactersList());
                CharacterName = CharacterManager.CharaVariables[VariableIndex].CharacterName;
                UniqueColor = CharacterManager.CharaVariables[VariableIndex].UniqueColor;
            }
            else if (CharacterManager == null)
            {
                GUILayout.Label("Custom Character", EditorStyles.boldLabel);
                CharacterName = EditorGUILayout.TextField("Character", CharacterName);
                UniqueColor = EditorGUILayout.ColorField("Unique Color", UniqueColor);
            }
            
            GUILayout.Label("Dialogue");
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));
            {
                Dialogue = EditorGUILayout.TextArea(Dialogue, GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));
            }
            GUILayout.EndScrollView();

            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager gameManager, UVNFCanvas canvas)
        {
            return canvas.DisplayText(Dialogue, CharacterName, UniqueColor);
        }
    }
}
