using BlackboardSystem;
using ServiceLocator;
using UnityEngine;
using UnityEngine.AI;

public class BBBossController : MonoBehaviour, IBBMovable
{
    public Vector3 targetPosition = Vector3.zero;
    public GameObject targetObject;
    public CharacterController controller;
    public NavMeshAgent agent;
    public BlackboardEntryData bossSpeedData;
    public BossAnimatorWrapper anim;
    public string targetTag;

    public Blackboard blackboard;
    BlackboardKey targetPositionKey;
    private float moveSpeed = 1f;

    private void Awake()
    {
        Locator.Subscribe(this);
    }

    private void Start()
    {
        anim = Locator.Get<BossAnimatorWrapper>(this);
        blackboard = Locator.Get<BlackboardController>(this).GetBlackboard();
        targetPositionKey = blackboard.GetOrRegisterKey("TargetPosition");
        targetObject = GameObject.Find(targetTag);
    }

    private void Update()
    {
        if (targetObject != null)
        {
            targetPosition = targetObject.transform.position;

            blackboard.SetValue(targetPositionKey, targetPosition);
        }
    }

    public void MoveTo(string targetTag)
    {
        if (!blackboard.TrygetValue(targetPositionKey, out Vector3 targetPosition))
        {
            Debug.LogWarning("[BBBossController] Target Position is Null.");
        }

        var direction = (targetPosition - transform.position).normalized;
        var velocity = direction * moveSpeed;

        anim.PlayAction("Speed", velocity.magnitude);
        transform.rotation.SetLookRotation(direction);
        agent.Move(velocity);
    }

    public void SetTarget(string tag)
    {
        targetObject = GameObject.FindWithTag(tag);
    }
} 