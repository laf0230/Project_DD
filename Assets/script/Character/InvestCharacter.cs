namespace CharacterStateMachine
{
    public class InvestCharacter : State<Character>
    {
        public StateMachine<InvestCharacter> currentState = new();
        public InvestCharacterMoveState moveState;
        public InvestCharacterTalkState talkState;

        public override void Enter()
        {
            base.Enter();
        }

        public void Interection(IInterectable interactable)
        {
            interactable.Interect();
        }

        public InvestCharacter(Character character) : base(character)
        {
        }
    }

    public class InvestCharacterTalkState : State<InvestCharacter>
    {
        public InvestCharacterTalkState(InvestCharacter character) : base(character)
        {
        }
    }

    public class InvestCharacterMoveState : State<InvestCharacter>
    {
        public InvestCharacterMoveState(InvestCharacter character) : base(character)
        {
        }
    }
}
