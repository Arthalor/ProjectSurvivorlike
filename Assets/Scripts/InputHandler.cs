using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Controls actions;

    private Camera mainCamera;

    private bool shootingInput;

    private void Awake()
    {
        actions = new Controls();

        actions.Player.Shooting.performed += context => shootingInput = true;
        actions.Player.Shooting.canceled += context => shootingInput = false;

        actions.Game.Pause.performed += PauseKeyPressed;

        mainCamera = Camera.main;
    }

    public PlayerInput Input()
    {
        Vector2 movement = actions.Player.Movement.ReadValue<Vector2>();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return new PlayerInput(movement, worldMousePos, shootingInput);
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Game.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
        actions.Game.Disable();
    }

    private void PauseKeyPressed(InputAction.CallbackContext context) 
    {
        GameManager.Instance.gamePlayManager.PauseKeyPressed();
    }
}

public struct PlayerInput
{
    public Vector2 movement { get; private set; }
    public Vector2 worldMousePosition { get; private set; }
    public bool shooting { get; private set; }

    public PlayerInput(Vector2 _movement, Vector2 _mousePosition, bool _shooting) 
    {
        movement = _movement;
        shooting = _shooting;
        worldMousePosition = _mousePosition;
    }
}