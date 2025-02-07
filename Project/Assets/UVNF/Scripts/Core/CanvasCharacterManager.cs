﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UVNF.Core.Story.Character;
using UVNF.Entities;

namespace UVNF.Core
{
    public class CanvasCharacterManager : MonoBehaviour
    {
        public List<Character> CharactersOnScreen;
        public RectTransform MainCharacterStack;

        private Character[] LeftSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Left).ToArray(); } }
        private Character[] MiddleSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Middle).ToArray(); } }
        private Character[] RightSideCharacters { get { return CharactersOnScreen.Where(x => x.CurrentPosition == ScenePositions.Right).ToArray(); } }

#region Add Character to Screen
        public void AddCharacter(string characterName, Sprite characterSprite, bool flip, ScenePositions enter, ScenePositions position, float enterTime)
        {
            MainCharacterStack.gameObject.SetActive(true);

            GameObject obj = new GameObject(characterSprite.name, typeof(RectTransform));
            RectTransform parentTransform;

            Image spriteRenderer = obj.AddComponent<Image>();
            spriteRenderer.sprite = characterSprite;
            spriteRenderer.preserveAspect = true;

            RectTransform spriteTransform = obj.GetComponent<RectTransform>();
            obj.transform.SetParent(MainCharacterStack);
            parentTransform = MainCharacterStack;
            spriteTransform.sizeDelta = MainCharacterStack.sizeDelta;

            if (flip)
                spriteTransform.localScale = new Vector3(-1, 1, 1);
            else
                spriteTransform.localScale = new Vector3(1, 1, 1);

            Character character = obj.AddComponent<Character>();
            character.Name = characterName;
            character.Transform = spriteTransform;
            character.Parent = parentTransform;
            character.CurrentPosition = position;
            character.SpriteRenderer = spriteRenderer;

            float multiplier = characterSprite.rect.height / spriteTransform.rect.height;
            spriteTransform.sizeDelta = new Vector2(characterSprite.rect.width / multiplier, spriteTransform.sizeDelta.y);

            Vector3 startPosition = new Vector3();
            switch (enter)
            {
                case ScenePositions.Left:
                    startPosition = new Vector3(-parentTransform.sizeDelta.x - spriteTransform.sizeDelta.x / 2, 0, 0);
                    break;
                case ScenePositions.Top:
                    startPosition = new Vector3(0, parentTransform.sizeDelta.y + spriteTransform.sizeDelta.y / 2, 0);
                    break;
                case ScenePositions.Right:
                    startPosition = new Vector3(parentTransform.sizeDelta.x + spriteTransform.sizeDelta.x / 2, 0, 0);
                    break;
            }

            spriteTransform.anchoredPosition = startPosition;
            CharactersOnScreen.Add(character);

            Vector3 endPosition = new Vector3();
            switch (position)
            {
                case ScenePositions.Left:
                    endPosition = new Vector3(-(parentTransform.sizeDelta.x / 2), 0, 0);

                    Character[] leftCharacters = LeftSideCharacters.Reverse().ToArray();
                    if (leftCharacters.Length > 1)
                    {
                        float leftPosition = Mathf.Abs(parentTransform.sizeDelta.x);
                        float offset = leftPosition / (leftCharacters.Length + 1);
                        for (int i = 0; i < leftCharacters.Length; i++)
                        {
                            Vector3 newPosition = new Vector3(-parentTransform.sizeDelta.x + offset * (i + 1), 0, 0);
                            leftCharacters[i].MoveCharacter(newPosition, 1f);
                        }
                    }
                    else
                    {
                        character.MoveCharacter(endPosition, enterTime);
                    }
                    break;
                case ScenePositions.Top:
                    endPosition = new Vector3(0, 0, 0);
                    character.MoveCharacter(endPosition, enterTime);
                    break;
                case ScenePositions.Middle:
                    endPosition = new Vector3(0, 0, 0);
                    character.MoveCharacter(endPosition, enterTime);
                    break;
                case ScenePositions.Right:
                    endPosition = new Vector3(parentTransform.sizeDelta.x / 2, 0, 0);

                    Character[] rightCharacters = RightSideCharacters;
                    if (rightCharacters.Length > 1)
                    {
                        float rightPosition = Mathf.Abs(parentTransform.sizeDelta.x);
                        float offset = rightPosition / (rightCharacters.Length + 1);
                        for (int i = 0; i < rightCharacters.Length; i++)
                        {
                            Vector3 newPosition = new Vector3(offset * (i + 1), 0, 0);
                            rightCharacters[i].MoveCharacter(newPosition, 1f);
                        }
                    }
                    else
                    {
                        character.MoveCharacter(endPosition, enterTime);
                    }
                    break;
            }
        }
#endregion

#region Remove Character from Screen
        public void RemoveCharacter(string characterName, ScenePositions exitPosition, float exitTime)
        {
            Character character = CharactersOnScreen.Find(x => x.Name == characterName);

            Vector2 endPosition = new Vector2();

            switch (exitPosition)
            {
                case ScenePositions.Left:
                    endPosition = new Vector2(-(character.Parent.rect.width + (character.Transform.rect.width)), 0);
                    break;
                case ScenePositions.Top:
                    endPosition = new Vector2(0, character.Transform.rect.height);
                    break;
                case ScenePositions.Right:
                    endPosition = new Vector2(character.Parent.rect.width + (character.Transform.rect.width), 0);
                    break;
            }

            CharactersOnScreen.Remove(character);
            character.MoveCharacter(endPosition, exitTime);
        }
#endregion

#region Move Character to Another Character Position
        public void MoveCharacterTo(string characterName, string characterToMoveTo, float moveTime)
        {
            Character mainCharacter = CharactersOnScreen.Find(x => x.Name == characterName);
            Character moveToCharacter = CharactersOnScreen.Find(x => x.Name == characterToMoveTo);

            mainCharacter.MoveCharacter(moveToCharacter.Transform.anchoredPosition, moveTime);
        }
#endregion

#region Change Character Sprite
        public void ChangeCharacterSprite(string characterName, Sprite characterSprite, bool flip)
        {
            Character character = CharactersOnScreen.Find(x => x.Name == characterName);
            character.ChangeSprite(characterSprite);
            RectTransform spriteTransform = character.GetComponent<RectTransform>();

            if (flip)
            {
                switch (spriteTransform.localScale.x)
                {
                    case -1:
                        spriteTransform.localScale = new Vector3(1, 1, 1);
                        break;
                    case 1:
                        spriteTransform.localScale = new Vector3(-1, 1, 1);
                        break;
                }
            }
        }
#endregion

#region Hide Character
        public void Hide()
        {
            MainCharacterStack.gameObject.SetActive(false);
        }
#endregion        
    }
}
