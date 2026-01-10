using System.Collections.Generic;
using UnityEngine;

namespace ChainAction
{
    [CreateAssetMenu(menuName ="ActionNode/SequenceNode")]
    public class SequenceNode : Node
    {
        [SerializeField] List<Node> children;
        int currentChildren = 0;

        private void OnEnable()
        {
            currentChildren = 0;
        }

        public override Status Process(GameObject gameObject)
        {
            if (currentChildren >= children.Count)
            {
                ResetNode();
                return Status.Success;
            }

            var status = children[currentChildren].Process(gameObject);

            switch (status)
            {
                case Status.Running:
                    return Status.Running;

                case Status.Failure:
                    ResetNode();
                    return Status.Failure;

                case Status.Success:
                    int prevIndex = currentChildren; // 현재 성공한 자식의 인덱스
                    currentChildren++;               // 다음 자식으로 이동

                    if (currentChildren < children.Count)
                    {
                        // 다음 자식이 있는 경우
                        Debug.Log($"[SequenceNode] Success: {children[prevIndex].name} -> Next: {children[currentChildren].name}");
                        return Status.Running;
                    }
                    else
                    {
                        // 모든 자식이 성공한 경우
                        Debug.Log($"[SequenceNode] All Success: Last was {children[prevIndex].name}");
                        return Status.Success;
                    }
            }

            return Status.Failure; // fallback
        }

        public override void ResetNode() => currentChildren = 0;
    }
}
