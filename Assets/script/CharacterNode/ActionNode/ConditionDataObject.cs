using UnityEngine;

public class ConditionDataObject : MonoBehaviour
{
    public ConditionSO ConditionSO;

    public void AddData(string key, bool value = false)
    {
        ConditionSO.Add(key, value);
    }

    public void UpdateData(string key, bool value)
    {
        ConditionSO.SetValue(key, value);
    }
}
