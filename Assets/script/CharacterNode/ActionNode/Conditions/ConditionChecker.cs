using UnityEngine;

public class ConditionChecker : MonoBehaviour
{
    [SerializeField] ConditionSO conditionTable;

    [SerializeField] string key;

    public void ChangeState(bool value)
    {
        conditionTable.conditionTable[key] = value;
    }
}
