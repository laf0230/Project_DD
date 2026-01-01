using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "BehaviourTree/Node/Selector")]
    public class Selector : Node // OR failure일 시 다음 자식 실행 자식이 하나라도 Success이면 Success하고 종료
    {
        [SerializeField] private State currentState;
        public Selector(string name, int priority = 0) : base(name, priority)
        {
        }

        private void OnEnable()
        {
            Reset();
        }

        public override State Process()
        {
            // 하나를 선택해서 실행함
            if(currentChild < children.Count)
            {
                Debug.Log($"[Selector] Current Node: {children[currentChild]}");
                currentState = children[currentChild].Process();

                switch(currentState)
                {
                    case State.Sucess:
                        return State.Sucess;
                    case State.Failure:
                        currentChild++;
                        return State.Running;
                    case State.Running:
                        return State.Running;
                }
            }
            Reset();
            return State.Sucess;
        }
    }
}
