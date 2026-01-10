using Assets.script.Talk_System;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

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
