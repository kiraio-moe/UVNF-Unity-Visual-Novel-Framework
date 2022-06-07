using System.Collections;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Audio
{
    public class StopAudio : StoryElement
    {
        public override string ElementName => "Stop Audio";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Audio();

        public override StoryElementTypes Type => StoryElementTypes.Audio;

        public bool PauseBackgroundMusic = true;

        public bool Fade = true;

        [Range(0.1f, 10f)]
        public float FadeTime = 1f;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            GUILayout.Space(8);
            PauseBackgroundMusic = GUILayout.Toggle(PauseBackgroundMusic, "Pause Background Music");
            Fade = GUILayout.Toggle(Fade, "Fade Background Music");
            if (Fade)
            {
                FadeTime = EditorGUILayout.Slider("Fade Time", FadeTime, 0.1f, 10f);
            }
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (PauseBackgroundMusic)
            {
                if (Fade)
                    managerCallback.AudioManager.StopBackgroundMusic(FadeTime, false);
                else
                    managerCallback.AudioManager.PauseBackgroundMusic();
            }
            else
            {
                if (Fade)
                    managerCallback.AudioManager.StopBackgroundMusic(FadeTime);
                else
                    managerCallback.AudioManager.StopBackgroundMusic();
            }
            yield return null;
        }
    }
}
