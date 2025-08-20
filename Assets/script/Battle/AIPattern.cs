using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AIActionPattern
{
    Peak,
    rush,
    cast
}

[CreateAssetMenu(fileName = "PatternsSO")]
public class AIPatternListSO: ScriptableObject
{
    public List<AIPattern> patternList;
}

[System.Serializable]
public class AIPattern
{
    public AIActionPattern pattern;
    public UnityEvent patternAction;
}
