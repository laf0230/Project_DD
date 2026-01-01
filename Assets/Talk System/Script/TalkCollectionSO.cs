using Assets.script.Talk_System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkCollectionSO", menuName = "Scriptable Objects/TalkCollectionSO")]
public class TalkCollectionSO : ScriptableObject
{
    [SerializeField] private List<TalkNode> talkList = new List<TalkNode>();
    public List<TalkNode> TalkNodes { get => talkList; set => talkList = value; }
}

//[CustomEditor(typeof(TalkCollectionSO))]
public class TalkCollectionSOEditor: Editor
{
    ReorderableList talkList;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        talkList = new ReorderableList(serializedObject, serializedObject.FindProperty("talkList"), true, true, true, true)
        {
            drawElementCallback = (rect, index, inActive, isFocus) =>
            {
                var element = talkList.serializedProperty.GetArrayElementAtIndex(index);

            }
        };
    }
}
