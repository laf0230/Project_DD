using UnityEngine;

namespace ChainAction
{
    [CreateAssetMenu(menuName = "ActionNode/ConditionNode")]
    public class ConditionNode : Node
    {
        public string key;
        public ConditionSO conditionData;

        public override Status Process(GameObject gameObject)
        {
            if (conditionData == null)
            {
                Debug.Log("ConditionNode: conditionData is null");

                return Status.Failure;
            }

            if (!conditionData.conditionTable.TryGetValue(key, out bool condition))
            {
                condition = false;
                conditionData.conditionTable.Add(key, condition);
            }

            return condition ? Status.Success : Status.Failure;
        }
    }
}
