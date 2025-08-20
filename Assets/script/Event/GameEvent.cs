using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DD
{
    [System.Serializable]
    public class GameEvent
    {
        [SerializeField] private string name;
        [SerializeField] private List<ComprisonCondition> conditionList;
        [SerializeField] private UnityEvent onEventTriggered;

        public string Name { get => name; set => name = value; }
        public List<ComprisonCondition> ConditionList { get => conditionList; set => conditionList = value; }
        public UnityEvent OnEventTriggerd { get => OnEventTriggerd; set => OnEventTriggerd = value; }

        public GameEvent(GameEvent gameEvent)
        {
            this.name = gameEvent.name;
            this.conditionList = gameEvent.ConditionList;
            onEventTriggered = gameEvent.onEventTriggered;
        }

        public GameEvent(string name, List<ComprisonCondition> conditionList, UnityEvent onEventTriggered)
        {
            this.name = name;
            this.onEventTriggered = onEventTriggered;
        }

        public void AddCondition(ComprisonCondition condition)
        {
            conditionList.Add(condition);
            UpdateCondition(condition.Name, condition.Value);
        }

        public void RemoveCondition(ComprisonCondition condition)
        {
            conditionList.Remove(condition);
        }

        public List<ComprisonCondition> GetConditions() => conditionList;

        public void UpdateCondition(string k, int v)
        {
            bool conditionUpdated = false;

            // 1. 미완료 조건에서 검사
            for (int i = conditionList.Count - 1; i >= 0; i--)
            {
                var cond = conditionList[i];

                if (cond.Name == k)
                {
                    bool result = EvaluateComparison(cond, v);
                    cond.IsSatisfied = result;

                    if (result)
                    {
                        conditionUpdated = true;
                    }
                }
            }

            // 3. 모든 조건이 충족되었을 때 Trigger
            if (conditionUpdated)
            {
                TryExecuteTrigger();
            }
        }

        private bool EvaluateComparison(ComprisonCondition inputValue,  int targetValue)
        {
            return inputValue.CompareType switch
            {
                CompareType.Equal => inputValue.Value == targetValue,
                CompareType.Greater => inputValue.Value < targetValue,
                CompareType.Less => inputValue.Value > targetValue,
                _ => false
            };
        }

        public void TryExecuteTrigger()
        {
            if(conditionList.All(c=>c.IsSatisfied == true))
            {
                Trigger();
            }
        }

        private void Trigger() => onEventTriggered?.Invoke();
    }
}
