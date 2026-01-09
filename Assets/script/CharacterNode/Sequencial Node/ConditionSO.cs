using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/ConditionData")]
public class ConditionSO : ScriptableObject
{
    public Dictionary<string, bool> conditionTable = new();
    // 변경 예약 리스트
    private Dictionary<string, bool> pendingChanges = new();

    public void SetValue(string key, bool value) => conditionTable[key] = value;
    public bool GetValue(string key) => conditionTable.ContainsKey(key) && conditionTable[key];

    private void OnEnable()
    {
        foreach (var condition in conditionTable)
        {
            conditionTable[condition.Key] = false;
        }
        pendingChanges = new Dictionary<string, bool>();
    }

    // 대화 종료 시 호출: 즉시 바꾸지 않고 '예약'만 함
    public void SetValueDeferred(string key, bool value)
    {
        pendingChanges[key] = value;
    }

    // 캐릭터의 Update() 혹은 AI Tick() 시작 시점에 호출
    public void ProcessPendingChanges()
    {
        foreach (var change in pendingChanges)
        {
            conditionTable[change.Key] = change.Value;
        }
    }

    // 캐릭터의 행동 갱신이 끝난 후 호출 (휘발성 데이터 청소)
    public void PostProcessChanges()
    {
        // 일회성 트리거들은 여기서 다시 false로 변경
        // 혹은 pendingChanges에 있던 키들만 골라서 초기화
        foreach (var key in pendingChanges.Keys.ToList())
        {
            conditionTable[key] = false;
        }
        pendingChanges.Clear();
    }
}