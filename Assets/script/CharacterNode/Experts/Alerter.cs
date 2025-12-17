using ServiceLocator;
using UnityEditor;
using UnityEngine;

namespace BlackboardSystem
{
    public class Alerter : MonoBehaviour, IExpert
    {
        Blackboard blackboard;

        BlackboardKey bossPositionKey;
        BlackboardKey targetPositionKey;
        BlackboardKey distanceKey;
        BlackboardKey isAttackableKey;

        float distance;
        [SerializeField] float targetDistance;

        private void Start()
        {
            blackboard = Locator.Get<BlackboardController>().GetBlackboard();

            Locator.Get<BlackboardController>().RegisterExpert(this);
            bossPositionKey = blackboard.GetOrRegisterKey("BossPosition");
            targetPositionKey = blackboard.GetOrRegisterKey("TargetPosition");
            distanceKey = blackboard.GetOrRegisterKey("Distance");
            isAttackableKey = blackboard.GetOrRegisterKey("IsAttackable");
        }

        public void Execute(Blackboard blackboard)
        {
            blackboard.AddAction(() =>
            {
                if (blackboard.TrygetValue(isAttackableKey, out bool isAttackable))
                {
                    blackboard.SetValue(isAttackableKey, true);
                    blackboard.SetValue(distanceKey, distance);
                }
            });
        }

        public int GetInsistence(Blackboard blackboard)
        {
            if (blackboard.TrygetValue(bossPositionKey, out Vector3 bossPosition) && blackboard.TrygetValue(targetPositionKey, out Vector3 targetPosition))
            {
                distance = Vector3.Distance(bossPosition, targetPosition);
            }
            
            return distance < targetDistance ? 100 : 0;
        }
    }
}
