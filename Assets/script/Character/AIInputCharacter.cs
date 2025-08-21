using CharacterStateMachine;
using Interection;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIInputCharacter : Character, IInterectable
{
    NavMeshAgent agent;

    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetDirection(Vector3 direction)
    {
        agent.Move(direction);
    }

    public override void SetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void SetDestinations(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            StartCoroutine(SetDestinationWhenReady(positions[i]));
        }
    }

    private IEnumerator SetDestinationWhenReady(Vector3 target)
    {
        while (agent.pathPending)
        {
            yield return null;
        }
        agent.SetDestination(target);
        agent.transform.LookAt(agent.nextPosition);

        while (agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
    }

    public void Interect()
    {
        agent.isStopped = true;
    }
}
