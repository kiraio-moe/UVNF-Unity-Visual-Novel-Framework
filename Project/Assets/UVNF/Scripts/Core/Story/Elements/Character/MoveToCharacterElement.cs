using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Character
{
    public class MoveToCharacterElement : StoryElement
    {
        public override string ElementName => "Move To Character";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Character();

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string Character;
        public string CharacterToMoveTo;

        [Range(0.1f, 10f)]
        public float MoveTime = 1f;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            Character = EditorGUILayout.TextField("Character", Character);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Character To Move To");
                CharacterToMoveTo = EditorGUILayout.TextField(CharacterToMoveTo);
            }
            GUILayout.EndHorizontal();

            MoveTime = EditorGUILayout.Slider("Move Time", MoveTime, 0.1f, 10f);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.MoveCharacterTo(Character, CharacterToMoveTo, MoveTime);
            return null;
        }
    }
}
