using DD;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour, IInterectable
{
    [SerializeField] public EventListener eventListener = new();
    [SerializeField] PlayerState state;
    [SerializeField] public Combat combat;
    [SerializeField] AIBossCombatController aiCombat;
    NavMeshAgent agent;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float defaultSpeed = 3;
    [SerializeField] string[] phase;
    [SerializeField] PatrolPath patrol;

    private string currentPhase;
    private int pathIndex = 0;

    private void Start()
    {
        combat = GetComponent<Combat>();
        aiCombat = GetComponent<AIBossCombatController>();
        agent = GetComponent<NavMeshAgent>();
        patrol = GetComponent<PatrolPath>();
        aiCombat.Initialize(transform, combat, agent);
        combat.Init(aiCombat);
        var target = aiCombat.target;
        aiCombat.target = target;
        aiCombat.agent = GetComponent<NavMeshAgent>();
        aiCombat.self = transform;
    }

    private void OnEnable()
    {
        eventListener.StartTrack();

        switch (state)
        {
            case PlayerState.Investigation:
                if (phase.Length <= 0) return;

                    currentPhase = phase[0];
                break;
            case PlayerState.Combat:

                break;
            default:
                break;
        }
    }

    public async void MoveTo(Vector3 position, float speed, float offset = 1)
    {
        var currentSpeed = agent.speed;
        agent.SetDestination(position);
        while (agent.pathPending || agent.remainingDistance > offset)
        {
            await Task.Yield();
        }
        agent.speed = currentSpeed;
    }

    public void Move(Vector3 velocity)
    {
        agent.Move(velocity); 
    }

    private void Update()
    {
        switch (state)
        {
            case PlayerState.Investigation:
                if(phase.Length > 0)
                {
                    var destination = patrol.GetWaypoint(currentPhase, pathIndex);
                    MoveTo(destination.position, 3f);
                }
                break;
            case PlayerState.Combat:
                aiCombat.Update();

                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        eventListener.EndTrack();
    }

    public void Anger()
    {
        Debug.Log("¹» °Ô¼Ó Âñ·¯!");
    }

    public void Interect(Player player)
    {
        Debug.Log("Hello Player");
    }

    public void Interect(PlayerController player)
    {
    }
}
