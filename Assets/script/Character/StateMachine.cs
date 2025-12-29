using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CharacterStateMachine
{
    public class StateMachine
    {
        public List<State> states = new();
        public State currentState;
        public bool isDebugActive = false;

        public StateMachine AddState(State state)
        {
            states.Add(state);

            return this;
        }

        State GetStateFromStateList<T>() where T : State
        {
            foreach (var state in states)
            {
                if(state.GetType() == typeof(T))
                    return state;
            }

            Debug.Log($"StateMachine: {typeof(T)} is not exist State.");
            return null;
        }

        public void ChangeState(State newState)
        {
            if(currentState == newState) return;

            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void ChangeState<T>() where T : State
        {
            if(isDebugActive)
                Debug.Log($"Player State Changed {currentState.GetType()} to {typeof(T)}");
            currentState?.Exit();
            currentState = GetStateFromStateList<T>();
            currentState?.Enter();
        }

        public void Update() => currentState?.Update();
    }

    public class State
    {
        public StateMachine stateMachine { get; set; }
        public AnimatorWrapper animator { get; set; }

        public State(StateMachine stateMachine, AnimatorWrapper animator)
        {
            this.stateMachine = stateMachine;
            this.animator = animator;
        }

        public virtual void Update() { }
        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}
