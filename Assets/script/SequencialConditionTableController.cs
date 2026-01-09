using UnityEngine;

public class SequencialConditionTableController : MonoBehaviour
{
    [Header("필수")]
    public ConditionSO conditionTableSO; 
    public string conditionName;
    public bool targetConditionValue;

    public void UpdateCondition()
    {
        conditionTableSO.SetValue(conditionName, targetConditionValue);
    }
}
