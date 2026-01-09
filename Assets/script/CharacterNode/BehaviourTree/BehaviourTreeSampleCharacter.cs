using UnityEngine;
using BehaviourTrees;
using BlackboardSystem;
using ServiceLocator;

public class BehaviourTreeSampleCharacter : MonoBehaviour
{
    [SerializeField] BehaviourTree tree;
    [SerializeField] Transform targetTransform;

    Blackboard blackboard;

    private void Start()
    {
        blackboard = Locator.Get<BlackboardController>(this).GetBlackboard();
    }

    private void Update()
    {
        tree.Process();
    }

    private void Move()
    {

    }
}
