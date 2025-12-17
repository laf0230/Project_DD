using UnityEngine;
using BehaviourTrees;
using BlackboardSystem;
using ServiceLocator;

public class BehaviourTreeSampleCharacter : MonoBehaviour
{
    [SerializeField] BehaviourTree tree;
    [SerializeField] Transform targetTransform;

    Blackboard blackboard;
    BlackboardKey isSafeKey;
    BlackboardKey bossPositionKey;
    BlackboardKey targetDistanceKey;
    BlackboardKey isAttackableKey;
    BlackboardKey distanceKey;

    private void Start()
    {
        blackboard = Locator.Get<BlackboardController>().GetBlackboard();

        isSafeKey = blackboard.GetOrRegisterKey("IsSafe");
        bossPositionKey = blackboard.GetOrRegisterKey("BossPosition");
        targetDistanceKey = blackboard.GetOrRegisterKey("TargetPosition");
        isAttackableKey = blackboard.GetOrRegisterKey("IsAttackable");
        distanceKey = blackboard.GetOrRegisterKey("Distance");

        bool IsSafe()
        {
            if (blackboard.TrygetValue(isSafeKey, out bool isSafe))
            {
                if (!isSafe)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void Update()
    {
        tree.Process();

        blackboard.SetValue(bossPositionKey, transform.position);
        blackboard.SetValue(targetDistanceKey, targetTransform.position);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (blackboard.TrygetValue(isSafeKey, out bool isSafe))
                {
                    blackboard.SetValue(isSafeKey, !isSafe);
                    Debug.Log($"IsSafe: {isSafe}");
                }
            }
    }
}
