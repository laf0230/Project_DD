using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data/ConditionData")]
public class ConditionSO : ScriptableObject
{
    public Dictionary<string, bool> conditionTable = new();

    public void AddKey(string key, bool value) => conditionTable.Add(key, value);

    public void SetValue(string key, bool value) => conditionTable[key] = value;

    public bool GetValue(string key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        return conditionTable[key];
    }

    public void Add(string key, bool value) => conditionTable[key] = value;
}