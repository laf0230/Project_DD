using System.Collections.Generic;
using UnityEngine;

namespace Assets.script.Talk_System
{
    // 캐릭터 정보
    [System.Serializable]
    public class DialogueCharacter
    {
        // Displayed Data
        public string name;
        public string id;
    }
    
    [System.Serializable]
    public class DialogueNode
    {
        public string id;
        public List<DialogueLine> talkLineList;
        public List<DialogueSelection> talkSelectionList;
    }

    // 대화 시 보여줄 텍스트
    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter speakerData;
        public DialogueCharacter otherSpeakerData;
        public string id;
        [TextArea] public string text;
    }

    // 대화 마지막에 보여줄 선택지
    [System.Serializable]
    public class DialogueSelection 
    {
        public string text;
        public List<string> eventName;
        public SelectionType selectionType;
    }

    public enum SelectionType
    {
        Normal,
        Battle,
        Investigation
    }
}
