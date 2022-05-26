using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using UVNF.Core.UI;
using UVNF.Entities.Containers.Variables;
using UVNF.Extensions;

namespace UVNF.Core.Story.Dialogue
{
    public class ConditionElement : StoryElement
    {
        public override string ElementName => "Condition";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Story();

        public override StoryElementTypes Type => StoryElementTypes.Story;

        [HideInInspector]
        [Output(ShowBackingValue.Never, ConnectionType.Multiple)] public NodePort PassNode;
        [HideInInspector]
        [Output(ShowBackingValue.Never, ConnectionType.Multiple)] public NodePort FailNode;

        ConditionalOperators.NumberOperators NumberOperators;
        ConditionalOperators.StringOperators StringOperators;

        public VariableManager Variables;
        int VariableIndex = 0;

        public float NumberValue = 0f;
        public string TextValue = string.Empty;
        public bool BooleanValue = false;

        string[] booleanOptions = new string[] { "False", "True" };

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            Variables = EditorGUILayout.ObjectField("Variables", Variables, typeof(VariableManager), false) as VariableManager;
            if (Variables != null && Variables.Variables.Count > 0)
            {
                VariableIndex = EditorGUILayout.Popup("Variable", VariableIndex, Variables.VariableNames());

                switch (Variables.Variables[VariableIndex].ValueType)
                {
                    case VariableTypes.Number:
                        NumberOperators = (ConditionalOperators.NumberOperators)EditorGUILayout.EnumPopup("Action", NumberOperators);
                        NumberValue = EditorGUILayout.FloatField("Value", NumberValue);
                        break;
                    case VariableTypes.String:
                        StringOperators = (ConditionalOperators.StringOperators)EditorGUILayout.EnumPopup("Action", StringOperators);
                        TextValue = EditorGUILayout.TextField("Value", TextValue);
                        break;
                    case VariableTypes.Boolean:
                        BooleanValue = System.Convert.ToBoolean(EditorGUILayout.Popup("Value", System.Convert.ToInt32(BooleanValue), booleanOptions));
                        break;
                }
            }
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            bool conditionTrue = false;
            switch (Variables.Variables[VariableIndex].ValueType)
            {
                case VariableTypes.Boolean:
                    conditionTrue = BooleanValue == Variables.Variables[VariableIndex].BooleanValue;
                    break;
                case VariableTypes.Number:
                    switch (NumberOperators)
                    {
                        case ConditionalOperators.NumberOperators.Equal:
                            conditionTrue = NumberValue == Variables.Variables[VariableIndex].NumberValue;
                            break;
                        case ConditionalOperators.NumberOperators.NotEqual:
                            conditionTrue = NumberValue != Variables.Variables[VariableIndex].NumberValue;
                            break;
                        case ConditionalOperators.NumberOperators.Greater:
                            conditionTrue = NumberValue > Variables.Variables[VariableIndex].NumberValue;
                            break;
                        case ConditionalOperators.NumberOperators.Less:
                            conditionTrue = NumberValue < Variables.Variables[VariableIndex].NumberValue;
                            break;
                        case ConditionalOperators.NumberOperators.GreaterEqual:
                            conditionTrue = NumberValue >= Variables.Variables[VariableIndex].NumberValue;
                            break;
                        case ConditionalOperators.NumberOperators.LessEqual:
                            conditionTrue = NumberValue <= Variables.Variables[VariableIndex].NumberValue;
                            break;
                    }
                    break;
                case VariableTypes.String:
                    switch (StringOperators)
                    {
                        case ConditionalOperators.StringOperators.Equal:
                            conditionTrue = TextValue == Variables.Variables[VariableIndex].TextValue;
                            break;
                        case ConditionalOperators.StringOperators.NotEqual:
                            conditionTrue = TextValue != Variables.Variables[VariableIndex].TextValue;
                            break;
                    }
                    break;
            }

            if (conditionTrue)
            {
                managerCallback.AdvanceStoryGraph(GetOutputPort("PassNode").Connection.node as StoryElement);
            }
            else
            {
                managerCallback.AdvanceStoryGraph(GetOutputPort("FailNode").Connection.node as StoryElement);
            }
            yield return null;
        }
    }
}
