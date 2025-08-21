namespace CharacterStateMachine
{
    public class StateMachine<T>
    {
        public State<T> currentState;

        public void ChangeState(State<T> newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void Update() => currentState?.Update();
    }

    public class State<T>
    {
        public T character;
        public State(T character) { this.character = character; }

        public virtual void Update() { }
        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}
