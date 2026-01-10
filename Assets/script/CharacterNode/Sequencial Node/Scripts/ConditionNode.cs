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
                // 초기값 설정
                condition = false;
                conditionData.conditionTable.Add(key, condition);
            }

            return condition ? Status.Success : Status.Running;

            // if it's Behaviour tree, condition has succes, return running value and run childrun
            /*
            int currentChild;
            var childs = new List<Node>();

            while(currentChild < childs.Count)
            {
                switch(childs[currentChild].Process())
                {
                    case Status.Success:
                        currentChild++;
                        return Status.Running;
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        currentChild++;
                        return Status.Running;
                }
            }
            return Status.Success;
        }
            */
        }
    }
}
