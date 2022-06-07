using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;
using CoroutineManager;

namespace UVNF.Core.Story.Scenery
{
    public class TransitionElement : StoryElement
    {
        public override string ElementName => "Transition";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Scene();

        public override StoryElementTypes Type => StoryElementTypes.Scenery;

        public TransitionEnum.TransitionEffects TransitionEffects = TransitionEnum.TransitionEffects.Fade;

        public Color MainColor = new Color(0, 0, 0, 255);
        [Range(0.1f, 10f)]
        public float FadeTime = 1f;
        [Range(0.1f, 10f)]
        public float WaitDuration = 1f;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            GUILayout.Space(8);
            TransitionEffects = (TransitionEnum.TransitionEffects)EditorGUILayout.EnumPopup("Effects", TransitionEffects);

            GUILayout.Label("Options", EditorStyles.boldLabel);
            switch (TransitionEffects)
            {
                case TransitionEnum.TransitionEffects.Fade:
                    MainColor = EditorGUILayout.ColorField(new GUIContent("Color"), MainColor, true, false, false);
                    FadeTime = EditorGUILayout.Slider("Fade Time", FadeTime, 0.1f, 10f);
                    WaitDuration = EditorGUILayout.Slider("Wait Duration", WaitDuration, 0.1f, 10f);
                    break;
                case TransitionEnum.TransitionEffects.Slide:
                    // TODO
                    break;
                case TransitionEnum.TransitionEffects.Swipe:
                    // TODO
                    break;
            }
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            bool conditionTrue = false;

            switch (TransitionEffects)
            {
                case TransitionEnum.TransitionEffects.Fade:
                    new Task(canvas.ScreenFadeTransition(canvas.TransitionCanvasGroup, MainColor, FadeTime, WaitDuration), true);
                    yield return new WaitForSeconds((FadeTime * 2) + WaitDuration);
                    conditionTrue = true;
                    break;
                case TransitionEnum.TransitionEffects.Slide:
                    // TODO
                    break;
                case TransitionEnum.TransitionEffects.Swipe:
                    // TODO
                    break;
            }

            if (conditionTrue)
            {
                managerCallback.AdvanceStoryGraph(GetOutputPort("NextNode").Connection.node as StoryElement);
            }
            yield return null;
        }
    }
}
