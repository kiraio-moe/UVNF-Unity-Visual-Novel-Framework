﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Other
{
    public class LogElement : StoryElement
    {
        public override string ElementName => "Log";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Other();

        public override StoryElementTypes Type => StoryElementTypes.Other;

        [TextArea(7, 7)]
        public string LogText;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            GUILayout.Space(8);
            GUILayout.Label("Log Text");
            LogText = EditorGUILayout.TextArea(LogText, GUILayout.Height(100));
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            Debug.Log(LogText);
            return null;
        }
    }
}
