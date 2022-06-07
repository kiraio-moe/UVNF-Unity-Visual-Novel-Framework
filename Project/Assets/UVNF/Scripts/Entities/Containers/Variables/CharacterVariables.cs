using System;
using UnityEngine;

namespace UVNF.Entities.Containers.Variables
{
    [Serializable]
    public class CharacterVariables
    {
        public string VariableName;
        public string CharacterName;
        public Color UniqueColor;

        public CharacterVariables(string variableName, string characterName, Color color)
        {
            VariableName = variableName;
            CharacterName = characterName;
            UniqueColor = color;
        }
    }
}
