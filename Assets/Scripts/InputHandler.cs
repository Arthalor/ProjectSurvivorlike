using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    private Controls actions;

    private Camera mainCamera;

    private bool shootingInput;
    private bool reloadInput;

    private void Awake()
    {
        actions = new Controls();

        actions.Player.Shoot.performed += context => shootingInput = true;
        actions.Player.Shoot.canceled += context => shootingInput = false;

        actions.Game.Pause.performed += PauseKeyPressed;

        mainCamera = Camera.main;
    }

    public PlayerInput Input()
    {
        Vector2 movement = actions.Player.Movement.ReadValue<Vector2>();
        Vector2 worldMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        reloadInput = actions.Player.Reload.WasPressedThisFrame();

        return new PlayerInput(movement, worldMousePos, shootingInput, reloadInput);
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
    public bool shoot { get; private set; }
    public bool reload { get; private set; }

    public PlayerInput(Vector2 _movement, Vector2 _mousePosition, bool _shooting, bool _reload) 
    {
        movement = _movement;
        shoot = _shooting;
        reload = _reload;
        worldMousePosition = _mousePosition;
    }
}