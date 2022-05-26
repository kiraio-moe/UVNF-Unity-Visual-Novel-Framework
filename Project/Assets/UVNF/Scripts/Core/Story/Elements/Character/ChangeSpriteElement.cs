using System.Collections;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Character
{
    public class ChangeSpriteElement : StoryElement
    {
        public override string ElementName => "Change Sprite";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Character();

        public override StoryElementTypes Type => StoryElementTypes.Character;

        public string CharacterName;
        public Sprite NewSprite;

        public bool Flip;

        bool Preview;
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
                GUILayout.Label("New Character Sprite");
                NewSprite = EditorGUILayout.ObjectField(NewSprite, typeof(Sprite), false) as Sprite;
                Flip = GUILayout.Toggle(Flip, "Flip");
            }
            GUILayout.EndHorizontal();

            if (NewSprite != null)
            {
                Preview = EditorGUILayout.Foldout(Preview, "Preview", true);
                if (Preview)
                {
                    float CharacterWidth = NewSprite.rect.width - (NewSprite.rect.width / CharacterScale);
                    float CharacterHeight = NewSprite.rect.height - (NewSprite.rect.height / CharacterScale);
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

                    GUI.DrawTexture(layoutRect, NewSprite.texture, ScaleMode.ScaleToFit);
                    GUILayout.Space(CharacterHeight + 35f);
                }
            }
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            managerCallback.CharacterManager.ChangeCharacterSprite(CharacterName, NewSprite, Flip);
            return null;
        }
    }
}
