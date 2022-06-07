using System.Collections;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;
using UVNF.Entities.Containers.Variables;

namespace UVNF.Core.Story.Character
{
    public class ExitSceneElement : StoryElement
    {
        public override string ElementName => "Exit Scene";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Character();

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public CharacterVariableManager CharacterManager;
        int VariableIndex = 0;

        public string CharacterName;
        public ScenePositions ExitPosition;

        [Range(0.1f, 10f)]
        public float ExitTime = 1f;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
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
            }
            else if (CharacterManager == null)
            {
                GUILayout.Label("Custom Character", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Character Name");
                    CharacterName = EditorGUILayout.TextField(CharacterName);
                }
                GUILayout.EndHorizontal();
            }

            ExitPosition = (ScenePositions)EditorGUILayout.EnumPopup("Exit Position", ExitPosition);
            ExitTime = EditorGUILayout.Slider("Exit Time", ExitTime, 0.1f, 10f);
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.RemoveCharacter(CharacterName, ExitPosition, ExitTime);
            return null;
        }
    }
}
