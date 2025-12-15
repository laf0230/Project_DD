using CharacterStateMachine;
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
    Combat,
    Conversation
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    Character character;
    [SerializeField] Vector3 direction;
    [SerializeField] PlayerState playerState;
    CameraController cameraController;
    TalkManager _talkManager;
    public bool isMovable = true;

    [SerializeField] LayerMask groundLayer;

    private void Start()
    {
        character = GetComponent<Character>();
        cameraController = FindAnyObjectByType<CameraController>();
        _talkManager = FindAnyObjectByType<TalkManager>();
    }

    public void Update()
    {
        if(Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, out RaycastHit hit, 100))
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.TransformDirection(Vector3.down));
    }

    public void OnNextConversation(InputAction.CallbackContext context)
    {
        if(playerState == PlayerState.Conversation)
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

    public void UpdateState(PlayerState state)
    {
        playerState = state;

        if (playerState == PlayerState.Conversation && cameraController != null)
        {
            cameraController.SetMovable(false);
            isMovable = false;
            direction = Vector3.zero;
            character.SetDirection(direction);
        }
        else
        {
            cameraController.SetMovable(true);
            isMovable = true;
        }
    }
}
