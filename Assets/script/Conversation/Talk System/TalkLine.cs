using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.script.Talk_System
{
    [System.Serializable]
    public class EmotionData
    {
        public string name;
        public string emotionName = "Normal";
    }

    // 대화 시 보여줄 텍스트
    [System.Serializable]
    public class TalkLine
    {
        public EmotionData speakerData;
        public List<EmotionData> otherDataList;
        public string talkID;
        [TextArea] public string talkText;
    }

    // 대화 마지막에 보여줄 선택지
    [System.Serializable]
    public  class TalkSelection 
    {
        public string selectionText;
        public UnityEvent action;
    }
}
