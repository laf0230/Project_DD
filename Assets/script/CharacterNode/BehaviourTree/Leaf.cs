using UnityEngine;

namespace Node
{
    [CreateAssetMenu(fileName = "Behaviour Tree/Leaf")]
    public class Leaf : Node
    {
        [SerializeField] 
        readonly IStrategy strategy;

        public Leaf(string name, int priority) : base(name, priority) { }

        public override State Process() => strategy.Process();

        public override void ResetNode() => strategy.ResetStrategy();
    }
}
