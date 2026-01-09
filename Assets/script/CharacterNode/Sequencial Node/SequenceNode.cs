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
                    currentChildren++;
                    var prev = currentChildren -1 < 0 ? children.Count - 1 : currentChildren;
                    Debug.Log($"[SequenceNode] StatusChanged {children[prev].name} to {children[currentChildren].name}");
                    return Status.Running; // 다음 자식 실행을 위해 Running 반환
            }

            return Status.Failure; // fallback
        }

        public override void ResetNode() => currentChildren = 0;
    }
}
