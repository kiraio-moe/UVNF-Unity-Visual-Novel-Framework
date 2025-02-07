﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UVNF.Core.UI;

namespace UVNF.Core.Story.Dialogue
{
    public class AudioDialogueElement : DialogueElement
    {
        public override string ElementName => "Audio Dialogue";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public bool Beep;
        public bool BeepOnVowel;

        public bool PitchShift;

        [Range(0.1f, 0.2f)]
        public float MaxPitch;

        public AudioClip BoopSoundEffect;
        public AudioClip DialogueClip;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            base.DisplayLayout(layoutRect, label);
            Beep = GUILayout.Toggle(Beep, "Beep");
            if (Beep)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Beep SFX");
                    BoopSoundEffect = EditorGUILayout.ObjectField(BoopSoundEffect, typeof(AudioClip), false) as AudioClip;
                }
                GUILayout.EndHorizontal();

                BeepOnVowel = GUILayout.Toggle(BeepOnVowel, "Beep Only On Vowel");

                PitchShift = GUILayout.Toggle(PitchShift, "Pitch Shift");
                if (PitchShift)
                    MaxPitch = EditorGUILayout.Slider(MaxPitch, 0.1f, 0.2f);
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Dialogue");
                    DialogueClip = EditorGUILayout.ObjectField(DialogueClip, typeof(AudioClip), false) as AudioClip;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(8);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (Beep)
                return canvas.DisplayText(Dialogue, CharacterName, UniqueColor, BoopSoundEffect, managerCallback.AudioManager, MaxPitch, BeepOnVowel);
            else
                return canvas.DisplayText(Dialogue, CharacterName, UniqueColor, DialogueClip, managerCallback.AudioManager);
        }
    }
}
