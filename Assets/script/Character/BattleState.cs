using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterStateMachine
{
    public class Player: IMovable
    {
        StateMachine stateMachine;
        AnimatorWrapper animator;
        bool isMovable = true;

        PlayerIdleState idle;
        PlayerMoveState chase;
        PlayerAttackState attack;
        PlayerDodgeState dodge;

        public void Start()
        {
            idle = new(stateMachine, animator, this);
            chase = new(stateMachine, animator, this);
            attack = new(stateMachine, animator, this);
            dodge = new(stateMachine, animator, this);

            stateMachine = new();
            stateMachine
                .AddState(idle)
                .AddState(chase)
                .AddState(attack)
                .AddState(dodge)
                .ChangeState<PlayerIdleState>();
            stateMachine.ChangeState<PlayerMoveState>();
            stateMachine .ChangeState<PlayerAttackState>();
        }

        public void SetIsMovable(bool isMovable) => this.isMovable = isMovable;
        public bool GetIsMovable() => isMovable;
    }

    public class BattleState : State, IBattleInput
    {
        public BattleState(StateMachine stateMachine, AnimatorWrapper animator) : base(stateMachine, animator)
        {
        }

        public virtual void OnMove(InputAction.CallbackContext context) { }
        public virtual void OnAttack(InputAction.CallbackContext context) { }
        public virtual void OnDodge(InputAction.CallbackContext context) { }
    }

    public class PlayerIdleState : BattleState
    {
        IMovable mMovable;

        public PlayerIdleState(StateMachine stateMachine, AnimatorWrapper animator, IMovable movable) : base(stateMachine, animator)
        {
            this.mMovable = movable;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            base.OnMove(context);

            stateMachine.ChangeState<PlayerMoveState>();
        }

        public override void OnAttack(InputAction.CallbackContext context)
        {
            base.OnAttack(context);

            if(context.performed)
                stateMachine.ChangeState<PlayerAttackState>();
        }

        public override void OnDodge(InputAction.CallbackContext context)
        {
            base.OnDodge(context);

            if(context.performed)
                stateMachine.ChangeState<PlayerDodgeState>();
        }
    }

    public class PlayerMoveState : BattleState
    {
        IMovable movable;

        public PlayerMoveState(StateMachine stateMachine, AnimatorWrapper animator, IMovable movable) : base(stateMachine, animator)
        {
            this.movable = movable;
        }

        public override void Enter()
        {
            base.Enter();

            movable.SetIsMovable(true);
        }

        public override void OnAttack(InputAction.CallbackContext context)
        {
            base.OnAttack(context);

            stateMachine.ChangeState<PlayerAttackState>();
        }

        public override void OnDodge(InputAction.CallbackContext context)
        {
            base.OnDodge(context);

            stateMachine.ChangeState<PlayerDodgeState>();
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            base.OnMove(context);

            if(context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Disabled)
                stateMachine.ChangeState<PlayerIdleState>();
        }
    }

    public class PlayerAttackState : BattleState
    {
        IMovable movable;

        public PlayerAttackState(StateMachine stateMachine, AnimatorWrapper animator, IMovable movable) : base(stateMachine, animator)
        {
            this.movable = movable;
        }

        public override void Enter()
        {
            base.Enter();

            movable.SetIsMovable(false);
            animator.SetTrigger("AttackTrigger");
        }

        public override void Update()
        {
            base.Update();

            if (animator.isActionFinished)
            {
                stateMachine.ChangeState<PlayerIdleState>();
            }
        }

        public override void Exit()
        {
            base.Exit();

            movable.SetIsMovable(true);
        }
    }

    public class PlayerDodgeState : BattleState
    {
        IMovable movable;

        public PlayerDodgeState(StateMachine stateMachine, AnimatorWrapper animator, IMovable movable) : base(stateMachine, animator)
        {
            this.movable = movable;
        }

        public override void Enter()
        {
            base.Enter();

            movable.SetIsMovable(false);
            animator.SetTrigger("DodgeTrigger");
        }

        public override void Update()
        {
            base.Update();

            if (animator.isActionFinished)
            {
                stateMachine.ChangeState<PlayerIdleState>();
            }
        }

        public override void Exit()
        {
            base.Exit();

            movable.SetIsMovable(true);
        }
    }

    public class PlayerDeadState : BattleState
    {
        IMovable movable;
        float animationTime;

        public PlayerDeadState(StateMachine stateMachine, AnimatorWrapper animator, IMovable movable) : base(stateMachine, animator)
        {
            this.movable = movable;
        }

        public override void Enter()
        {
            base.Enter();

            movable.SetIsMovable(false);
            animator.SetTrigger("DeadTrigger");
            animationTime = animator.currentClip.length;
        }

        public override void Update()
        {
            base.Update();

            if (animationTime > 0)
            {
                animationTime -= Time.deltaTime;
            }
        }
    }
}
