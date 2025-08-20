using DD;
using DD.Combat;
using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Investigation,
    Combat
}

[RequireComponent(typeof(Combat), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IMovable, IPointerMovable
{
    // 플레이어가 있는 공간에 따라서 상태를 조절한다.
    private ControllerState currentState;
    [SerializeField] PlayerInput input;
    [SerializeField] ThirdPersonController thirdPersonController;

    public TriggerTableSO triggerTable;
    public EventListener eventListener = new();
    public EventHandler eventHandler = new();

    public int shotCount = 0;
    public string trigger = "ShootCount";

    public CombatState CombatState;
    public InvestigationState InvestigationState;

    public PlayerState playerState;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        CombatState = new(this, input);
        InvestigationState = new(this, input, triggerTable);

        UpdateState();
        currentState.EnterState();
    }

    public void UpdateState()
    {
        switch(playerState)
        {
            case PlayerState.Investigation:
        currentState = InvestigationState;
                break;
            case PlayerState.Combat:
        currentState = CombatState;
                break;
        }
    }

    private void OnEnable()
    {
        eventListener.StartTrack();
    }

    private void OnDisable()
    {
        eventListener.EndTrack();
    }

    private void Update()
    {
        if(currentState != null)
            currentState.Update();
    }

    public void ChangeState()
    {
        if(currentState != null)
        {
            currentState.ExitState();
            switch (playerState)
            {
                case PlayerState.Investigation:
                    playerState = PlayerState.Combat;
                    break;
                case PlayerState.Combat:
                    playerState = PlayerState.Investigation;
                    break;
                default:
                    break;
            }
            UpdateState();
            currentState.EnterState();
        }
    }

    public void Move(Vector3 direction)
    {

    }

    public void SetIsMovable(bool isMovable)
    {
        thirdPersonController.isMovable = isMovable;
    }

    public void SetPointerMovable(bool isPointerMovable)
    {
        var thirdInputs = thirdPersonController.GetComponent<StarterAssetsInputs>();
        thirdInputs.SetPointerMovable(isPointerMovable);
    }
}
