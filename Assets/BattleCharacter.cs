using CharacterStateMachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCharacter : MonoBehaviour, IBattleInput
{
    StateMachine StateMachine { get; set; }
    AnimatorWrapper animator;
    IMovable controller;

    PlayerIdleState idle;
    PlayerMoveState chase;
    PlayerAttackState attack;
    PlayerDodgeState dodge;

    [Header("Debug")]
    [SerializeField] private bool StateDebug = false;
    private bool isStateDebuging = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<AnimatorWrapper>();

        if (GetComponent<ThirdPersonController>() != null)
        {
            controller = GetComponent<ThirdPersonController>();
        }

        StateMachine = new();

        StateMachine
            .AddState(new PlayerIdleState(StateMachine, animator, controller))
            .AddState(new PlayerMoveState(StateMachine, animator, controller))
            .AddState(new PlayerAttackState(StateMachine, animator, controller))
            .AddState(new PlayerDodgeState(StateMachine, animator, controller))
            .ChangeState<PlayerIdleState>();
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine?.Update();

        if(StateDebug && !isStateDebuging)
        {
            StateMachine.isDebugActive = true;
            isStateDebuging = true;
        }
        else if(!StateDebug && isStateDebuging)
        {
            StateMachine.isDebugActive = false;
            isStateDebuging = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        IBattleInput battleInput = StateMachine.currentState as IBattleInput;

        battleInput.OnMove(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        IBattleInput battleInput = StateMachine.currentState as IBattleInput;

        battleInput.OnAttack(context);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        IBattleInput battleInput = StateMachine.currentState as IBattleInput;

        battleInput.OnDodge(context);
    }

    public void OnDamaged()
    {

    }

    public void OnDead()
    {

    }
}
