using UnityEngine.InputSystem;

namespace CharacterStateMachine
{
    internal interface IBattleInput
    {
        public void OnMove(InputAction.CallbackContext context) { }
        public void OnAttack(InputAction.CallbackContext context) { }
        public void OnDodge(InputAction.CallbackContext context) { }
    }
}