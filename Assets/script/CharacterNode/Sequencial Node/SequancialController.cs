using ChainAction;
using UnityEngine;

public class SequancialController : MonoBehaviour
{
    [SerializeField] ChainAction.SequenceNode node;
    [SerializeField] ConditionSO conditionTable;

    private void Update()
    {
        conditionTable.ProcessPendingChanges();
        node?.Process(gameObject);
        conditionTable.PostProcessChanges();
    }
}
