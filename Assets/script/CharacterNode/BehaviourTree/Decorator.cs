using BlackboardSystem;
using ServiceLocator;
using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "Node/Decorator")]
    public class Decorator : Sequence
    {
        [SerializeField] string key;
        BlackboardKey conditionKey;
        Blackboard blackboard;
        bool initialized = false;

        public Decorator(string name, int priority = 0) : base(name, priority)
        {
        }

        private void OnEnable()
        {
            initialized = false;
        }

        public override State Process()
        {
            if (!initialized)
            {
                blackboard = Locator.Get<BlackboardController>().GetBlackboard();
                conditionKey = blackboard.GetOrRegisterKey(key);
                initialized = true;
            }

            if (blackboard.TrygetValue(conditionKey, out bool isAchived) && isAchived) // Receive Only bool value
            {
                foreach (var child in children)
                {
                    Debug.Log("Achived");
                    switch (child.Process())
                    {
                        case State.Running:
                            return State.Running;
                        case State.Sucess:
                            continue;
                        case State.Failure:
                            continue;
                    }
                }
            }

            return State.Running;
        }
    }
}
