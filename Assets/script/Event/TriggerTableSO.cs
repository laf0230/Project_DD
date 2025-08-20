using DD;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "TriggerTableSO", menuName = "Scriptable Objects/TriggerTableSO")]
public class TriggerTableSO : ScriptableObject // 각 오브젝트에 할당되어 이벤트 목록이 됨
{
    [SerializeField] private List<GameEvent> eventList = new();

    public List<GameEvent> GetEventList()
    {
        return eventList;
    }

    public GameEvent GetEvent(string name)
    {
        for (int i = 0; i < eventList.Count; i++)
        {
            if(eventList[i].Name == name)
            {
                return eventList[i];
            }
        }

        Debug.Log($"{name} event is not exist");
        return null;
    }
}
