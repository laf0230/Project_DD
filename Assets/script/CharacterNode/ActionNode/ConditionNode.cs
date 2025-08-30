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
            // 키 유효성 검사
            if (conditionData == null || string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"{nameof(ConditionNode)}: Condition data or key is missing.");
                return Status.Failure;
            }

            // 딕셔너리에 키가 없는 경우 방어
            if (!conditionData.conditionTable.ContainsKey(key))
            {
                Debug.LogWarning($"{nameof(ConditionNode)}: Key {key} not found in ConditionSO.");
                return Status.Failure;
            }

            // 값 가져오기
            bool satisfied = conditionData.GetValue(key);

            // true → Success, false → Failure
            return satisfied ? Status.Success : Status.Failure;
        }
    }
}
