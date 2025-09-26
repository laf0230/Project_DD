using UnityEngine;
using UnityEditor;
using Inference;

[CustomEditor(typeof(ClueDataStroage))]
public class ClueDataStroageEditor : Editor
{
    private SerializedProperty savePathProp;
    private SerializedProperty fileNameProp;
    private SerializedProperty isForBuildProp;
    private SerializedProperty cluesProp;
    private SerializedProperty recipesProp;

    private void OnEnable()
    {
        // SerializedProperty로 각 필드 가져오기
        savePathProp = serializedObject.FindProperty("savePath");
        fileNameProp = serializedObject.FindProperty("fileName");
        isForBuildProp = serializedObject.FindProperty("isForBuild");
        cluesProp = serializedObject.FindProperty("clues");
        recipesProp = serializedObject.FindProperty("recipes");
    }

    public override void OnInspectorGUI()
    {
        // 데이터 동기화 시작
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Clue Data Storage Editor", EditorStyles.boldLabel);

        // 기본 필드 표시
        EditorGUILayout.PropertyField(savePathProp);
        EditorGUILayout.PropertyField(fileNameProp);
        EditorGUILayout.PropertyField(isForBuildProp);

        EditorGUILayout.Space();

        // Clues 배열 표시
        EditorGUILayout.PropertyField(cluesProp, new GUIContent("Clues"), true);

        EditorGUILayout.Space();

        // Recipes 배열 표시
        EditorGUILayout.PropertyField(recipesProp, new GUIContent("Recipes"), true);

        // 변경사항 적용
        serializedObject.ApplyModifiedProperties();
    }
}

