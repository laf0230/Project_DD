using DD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompareType
{
    None,
    Equal,
    Greater,
    Less
}

[System.Serializable]
public class Condition
{
    [SerializeField] protected string name;
    [SerializeField] protected int value;
    protected bool isSatisfied { get; set; } = false;

    public string Name { get => name; set => name = value; }
    public int Value { get => value; set => this.value = value; }
    public bool IsSatisfied { get => isSatisfied; set => isSatisfied = value; }

    public  Condition() { }

    public Condition(string name, int value)
    {
        this.name = name;
        this.value = value;
    }

    public override bool Equals(object obj) => 
        obj is Condition c && name == c.name && value == c.value;

    public override int GetHashCode() =>
        HashCode.Combine(name, value);
}

[System.Serializable]
public class ComprisonCondition: Condition
{
    [SerializeField] private CompareType compareType;
    public CompareType CompareType { get => compareType; set => compareType = value; }
    public ComprisonCondition() : base() { }
    public ComprisonCondition(string name, int value, CompareType compareType = default) : base()
    {
        this.name = name;
        this.value = value;
        CompareType = compareType;
    }

    public override bool Equals(object obj) => 
        base.Equals(obj) && obj is ComprisonCondition c && c.compareType == compareType;

    public override int GetHashCode() =>
        HashCode.Combine(base.GetHashCode(), compareType);
}

[System.Serializable]
public class Trigger
{
    [SerializeField] private Dictionary<ComprisonCondition, List<GameEvent>> gameEventMap = new Dictionary<ComprisonCondition, List<GameEvent>>();
    public Dictionary<ComprisonCondition, List<GameEvent>> GameEventMap { get => gameEventMap; set => gameEventMap = value; }

    internal void Register(GameEvent gameEvent)
    {
        // 이벤트 추가 로직
        foreach (var cond in gameEvent.ConditionList)
        {
            Debug.Log($"Regested Condition: {cond.Name}");
            if (!gameEventMap.TryGetValue(cond, out List<GameEvent> eventList))
            {
                // 조건이 존재하지 않을 경우 조건에 대응하는 새로운 이벤트 목록 생성
                eventList = new List<GameEvent>();
                gameEventMap[cond] = eventList;
            }
            // 이후 조건에 대응하는 이벤트 목록에 이벤트를 추가함
            eventList.Add(gameEvent);
        }
    }

    public void OnConditionChanged(ComprisonCondition cond)
    {
        if (gameEventMap.TryGetValue(cond, out List<GameEvent> events)) {
            foreach (var evt in events)
            {
                evt.UpdateCondition(cond.Name, cond.Value);
            }
        }
    }

    public void UnRegister(GameEvent gameEvent)
    {
        foreach(var condition in gameEvent.ConditionList)
        {
            // condition
            gameEventMap[condition].Remove(gameEvent);
        }

        foreach(var condition in gameEvent.ConditionList)
        {
            GameEventMap[condition].Remove(gameEvent);
        }
    }

    public void UpdateTriggerValue(ComprisonCondition condition)
    {
        // check value from condition, find matched condition name from gameEventMap
        // if it matched, compare with condition and it is sucessed it will call invoke
        foreach(var cond in GameEventMap.Keys)
        {
            if(cond.Name == condition.Name)
            {
                Debug.Log($"Updated Condition: {condition.Name}");

                foreach (var gameEvent in gameEventMap[cond])
                {
                    Debug.Log($"Try Update That Event Condition: {gameEvent.Name}");
                    gameEvent.UpdateCondition(condition.Name, condition.Value);
                }
            }
            else
            {
                Debug.LogError($"{condition.Name} is not exist condition.");
            }
        }
    }
}
