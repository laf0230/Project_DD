using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "new Leaf Node")]
    public class Leaf : Node
    {
        [SerializeField]
        private Strategy strategy;

        public Leaf(string name, int priority) : base(name, priority) { }

        public override State Process() => strategy.Process();

        public override void Reset() => strategy.ResetStrategy();
    }
}
