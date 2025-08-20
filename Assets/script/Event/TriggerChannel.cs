using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DD
{
    [System.Serializable]
    public class EventListener
    {
        [SerializeField] private List<GameEvent> eventList;
        public List<GameEvent> EventList { get => eventList; set => eventList = value; }

        public void OnTriggered(string name, int value)
        {
            var e = EventList.Find(o => o.Name == name);

            e.OnEventTriggerd?.Invoke();
        }

        public void StartTrack()
        {
            TriggerChannel.Subcribe(this);
        }

        public void EndTrack()
        {
            TriggerChannel.UnSubcribe(this);
        }

        public void AddEvent(List<GameEvent> events)
        {
            try
            {
                foreach (var e in events)
                {
                    AddEvent(events);
                }
            }
            catch
            {
                Debug.Log("Bug Exception");
            }
        }

        public void AddEvent(GameEvent e)
        {
            if (EventList.Contains(e))
            {
                Debug.Log($"{e.Name} Event is already contained");
                return;
            }
            EventList.Add(e);
        }

        public void RemoveEvent(GameEvent e)
        {
            if (!EventList.Contains(e))
            {
                Debug.Log($"{e.Name} Event is already Removed");
                return;
            }

            EventList.Remove(e);
        }
    }

    public static class TriggerChannel // 중개자
    {
        private static Trigger trigger = new();

        public static void Subcribe(EventListener listener) // 구독
        {
            if (listener.EventList == null) return;
            foreach (var evt in listener.EventList)
            {
                trigger.Register(evt);
            }
        }

        public static void UnSubcribe(EventListener listener) // 구독 취소
        {
            foreach(var evt in listener.EventList)
            {
                trigger.UnRegister(evt);
            }
        }

        public static void UpdateTrigger(ComprisonCondition condition)
        {
            trigger.UpdateTriggerValue(condition);
        }
    }

    [System.Serializable]
    public class EventHandler
    {
        public List<ComprisonCondition> handleDataList = new();

        public void UpdateTrigger(ComprisonCondition condition)
        {
            foreach (var cond in handleDataList)
            {
                if(cond.Name == condition.Name)
                {
                    cond.Value = condition.Value;
                    TriggerChannel.UpdateTrigger(cond);
                }
            }
        }
    }
}
