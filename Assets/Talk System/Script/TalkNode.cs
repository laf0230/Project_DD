using Assets.script.Talk_System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.script.Talk_System
{
    [System.Serializable]
    public class TalkNode
    {
        public string ID;
        public List<TalkLine> talkLineList;
        public List<TalkSelection> talkSelectionList;
    }
}
