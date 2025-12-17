using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "New BehaviourTree")]
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name)
        {
        }

        public override State Process()
        {
            while(currentChild < children.Count)
            {
                var state = children[currentChild].Process();

                if(state != State.Sucess)
                {
                    return state;
                }
                currentChild++;
            }
            return State.Sucess;
        }
    }
}
