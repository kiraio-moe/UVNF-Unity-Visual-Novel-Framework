using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UVNF.Entities.Containers.Variables;

namespace UVNF.Editor
{
    public class DefineCharacterEditor : EditorWindow
    {
        public CharacterVariableManager CharacterManager;
        Vector2 scrollPosition = new Vector2();
        int selectedIndex = -1;

        [MenuItem("UVNF/Define Characters")]
        public static void Init()
        {
            DefineCharacterEditor window = (DefineCharacterEditor)GetWindow(typeof(DefineCharacterEditor), false, "Define Characters");
            window.Show();
        }

        void OnGUI()
        {
            if (CharacterManager == null)
            {
                EditorGUILayout.HelpBox("Please assign a Character Variable Manager or you can create one by right click > Create > UVNF > Variable Manager.", MessageType.Warning);
                GUILayout.Space(8);
            }

            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal("Box");
                {
                    CharacterManager = EditorGUILayout.ObjectField("Variable", CharacterManager, typeof(CharacterVariableManager), false) as CharacterVariableManager;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (CharacterManager != null)
                    {
                        EditorUtility.SetDirty(CharacterManager);
                        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(200f));
                        {
                            for (int i = 0; i < CharacterManager.CharaVariables.Count; i++)
                            {
                                GUI.SetNextControlName("ButtonFocus");
                                if (GUILayout.Button(CharacterManager.CharaVariables[i].CharacterName))
                                {
                                    selectedIndex = i;
                                    GUI.FocusControl("ButtonFocus");
                                }
                            }

                            GUILayout.BeginHorizontal();
                            {
                                if (GUILayout.Button("+"))
                                {
                                    CharacterManager.AddVariable();
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.BeginVertical();
                    {
                        if (selectedIndex > -1 && selectedIndex < CharacterManager.CharaVariables.Count)
                        {
                            CharacterManager.CharaVariables[selectedIndex].VariableName = EditorGUILayout.TextField("Variable Name", CharacterManager.CharaVariables[selectedIndex].VariableName);
                            CharacterManager.CharaVariables[selectedIndex].CharacterName = EditorGUILayout.TextField("Character Name", CharacterManager.CharaVariables[selectedIndex].CharacterName);
                            CharacterManager.CharaVariables[selectedIndex].UniqueColor = EditorGUILayout.ColorField("Unique Color", CharacterManager.CharaVariables[selectedIndex].UniqueColor);

                            if (GUILayout.Button("-"))
                            {
                                CharacterManager.CharaVariables.RemoveAt(selectedIndex);
                                selectedIndex = -1;
                            }
                        }
                        else
                        {
                            selectedIndex = -1;
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}
