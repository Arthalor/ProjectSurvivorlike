using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Controls actions;

    private bool shootingInput;

    private void Awake()
    {
        actions = new Controls();

        actions.Player.Shooting.performed += context => shootingInput = true;
        actions.Player.Shooting.canceled += context => shootingInput = false;
    }

    public PlayerInput Input()
    {
        Vector2 movement = actions.Player.Movement.ReadValue<Vector2>();

        return new PlayerInput(movement, shootingInput);
    }

    private void OnEnable()
    {
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }
}

public struct PlayerInput
{
    public Vector2 movement { get; private set; }
    public Vector2 mousePosition { get; private set; }
    public bool shooting { get; private set; }

    public PlayerInput(Vector2 _movement, Vector2 _mousePosition, bool _shooting) 
    {
        movement = _movement;
        shooting = _shooting;
        mousePosition = _mousePosition;
    }
}