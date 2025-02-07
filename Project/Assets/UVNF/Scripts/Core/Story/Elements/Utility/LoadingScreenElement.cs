﻿using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Utility
{
    public class LoadingScreenElement : StoryElement
    {
        public override string ElementName => "Loading Screen";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Utility();

        public override StoryElementTypes Type => StoryElementTypes.Utility;

        public bool ShowLoadScreen;

        [Range(0, 10f)]
        public float FadeOutTime = 1f;
        
        public bool FadeOtherElements;

        public bool WaitToFinish = true;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            GUILayout.Space(8);
            ShowLoadScreen = GUILayout.Toggle(ShowLoadScreen, "Show Load Screen");
            GUILayout.BeginHorizontal();
            {
                FadeOutTime = EditorGUILayout.Slider("Fade Out Time", FadeOutTime, 0, 10f);
            }
            GUILayout.EndHorizontal();
            FadeOtherElements = GUILayout.Toggle(FadeOtherElements, $"Fade {(ShowLoadScreen ? "Out" : "In")} Other Elements");
            WaitToFinish = GUILayout.Toggle(WaitToFinish, "Wait To Finish Before Proceeding");
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (WaitToFinish)
            {
                if (ShowLoadScreen)
                    return managerCallback.Canvas.UnfadeCanvasGroup(managerCallback.Canvas.LoadingCanvasGroup, FadeOutTime);
                else
                    return managerCallback.Canvas.FadeCanvasGroup(managerCallback.Canvas.LoadingCanvasGroup, FadeOutTime);
            }
            else
            {
                if (ShowLoadScreen)
                    managerCallback.Canvas.ShowLoadScreen(FadeOutTime, FadeOtherElements);
                else
                    managerCallback.Canvas.HideLoadScreen(FadeOutTime, FadeOtherElements);
                return null;
            }
        }
    }
}
