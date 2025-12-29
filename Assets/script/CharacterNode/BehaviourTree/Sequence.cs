using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "BehaviourTree/Node/Sequence")]
    public class Sequence : Node // AND 하나라도 failure이면 모두 failure 모든 자식이 Success이면 Success
    {
        [SerializeField] State currentState;
        public Sequence(string name, int priority = 0) : base(name, priority)
        {
        }

        public override State Process()
        {
            if(currentChild < children.Count)
            {
                currentState = children[currentChild].Process();
                Debug.Log($"Sequence {children[currentChild].name}");

                // 자식 노드가 sucess일 때 다음 노드로 이동
                // 모든 노드 순회가 끝날 경우 sucess
                // 순회가 종료되지 않았을 경우 running
                switch (currentState)
                {
                    case State.Failure:
                        Reset();
                        return State.Failure;
                    case State.Running:
                        return State.Running;
                    case State.Sucess:
                        currentChild++;
                        return State.Running;
                }
            }
            Reset();
            return State.Sucess;
        }
    }
}
