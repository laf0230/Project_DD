using UnityEngine;
using ChainAction;
using System.Linq;



#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ConditionSO))]
public class ConditionSOEditor : Editor
{
    private string newKey = "";
    private bool newValue = false;

    public override void OnInspectorGUI()
    {
        ConditionSO conditionSO = (ConditionSO)target;

        // 딕셔너리 전체 표시
        if (conditionSO.conditionTable != null)
        {
            foreach (var kvp in conditionSO.conditionTable.ToList()) // ToList() 안 하면 foreach 중 수정시 에러
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(kvp.Key, GUILayout.MaxWidth(150));
                bool newBool = EditorGUILayout.Toggle(kvp.Value);

                if (newBool != kvp.Value)
                {
                    conditionSO.conditionTable[kvp.Key] = newBool;
                    EditorUtility.SetDirty(conditionSO);
                }

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    conditionSO.conditionTable.Remove(kvp.Key);
                    EditorUtility.SetDirty(conditionSO);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);

        // 새로운 Key-Value 추가 UI
        EditorGUILayout.LabelField("Add new condition", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        newKey = EditorGUILayout.TextField(newKey);
        newValue = EditorGUILayout.Toggle(newValue);

        if (GUILayout.Button("Add"))
        {
            if (!string.IsNullOrEmpty(newKey) && !conditionSO.conditionTable.ContainsKey(newKey))
            {
                conditionSO.conditionTable.Add(newKey, newValue);
                newKey = "";
                newValue = false;
                EditorUtility.SetDirty(conditionSO);
            }
            else
            {
                Debug.LogWarning("Key already exists or invalid!");
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif
