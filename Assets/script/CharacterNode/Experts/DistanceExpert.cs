using ServiceLocator;
using UnityEditor;
using UnityEngine;

namespace BlackboardSystem
{
    public class DistanceExpert : MonoBehaviour, IExpert
    {
        Blackboard blackboard;

        BlackboardKey bossPositionKey;
        BlackboardKey targetPositionKey;
        BlackboardKey distanceKey;
        BlackboardKey isAttackableKey;

        float distance;
        [SerializeField] float maxDistance;
        [SerializeField] AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private void Start()
        {
            blackboard = Locator.Get<BlackboardController>(this).GetBlackboard();

            Locator.Get<BlackboardController>(this).RegisterExpert(this);
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
            
            return distance < maxDistance ? 100 : 0;
        }

        private void OnDrawGizmosSelected()
        {
            var keys = weightCurve.keys;

            Color GetColor(int index)
            {
                float t = keys.Length > 0 ? (float)index / keys.Length : 0;
                return Color.Lerp(Color.blue, Color.red, t);
            }

            Gizmos.color = GetColor(0);
            Gizmos.DrawWireSphere(transform.position, maxDistance);

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].time <= 0) continue;

                Gizmos.color = GetColor(i + 1);

                var radious = maxDistance / keys[i].time;

                Gizmos.DrawWireSphere(transform.position, radious);
            }
        }
    }
}
