using System;
using UnityEngine.Events;

namespace GamEventNode
{
    [Serializable]
    public class GameNode
    {
        public string _EventName;
        public UnityEvent _Event;
    }

}