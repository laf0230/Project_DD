using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerState
{
    public PlayerInput input;
    public PlayerController controller;

    public ControllerState(PlayerController controller, PlayerInput input)
    {
        this.controller = controller;
        this.input = input;
    }

    public virtual void EnterState() { }

    public virtual void ExitState() { }

    public virtual void Update() { }
}
