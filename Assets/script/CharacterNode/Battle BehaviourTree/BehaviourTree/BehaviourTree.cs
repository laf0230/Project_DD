using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "New BehaviourTree")]
    public class BehaviourTree : Node
    {
        [SerializeField] State currentState;
        public BehaviourTree(string name) : base(name)
        {
        }

        public override State Process()
        {
            while(currentChild < children.Count)
            {
                currentState = children[currentChild].Process();

                if(currentState != State.Sucess)
                {
                    return currentState;
                }
                currentChild++;
            }
            Reset();
            return State.Sucess;
        }
    }
}
