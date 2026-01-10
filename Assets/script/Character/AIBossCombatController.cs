using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public enum BossState
{
    Idle, //초기 상태
    PatternReady, // 패턴 준비 상태
    Gaze, // 간보기
    Charge, // 돌진
    RangedAttack, // 원거리 공격
    MeleeAttackDelay // 근접 + 딜레이
}

public class AIBossCombatController : MonoBehaviour, IMovable
{
    public NavMeshAgent agent;
    public GameObject target;
    public Transform self;
    public float speed = 3f;
    public float stopDistance = 5f;
    public bool isMovable = true;

    private Combat npc;
    private BossState state = BossState.Idle;
    private bool isBusy = false;

    public void Initialize(Transform self, Combat npc, NavMeshAgent agent)
    {
        this.self = self;
        this.npc = npc;
        this.agent = agent;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public async void Update()
    {
        if (!isMovable || target == null || isBusy) return;

        float distance = Vector3.Distance(target.transform.position, self.position);

        switch (state)
        {
            case BossState.Idle:
                ChangeState(BossState.PatternReady);
                break;

            case BossState.PatternReady:
                if (distance > stopDistance + 1f)
                {
                    Move(target.transform.position - self.position); // 너무 멀면 다가감
                }
                else if (distance < 2f)
                {
                    ChangeState(BossState.MeleeAttackDelay); // 너무 가까우면 근접 공격
                }
                else
                {
                    await Task.Delay(Random.Range(3000, 7000));
                    DecideNextPattern();
                }
                break;

            case BossState.Gaze:
                isBusy = true;
                await Task.Delay(Random.Range(1000, 3000));
                isBusy = false;
                ChangeState(BossState.PatternReady);
                break;

            case BossState.Charge:
                isBusy = true;
                await MoveTo(target.transform.position, 100f, 1f);
                isBusy = false;
                ChangeState(BossState.PatternReady);
                break;

            case BossState.RangedAttack:
                isBusy = true;
                npc.Attack(CombatType.Range); // 원거리 공격
                await Task.Delay(1500);
                isBusy = false;
                ChangeState(BossState.PatternReady);
                break;

            case BossState.MeleeAttackDelay:
                isBusy = true;
                npc.Attack(CombatType.melee); // 근접 공격
                await Task.Delay(Random.Range(3000, 5000)); // 딜레이
                isBusy = false;
                ChangeState(BossState.PatternReady);
                break;
        }
    }

    private void DecideNextPattern()
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                ChangeState(BossState.Gaze);
                break;
            case 1:
                ChangeState(BossState.Charge);
                break;
            case 2:
                ChangeState(BossState.RangedAttack);
                break;
        }
    }

    private void ChangeState(BossState next)
    {
        Debug.Log("Current AI State: " + state);
        state = next;
    }

    public async Task MoveTo(Vector3 destination,float speed, float offset = 1f)
    {
        agent.speed = speed;
        agent.SetDestination(destination);
        while (agent.pathPending || agent.remainingDistance > offset)
        {
            await Task.Yield();
        }
        agent.speed = this.speed;
    }

    public async Task MoveTo(Vector3 destination, float offset = 1f)
    {
        agent.SetDestination(destination);
        while (agent.pathPending || agent.remainingDistance > offset)
        {
            await Task.Yield();
        }
    }

    public void Move(Vector3 direction)
    {
        var velocity = direction.normalized * speed * Time.deltaTime;
        transform.rotation.SetLookRotation(velocity);
        agent.Move(velocity);
    }

    void Move(Vector3 direction, float speed)
    {
        var velocity = direction.normalized * speed * Time.deltaTime;
        transform.rotation.SetLookRotation(velocity);
        agent.Move(velocity);
    }

    public void SetIsMovable(bool isMovable)
    {
        this.isMovable = isMovable;
    }

    public bool GetIsMovable()
    {
        return isMovable;
    }
}
