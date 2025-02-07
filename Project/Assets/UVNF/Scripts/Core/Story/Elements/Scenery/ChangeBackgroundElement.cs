﻿using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Scenery
{
    public class ChangeBackgroundElement : StoryElement
    {
        public override string ElementName => "Change Background";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Scene();

        public override StoryElementTypes Type => StoryElementTypes.Scenery;

        public Sprite NewBackground;
        public bool Fade = true;

        [Range(0.1f, 10f)]
        public float FadeTime = 1f;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            GUILayout.Space(8);
            GUILayout.Label("New Background");
            NewBackground = EditorGUILayout.ObjectField(NewBackground, typeof(Sprite), false) as Sprite;

            Fade = GUILayout.Toggle(Fade, "Fade");
            if (Fade)
            {
                FadeTime = EditorGUILayout.Slider("Fade Out Time", FadeTime, 0.1f, 10f);
            }
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (Fade)
                canvas.ChangeBackground(NewBackground, FadeTime);
            else
                canvas.ChangeBackground(NewBackground);
            return null;
        }
    }
}
