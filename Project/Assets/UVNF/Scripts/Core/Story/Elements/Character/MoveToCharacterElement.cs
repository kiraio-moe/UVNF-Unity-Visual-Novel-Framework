using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;
using UVNF.Entities.Containers.Variables;

namespace UVNF.Core.Story.Character
{
    public class MoveToCharacterElement : StoryElement
    {
        public override string ElementName => "Move To Character";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Character();

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public CharacterVariableManager CharacterManager;
        int CharacterIndex = 0;
        int DestinationIndex = 0;

        public string Character = "Velorexe";
        public string Destination = "Jocelyn";

        [Range(0.1f, 10f)]
        public float MoveTime = 1f;

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
                CharacterIndex = EditorGUILayout.Popup("Character", CharacterIndex, CharacterManager.CharactersList());
                Character = CharacterManager.CharaVariables[CharacterIndex].CharacterName;

                DestinationIndex = EditorGUILayout.Popup("Destination", DestinationIndex, CharacterManager.CharactersList());
                Destination = CharacterManager.CharaVariables[DestinationIndex].CharacterName;
            }
            else if (CharacterManager == null)
            {
                GUILayout.Label("Custom Character", EditorStyles.boldLabel);
                Character = EditorGUILayout.TextField("Character", Character);
                Destination = EditorGUILayout.TextField("Destination", Destination);
            }

            MoveTime = EditorGUILayout.Slider("Move Time", MoveTime, 0.1f, 10f);
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.MoveCharacterTo(Character, Destination, MoveTime);
            return null;
        }
    }
}
