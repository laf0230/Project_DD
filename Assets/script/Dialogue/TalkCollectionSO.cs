using Assets.script.Talk_System;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "대화 데이터", menuName = "대화 시스템/데이터 SO")]
public class DialogueDataSO : ScriptableObject
{
    [Header("변환 설정")]
    [Tooltip("헤더 제외, 실제 데이터 시작 전 건너뛸 행 수")]
    public int startRowIndex = 0;

    [Header("원본 CSV 데이터")]
    public TextAsset dialogueTable;
    public TextAsset selectionTable;

    [SerializeField] private List<DialogueNode> talkList = new List<DialogueNode>();
    public List<DialogueNode> TalkNodes => talkList;

    public DialogueNode GetDialogue(string id)
    {
        DialogueNode result = null;
        foreach (DialogueNode info in talkList)
        {
            if (info.id == id)
            {
                result = info;
            }
        }

        if (result == null)
            throw new NullReferenceException($"[TalkCollectionSO] 해당 ID가 존재하지 않습니다. \n ID={id}");

        return result;
    }

    [ContextMenu("데이터 변환하기")]
    public void ParseData()
    {
        List<DialogueTable> dialogueData = LoadDialogueTableCsv<DialogueTable>(dialogueTable.text);
        List<SelectionTable> selectionData = LoadDialogueTableCsv<SelectionTable>(selectionTable.text);


        talkList = dialogueData
            .GroupBy(d => d.DialogueId)
            .Select(group => new DialogueNode
            {
                id = group.Key,
                talkLineList = group.OrderBy(g => g.Order)
                .Select(line => new DialogueLine
                {
                    id = line.Line,
                    text = line.Line,
                    speakerData = new DialogueCharacter { name = line.Name, id = line.LeftCharacterImage },
                    otherSpeakerData = new DialogueCharacter { id = line.RightCharacterImage }
                })
                .ToList(),

                talkSelectionList = selectionData
                .Where(s => s.DialogueId == group.Key)
                .Select(sel => new DialogueSelection
                {
                    text = sel.Content,
                    eventName = new List<string> { sel.EventId },
                    selectionType = SelectionType.Normal
                })
                .ToList()
            })
            .ToList();
    }

    public List<T> LoadDialogueTableCsv<T>(string csvText)
    {
        // TextAsset의 텍스트를 StringReader로 변환하여 CsvHelper에 전달
        using (var reader = new StringReader(csvText))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            for(int i = 0; i < startRowIndex; i++)
            {
                csv.Read();
            }
            csv.Read();
            csv.ReadHeader();

            return csv.GetRecords<T>().ToList();
        }
    }
}

[CustomPropertyDrawer(typeof(DialogueLine))]
public class DialogueLineDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 내부 속성들을 가져옵니다.
        SerializedProperty speakerData = property.FindPropertyRelative("speakerData");
        SerializedProperty speakerName = speakerData.FindPropertyRelative("name");
        SerializedProperty text = property.FindPropertyRelative("text");

        // 표시할 문자열 생성 (예: "홍길동: 안녕하세요")
        string nameValue = string.IsNullOrEmpty(speakerName.stringValue) ? "Unknown" : speakerName.stringValue;
        string textValue = text.stringValue.Replace("\n", " "); // 줄바꿈은 한 칸 공백으로 치환

        string customLabel = $"{nameValue}: {textValue}";

        // 기본 PropertyDrawer 호출 시 레이블을 우리가 만든 커스텀 레이블로 교체
        EditorGUI.PropertyField(position, property, new GUIContent(customLabel), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 접혀있을 때와 펼쳐져 있을 때의 높이를 자동으로 계산
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}

//[CustomEditor(typeof(DialogueDataSO))]
public class DialogueDataSOEditor: Editor
{
    public override void OnInspectorGUI()
    {
        var list = serializedObject.FindProperty("talkList");

        ShowArrayProperty(list);
    }

    public void ShowArrayProperty(SerializedProperty list)
    {
        var talkLineList = list.FindPropertyRelative("talkLineList");

        EditorGUILayout.PropertyField(list);

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
            new GUIContent("Bla" + (i + 1).ToString()));
        }
        EditorGUI.indentLevel -= 1;
    }
}

[System.Serializable]
public class DialogueTable
{
    [Name("DialogueId")]
    [Default("0")]
    public string DialogueId { get; set; }
    [Name("Order")]
    [Default(0)]
    public int Order {  get; set; }
    [Name("ID")]
    [Default("0")]
    public string Id {  get; set; }
    [Name("Name")]
    [Default("0")]
    public string Name {  get; set; }
    [Name("CharacterLeftImageID")]
    [Default("0")]
    public string LeftCharacterImage {  get; set; }
    [Name("CharacterRightImageID")]
    [Default("0")]
    public string RightCharacterImage {  get; set; }
    [Name("Line")]
    [Default("0")]
    public string Line {  get; set; }
 }

[System.Serializable]
public class SelectionTable
{
    [Name("DialogueId")]
    [Default("0")]
    public string DialogueId { get; set; }
    [Name("Id")]
    [Default("0")]
    public string Id { get; set; }
    [Name("Content")]
    [Default("0")]
    public string Content { get; set; }
    [Name("EventId")]
    [Default("0")]
    public string EventId {  get; set; }
}
