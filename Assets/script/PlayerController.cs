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
    Combat
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    Character character;
    [SerializeField] Vector3 direction;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        this.direction = new Vector3(direction.x, 0, direction.y);
        character.SetDirection(this.direction);
    }
}
