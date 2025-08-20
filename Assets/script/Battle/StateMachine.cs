using System;
using UnityEngine;

namespace DD.Combat
{
    public class StateMachine
    {
        public BaseState currentState;

        public void ChangeState(BaseState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void Update()
        {
            currentState.UpdateState();
        }
    }

    public class BaseState
    {
        public StateController controller;
        public BaseState(StateController baseController) => controller = baseController;
        public virtual void Enter() { }
        public virtual void UpdateState() { }
        public virtual void Exit() { }
    }

    public class CombatState: BaseState
    {
        public Action onAttack;
        public Action<Vector3> onMove;
        public global::Combat combat;

        public CombatState(StateController baseController, global::Combat combat) : base(baseController)
        {
            this.combat = combat;
        }

        public override void Enter()
        {
            onMove += Move;
        }

        public override void UpdateState()
        {
            onMove(Vector3.zero);
        }

        public void Move(Vector3 direction)
        {

        }

        public void Attack()
        {
            combat.Attack(CombatType.melee);
        }
    }

    public class InvestigationState: BaseState
    {
        public Action onInterection;
        public Action<Vector3> onMove;

        public InvestigationState(StateController baseController) : base(baseController)
        {
        }
    }

    public class BaseCombatStateMachine: StateMachine
    {
    }

    public class BaseCombatState : BaseState
    {
        public BaseCombatState(StateController baseController) : base(baseController)
        {
        }
    }

    [RequireComponent(typeof(global::Combat))]
    public class StateController: MonoBehaviour
    {
        public StateMachine stateMachine;

        public CombatState combatState;
        public InvestigationState investigationState;

        private IMovable movable;

        private void Start()
        {
            var combat = GetComponent<global::Combat>();

            combatState = new(this, combat);
            investigationState = new(this);
            stateMachine.ChangeState(investigationState);

            movable = GetComponent<IMovable>();
        }

        private void Update()
        {
            stateMachine.Update();
        }

        public void Move(Vector3 direction)
        {

        }
    }

    public interface IMovable
    {
        public void Move(Vector3 direction);
        public virtual void Move(Vector3 direction, float speed) { }
        public void SetIsMovable(bool isMovable);
    }

    [RequireComponent(typeof(StateController))]
    public class AIController : MonoBehaviour, IMovable
    {
        BaseState baseState;
        StateController stateController;

        InvestigationState investigationState;
        CombatState combatState;

        private void Start()
        {
            stateController = GetComponent<StateController>();
            investigationState = new(stateController);
        }

        public void Update()
        {
            // ai 행동 지시
            
        }

        public void Move(Vector3 direction)
        {
            // MoveCharacter
        }

        public void Attack()
        {

        }

        public void SetIsMovable(bool isMovable)
        {
            throw new NotImplementedException();
        }
    }

    [RequireComponent(typeof(StateController))]
    public class PlayerController: MonoBehaviour, IMovable
    {
        BaseState baseState;
        StateController stateController;

        public void Move(Vector3 direction)
        {
            // MoveCharacter
        }

        public void SetIsMovable(bool isMovable)
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            stateController = GetComponent<StateController>();
        }
    }
}
