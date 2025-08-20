using UnityEngine;
using UnityEngine.InputSystem;

public class CombatState : ControllerState
{
    private InputAction attackInput;
    private Combat combat;

    public CombatState(PlayerController controller, PlayerInput input) : base(controller, input)
    {
        combat = controller.GetComponent<Combat>();
    }

    public override void EnterState()
    {
        attackInput = input.actions.FindActionMap("Player").FindAction("Attack");
        attackInput.Enable();

        combat.Init(controller);

        attackInput.started += AttackInput;
    }

    public void AttackInput(InputAction.CallbackContext callback)
    {
        combat.Attack(CombatType.melee);

        if(TryAttack())
        {
            controller.eventHandler.UpdateTrigger(new ComprisonCondition(controller.trigger, controller.shotCount));
        }
    }

    public bool TryAttack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 10f))
        {
        }
        return false;
    }

    public bool TryGetCombatState(out CombatState state)
    {
        state = this;
        return true;
    }
}
