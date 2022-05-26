using System.Collections;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Character
{
    public class EnterSceneElement : StoryElement
    {
        public override string ElementName => "Enter Scene";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Character();

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public ScenePositions EnterFromDirection = ScenePositions.Left;
        public ScenePositions FinalPosition = ScenePositions.Left;
        
        public string CharacterName;
        public Sprite Character;

        public bool Flip;
        bool Preview;

        [Range(0.1f, 10f)]
        public float EnterTime = 1f;
        float CharacterScale = 1.3f;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Character Name");
                CharacterName = EditorGUILayout.TextField(CharacterName);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Character Sprite");
                Character = EditorGUILayout.ObjectField(Character, typeof(Sprite), false) as Sprite;
                Flip = GUILayout.Toggle(Flip, "Flip");
            }
            GUILayout.EndHorizontal();

            if (Character != null)
            {
                Preview = EditorGUILayout.Foldout(Preview, "Preview", true);
                if (Preview)
                {
                    float CharacterWidth = Character.rect.width - (Character.rect.width / CharacterScale);
                    float CharacterHeight = Character.rect.height - (Character.rect.height / CharacterScale);
                    Vector2 CharacterPosition = layoutRect.center / 4f;

                    layoutRect.position = CharacterPosition;
                    layoutRect.width = CharacterWidth;
                    layoutRect.height = CharacterHeight;

                    CharacterScale = EditorGUILayout.Slider("Zoom", CharacterScale, 1.2f, 1.3f);
                    layoutRect.position = new Vector2(CharacterPosition.x - (CharacterScale * 30f), CharacterPosition.y + 145f);

                    if (Flip)
                    {
                        // TODO: Flip image at preview
                    }

                    GUI.DrawTexture(layoutRect, Character.texture, ScaleMode.ScaleToFit);
                    GUILayout.Space(CharacterHeight + 35f);
                }
            }

            EnterFromDirection = (ScenePositions)EditorGUILayout.EnumPopup("Enter From", EnterFromDirection);
            FinalPosition = (ScenePositions)EditorGUILayout.EnumPopup("Final Position", FinalPosition);

            EnterTime = EditorGUILayout.Slider("Enter Time", EnterTime, 0.1f, 10f);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.AddCharacter(CharacterName, Character, Flip, EnterFromDirection, FinalPosition, EnterTime);
            return null;
        }

        IEnumerator AnimatePreview(float StartPosition, float EndPosition, float Duration)
        {
            yield return null;
        }
    }
}
