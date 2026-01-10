using UnityEngine;

namespace BehaviourTrees
{
    [CreateAssetMenu(menuName = "BehaviourTree/Node/RandomSelector")]
    public class RandomSelector : Selector
    {
        int selectedIndex = -1;
        public RandomSelector(string name, int priority = 0) : base(name, priority)
        {
        }

        public override State Process()
        {
            if (selectedIndex == -1)
            {
                if (children.Count == 0) return State.Failure;
                selectedIndex = Random.Range(0, children.Count);
            }

            // 하나를 선택해서 실행함
            Debug.Log($"Randomly Selected Pattern: {selectedIndex}");
            switch (children[selectedIndex].Process())
            {
                case State.Sucess:
                    Reset();
                    return State.Sucess;
                case State.Running:
                    return State.Running;
                case State.Failure:
                    Reset();
                    return State.Failure;
            }
            return State.Failure;
        }

        public override void Reset()
        {
            base.Reset();
            selectedIndex = -1;
        }
    }
}
