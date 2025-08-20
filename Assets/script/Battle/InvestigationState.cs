using DD;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvestigationState : ControllerState
{
    private InputAction interection;
    private EventListener eventListener = new();
    private EventHandler eventHandler = new();
    private TriggerTableSO triggerTable;

    public InvestigationState(PlayerController controller, PlayerInput input, TriggerTableSO triggerTable) : base(controller, input)
    {
        this.triggerTable = triggerTable;
        eventListener.AddEvent(triggerTable.GetEventList());
    }

    public override void EnterState()
    {
        eventListener.StartTrack();

        interection = input.actions.FindActionMap("Player").FindAction("Interact");
        interection.Enable();

        interection.started += OnInvestigation;
    }

    public override void ExitState()
    {
        eventListener.EndTrack();

        interection.performed -= OnInvestigation;
    }

    public void OnInvestigation(InputAction.CallbackContext callback)
    {
        if(TryInterect())
        {
            controller.shotCount++;
            eventHandler.UpdateTrigger(new ComprisonCondition(controller.trigger, controller.shotCount));
        }
    }

    public bool TryInterect()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 10f))
        {
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.TryGetComponent(out IInterectable interectable))
            {
                interectable.Interect(controller);
                return true;
            }
        }
        return false;
    }

    public void PrintShoot(int shotCount)
    {
        Debug.Log($"{shotCount} Shot Count");
    }
}
