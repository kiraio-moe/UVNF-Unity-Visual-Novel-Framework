﻿using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UVNF.Extensions;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace UVNF.Core.UI
{
    public class UVNFCanvas : MonoBehaviour
    {
        [Header("Canvas Group")]
        public CanvasGroup BottomCanvasGroup;
        public CanvasGroup ChoiceCanvasGroup;
        public CanvasGroup LoadingCanvasGroup;
        public CanvasGroup BackgroundCanvasGroup;
        public CanvasGroup TransitionCanvasGroup;

        [Header("Dialogue")]
        public TextMeshProUGUI DialogueTMP;
        public TextMeshProUGUI CharacterTMP;
        public GameObject CharacterNamePlate;

        public float TextDisplayInterval = 0.05f;
        private float tempDisplayInterval = 0f;

        private float displayIntervalTimer = 0f;

        [Header("Choices")]
        public GameObject ChoiceButton;
        public Transform ChoicePanelTransform;

        [Header("Background")]
        public Image BackgroundImage;
        public Image BackgroundFade;

        [HideInInspector]
        public int ChoiceCallback = -1;
        public void ResetChoice() => ChoiceCallback = -1;

        public bool BottomPanelEnabled => BottomCanvasGroup.gameObject.activeSelf;
        public bool ChoiceCanvasEnabled => ChoiceCanvasGroup.gameObject.activeSelf;
        public bool LoadingCanvasEnabled => LoadingCanvasGroup.gameObject.activeSelf;

#if ENABLE_INPUT_SYSTEM
        private Mouse _currentMouse = Mouse.current;
        private bool HasInput => _currentMouse.leftButton.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
        bool HasInput => Input.GetMouseButtonDown(0);
#endif

        private void Awake()
        {
            if (BackgroundCanvasGroup != null)
                BackgroundCanvasGroup.gameObject.SetActive(true);
            if (ChoiceCanvasGroup != null)
                ChoiceCanvasGroup.gameObject.SetActive(false);
            if (BottomCanvasGroup != null)
                BottomCanvasGroup.gameObject.SetActive(false);
        }

        #region Dialogue
        public IEnumerator DisplayText(string text, Color color, params TextDisplayStyle[] displayStyles)
        {
            ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
            BottomCanvasGroup.gameObject.SetActive(true);

            CharacterNamePlate.SetActive(false);
            CharacterTMP.color = color;

            int textIndex = 0;
            while (textIndex < text.Length)
            {
                if (HasInput)
                {
                    DialogueTMP.text = text;
                    textIndex = text.Length - 1;
                }
                else if (displayIntervalTimer >= tempDisplayInterval)
                {
                    DialogueTMP.text += ApplyTypography(text, ref textIndex);
                    textIndex++;
                    displayIntervalTimer = 0f;
                }
                else
                    displayIntervalTimer += Time.deltaTime;
                yield return null;
            }

            while (!HasInput) yield return null;
        }

        public IEnumerator DisplayText(string text, string characterName, Color color, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
        {
            ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
            if (useStylesForCharacterField)
                ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);

            CharacterNamePlate.SetActive(!string.IsNullOrEmpty(characterName));

            BottomCanvasGroup.gameObject.SetActive(true);
            CharacterTMP.color = color;

            if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            {
                CharacterTMP.text = characterName;
            }

            int textIndex = 0;
            while (textIndex < text.Length)
            {
                if (HasInput)
                {
                    DialogueTMP.text = text;
                    textIndex = text.Length - 1;
                }
                else if (displayIntervalTimer >= tempDisplayInterval)
                {
                    DialogueTMP.text += ApplyTypography(text, ref textIndex);
                    textIndex++;
                    displayIntervalTimer = 0f;
                }
                else
                    displayIntervalTimer += Time.deltaTime;
                yield return null;
            }

            while (!HasInput) yield return null;
        }

        public IEnumerator DisplayText(string text, string characterName, Color color, AudioClip dialogue, AudioManager audio, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
        {
            ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
            if (useStylesForCharacterField)
                ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);

            CharacterNamePlate.SetActive(!string.IsNullOrEmpty(characterName));

            BottomCanvasGroup.gameObject.SetActive(true);
            CharacterTMP.color = color;

            if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            {
                CharacterTMP.text = characterName;
            }

            audio.PlaySound(dialogue, 1f);

            int textIndex = 0;
            while (textIndex < text.Length)
            {
                if (HasInput)
                {
                    DialogueTMP.text = text;
                    textIndex = text.Length - 1;
                }
                else if (displayIntervalTimer >= tempDisplayInterval)
                {
                    DialogueTMP.text += ApplyTypography(text, ref textIndex);
                    textIndex++;
                    displayIntervalTimer = 0f;
                }
                else
                    displayIntervalTimer += Time.deltaTime;
                yield return null;
            }

            while (!HasInput) yield return null;
        }

        public IEnumerator DisplayText(string text, string characterName, Color color, AudioClip boop, AudioManager audio, float pitchShift, bool beepOnVowel = false, bool useStylesForCharacterField = false, params TextDisplayStyle[] displayStyles)
        {
            ApplyTextDisplayStylesToTMP(DialogueTMP, displayStyles);
            if (useStylesForCharacterField)
                ApplyTextDisplayStylesToTMP(CharacterTMP, displayStyles);

            CharacterNamePlate.SetActive(!string.IsNullOrEmpty(characterName));

            BottomCanvasGroup.gameObject.SetActive(true);
            CharacterTMP.color = color;

            if (!string.Equals(CharacterTMP.text, characterName, StringComparison.Ordinal))
            {
                CharacterTMP.text = characterName;
            }

            int textIndex = 0;
            while (textIndex < text.Length)
            {
                if (HasInput)
                {
                    DialogueTMP.text = text;
                    textIndex = text.Length - 1;
                }
                else if (displayIntervalTimer >= tempDisplayInterval)
                {
                    DialogueTMP.text += ApplyTypography(text, ref textIndex);

                    if (text[textIndex] != ' ')
                    {
                        if (beepOnVowel && text[textIndex].IsVowel())
                            audio.PlaySound(boop, 1f, UnityEngine.Random.Range(0, pitchShift));
                        else if (!beepOnVowel)
                            audio.PlaySound(boop, 1f, UnityEngine.Random.Range(0, pitchShift));
                    }

                    textIndex++;
                    displayIntervalTimer = 0f;

                }
                else
                    displayIntervalTimer += Time.deltaTime;
                yield return null;
            }

            while (!HasInput) yield return null;
        }
        #endregion

        #region Choice
        public void DisplayChoice(string[] options, bool hideDialogue = true, params TextDisplayStyle[] displayStyles)
        {
            StartCoroutine(DisplayChoiceCoroutine(options, hideDialogue, displayStyles));
        }

        public IEnumerator DisplayChoiceCoroutine(string[] options, bool hideDialogue = true, params TextDisplayStyle[] displayStyles)
        {
            BottomCanvasGroup.gameObject.SetActive(!hideDialogue);
            ChoiceCanvasGroup.gameObject.SetActive(true);

            foreach (Transform child in ChoicePanelTransform)
                Destroy(child.gameObject);

            for (int i = 0; i < options.Length; i++)
            {
                ChoiceButton button = Instantiate(ChoiceButton, ChoicePanelTransform).GetComponent<ChoiceButton>();
                button.Display(options[i], i, this);
            }

            while (ChoiceCallback == -1) yield return null;

            ChoiceCanvasGroup.gameObject.SetActive(false);

            foreach (Transform child in ChoicePanelTransform)
                Destroy(child.gameObject);
        }
        #endregion

        #region Utility
        public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float time = 1f)
        {
            if (time <= 0f)
            {
                time = 1f;
                Debug.LogWarning("Tried to fade canvas group with a value for time that's less or equal to zero.");
            }

            while (canvasGroup.alpha != 0f)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
            canvasGroup.gameObject.SetActive(false);
        }

        public IEnumerator UnfadeCanvasGroup(CanvasGroup canvasGroup, float time = 1f)
        {
            canvasGroup.gameObject.SetActive(true);

            if (time <= 0f)
            {
                time = 1f;
                Debug.LogWarning("Tried to unfade canvas group with a value for time that's less or equal to zero.");
            }

            while (canvasGroup.alpha != 1f)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public void ShowLoadScreen(float time = 1f, bool hideOtherComponents = false)
        {
            if (time <= 0f)
            {
                time = 1f;
                Debug.LogWarning("Tried to show load screen with a value for time that's less or equal to zero.");
            }

            if (hideOtherComponents)
            {
                StartCoroutine(FadeCanvasGroup(BottomCanvasGroup, time));
                StartCoroutine(FadeCanvasGroup(ChoiceCanvasGroup, time));
            }
            StartCoroutine(UnfadeCanvasGroup(LoadingCanvasGroup, time));
        }

        public void HideLoadScreen(float time = 1f, bool showOtherComponents = false)
        {
            if (time <= 0f)
            {
                time = 1f;
                Debug.LogWarning("Tried to show load screen with a value for time that's less or equal to zero.");
            }

            if (showOtherComponents)
            {
                StartCoroutine(UnfadeCanvasGroup(BottomCanvasGroup, time));
                StartCoroutine(UnfadeCanvasGroup(ChoiceCanvasGroup, time));
            }
            StartCoroutine(FadeCanvasGroup(LoadingCanvasGroup, time));
        }
        #endregion

        #region Scenery
        public void ChangeBackground(Sprite newBackground)
        {
            BackgroundCanvasGroup.gameObject.SetActive(true);
            BackgroundImage.sprite = newBackground;
        }

        public void ChangeBackground(Sprite newBackground, float transitionTime)
        {
            BackgroundCanvasGroup.gameObject.SetActive(true);
            Color32 alpha = BackgroundFade.color;
            alpha.a = 255;
            BackgroundFade.color = alpha;

            BackgroundFade.sprite = BackgroundImage.sprite;
            BackgroundImage.sprite = newBackground;

            BackgroundFade.canvasRenderer.SetAlpha(1f);
            BackgroundFade.CrossFadeAlpha(0f, transitionTime, false);
        }

        public IEnumerator ScreenFadeTransition(CanvasGroup canvasGroup, Color mainColor, float fadeTime, float duration)
        {
            if (canvasGroup != null)
            {
                Image img = canvasGroup.GetComponent<Image>();
                float progress = 0;

                img.color = Color.white;
                img.material = Resources.Load<Material>("Fade");
                img.material.SetColor("_Color", mainColor);

                // DO FADE IN
                while (progress < 1f)
                {
                    progress += Time.deltaTime / fadeTime;
                    img.material.SetFloat("_Alpha", progress);
                    yield return new WaitForSeconds(0.02f);
                }

                // PAUSE
                yield return new WaitForSeconds(duration);

                // DO FADE OUT
                while (progress > 0)
                {
                    progress -= Time.deltaTime / fadeTime;
                    img.material.SetFloat("_Alpha", progress);
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }
        #endregion

        private void ApplyTextDisplayStylesToTMP(TextMeshProUGUI tmp, TextDisplayStyle[] displayStyles)
        {
            ResetTMP(tmp);
            for (int i = 0; i < displayStyles.Length; i++)
            {
                switch (displayStyles[i])
                {
                    case TextDisplayStyle.Gigantic:
                        tmp.fontSize = 40f;
                        break;
                    case TextDisplayStyle.Big:
                        tmp.fontSize = 25f;
                        break;
                    case TextDisplayStyle.Small:
                        tmp.fontSize = 12f;
                        break;
                    case TextDisplayStyle.Italic:
                        tmp.fontStyle = FontStyles.Italic;
                        break;
                    case TextDisplayStyle.Bold:
                        tmp.fontStyle = FontStyles.Bold;
                        break;
                    case TextDisplayStyle.Fast:
                        tempDisplayInterval = TextDisplayInterval * 3f;
                        break;
                    case TextDisplayStyle.Slow:
                        tempDisplayInterval = TextDisplayInterval / 2f;
                        break;
                }
            }
        }

        private void ResetTMP(TextMeshProUGUI tmp)
        {
            tmp.fontSize = 18f;
            tmp.fontStyle = 0;
            tmp.text = string.Empty;

            tempDisplayInterval = TextDisplayInterval;
        }

        private string ApplyTypography(string text, ref int textIndex)
        {
            if (text[textIndex] == '<')
            {
                string subString = text.Substring(textIndex);
                int endMark = subString.IndexOf('>');
                if (endMark < 0)
                    return text[textIndex].ToString();
                textIndex += endMark;
                return subString.Substring(0, endMark + 1);
            }
            else return text[textIndex].ToString();
        }

        public void Hide()
        {
            if (BackgroundCanvasGroup != null)
                BackgroundCanvasGroup.gameObject.SetActive(false);
            if (ChoiceCanvasGroup != null)
                ChoiceCanvasGroup.gameObject.SetActive(false);
            if (BottomCanvasGroup != null)
                BottomCanvasGroup.gameObject.SetActive(false);
            if (CharacterNamePlate != null)
                CharacterNamePlate.gameObject.SetActive(false);
        }

        public void Show()
        {
            if (BackgroundCanvasGroup != null)
                BackgroundCanvasGroup.gameObject.SetActive(true);
        }
    }
}
