using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace CharacterStateMachine
{
    public class Character : MonoBehaviour
    {
        StateMachine StateMachine = new();

        public InvestCharacter investCharacter;
        public BattleState battleCharacter;

        protected virtual void Awake()
        {
            //investCharacter = new(this);
            //battleCharacter = new(this);

            StateMachine.ChangeState(investCharacter);
        }

        protected virtual void Start() { }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        protected virtual void FixedUpdate() { }

        public virtual void SetDirection(Vector3 direction) { }

        public virtual void SetDestination(Vector3 position) { }
        // Character에 Input Handle을 할 수 없음. AI또한 Character를 컨트롤 해야하기 때문, 따라서 각 상태(Enter, Exit, Update)를 분리해서 관리한다.
    }
}
