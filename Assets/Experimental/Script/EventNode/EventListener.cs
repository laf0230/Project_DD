using UnityEngine;

namespace GamEventNode
{
    public class EventListener : MonoBehaviour
    {
        public string eventName;

        public void Invoke()
        {
            GameEventBoard.Invoke(eventName);
        }
    }

}