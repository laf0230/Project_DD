using UnityEngine;
using System.Collections.Generic;
using DD;

public class SampleObject : MonoBehaviour
{
    public TriggerTableSO so;
    public List<GameEvent> eventList = new();

    public EventListener listener;

    private void Awake()
    {
        var eventList = so.GetEventList();
    }

    public void Interect(Player player)
    {
        
    }
}
