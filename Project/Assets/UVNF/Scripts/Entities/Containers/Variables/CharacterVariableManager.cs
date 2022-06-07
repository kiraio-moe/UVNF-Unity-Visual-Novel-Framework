using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UVNF.Core.Serialization;

namespace UVNF.Entities.Containers.Variables
{
    [CreateAssetMenu(fileName = "Characters", menuName = "UVNF/Character Variable")]
    public class CharacterVariableManager : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<CharacterVariables> CharaVariables = new List<CharacterVariables>();
        public List<SerializedData> VariableValues = new List<SerializedData>();

        public void AddVariable()
        {
            CharaVariables.Add(new CharacterVariables("Velorexe_1", "Velorexe", new Color(75, 75, 75, 255)));
        }

        public string[] CharactersList()
        {
            return CharaVariables.Select(x => x.VariableName).ToArray();
        }

        #region Serialization
        public void OnBeforeSerialize()
        {
            VariableValues.Clear();

            for (int i = 0; i < CharaVariables.Count; i++)
            {
                VariableValues.Add(SerializedData.Serialize(CharaVariables[i]));
            }
        }

        public void OnAfterDeserialize()
        {
            CharaVariables.Clear();

            for (int i = 0; i < VariableValues.Count; i++)
            {
                CharaVariables.Add(SerializedData.Deserialize(VariableValues[i]) as CharacterVariables);
            }
        }
        #endregion
    }
}
