using CharacterStateMachine;
using DD;
using DD.Combat;
using ServiceLocator;
using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Investigation, // 조사 상태
    Combat, // 전투 상태
    Conversation, // 대화상태
    Normal // 일반상태
}

[RequireComponent(typeof(PlayerInput))]
public partial class PlayerController : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] PlayerState currentState;
    public bool isMovable = true;

    [Header("상호작용 변수")]
    public bool isInterectable = true;
    public bool tryInterection = false;
    public LayerMask interectionLayer;
    [SerializeField] FocusCursor focusCursor;
    [Header("상호작용 거리")]
    public float interectionDistance = 10f;

    [Header("Ground Layer")]
    [SerializeField] LayerMask groundLayer;

    Character character;
    CameraController cameraController;
    TalkManager _talkManager;

    private void Awake()
    {
        Locator.Subscribe(this);
    }

    private void Start()
    {
        character = GetComponent<Character>();
        cameraController = FindAnyObjectByType<CameraController>();
        _talkManager = TalkManager.Instance;
        focusCursor = Locator.Get<FocusCursor>(this);
    }

    public void Update()
    {
        if(Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, out RaycastHit hit, interectionDistance, groundLayer))
        {
            if (hit.collider != null)
            {
                if (hit.distance >= 0.01f)
                {
                    direction.y -= Time.deltaTime;
                }
                else
                {
                    direction.y = 0f;
                }
            }
        }
        character.SetDirection(this.direction);

        RaycastHit interectionHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out interectionHit, 100f, interectionLayer))
        {
            focusCursor.DisplayFocusCursor(true);

            if (tryInterection)
            {
                if (interectionHit.transform.TryGetComponent(out IInterectable interectable))
                {
                    switch (interectable.Interect())
                    {
                        case InterectionType.Conversation:
                            isMovable = false;
                            UpdateState(PlayerState.Conversation);
                            break;
                        default:
                            isMovable = true;
                            UpdateState(PlayerState.Investigation);
                            break;
                    }
                }
            }
        }
        else
        {
            focusCursor.DisplayFocusCursor(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.TransformDirection(Vector3.down));
    }

    public void Interection(InputAction.CallbackContext context)
    {
        if (focusCursor == null) Debug.LogError("커서 UI를 찾지 못했습니다.\n" + new NullReferenceException());
        if (currentState == PlayerState.Investigation) return;

        if (context.started)
            tryInterection = true;
        else if (context.canceled)
            tryInterection = false;
    }

    public void OnNextConversation(InputAction.CallbackContext context)
    {
        if(currentState == PlayerState.Conversation && context.performed)
            _talkManager.NextDialogue();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!isMovable)
            return;

        var direction = context.ReadValue<Vector2>();
        this.direction.x = direction.x;
        this.direction.z = direction.y;
    }

    public void OpenInferenceUI(InputAction.CallbackContext context)
    {
        if (context.performed)
            Locator.Get<InferenceUI>(this).OpenUI();
    }

    public void UpdateState(PlayerState state)
    {
        Debug.Log($"[PlayerController currentState{state}");
        currentState = state;

        if (currentState == PlayerState.Conversation && cameraController != null)
        {
            cameraController.SetMovable(false);
            cameraController.SetMouseLock(false);
            isMovable = false;
            direction = Vector3.zero;
            character.SetDirection(direction);
            isInterectable = false;
        }
        else
        {
            cameraController.SetMovable(true);
            cameraController.SetMouseLock(true);
            isMovable = true;
            isInterectable = true;
        }
    }
}
