using UnityEngine;

namespace CharacterStateMachine
{
    public class InvestCharacter: State
    {
        public StateMachine currentState = new();
        public InvestCharacterMoveState moveState;
        public InvestCharacterTalkState talkState;

        public InvestCharacter(StateMachine stateMachine, Animator animator) : base(stateMachine, animator)
        {
        }

        public void Interection(IInterectable interactable)
        {
            interactable.Interect();
        }
    }

    public class InvestCharacterTalkState : State
    {
        public InvestCharacterTalkState(StateMachine stateMachine, Animator animator) : base(stateMachine, animator)
        {
        }
    }

    public class InvestCharacterMoveState : State
    {
        public InvestCharacterMoveState(StateMachine stateMachine, Animator animator) : base(stateMachine, animator)
        {
        }
    }
}
