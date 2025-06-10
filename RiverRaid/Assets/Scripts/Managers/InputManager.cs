using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event EventHandler OnStartPressed;

    public event EventHandler<float> OnPlayerMoves;
    public event EventHandler OnPlayerStopsMoving;
    public event EventHandler<float> OnPlayerAccelerating;
    public event EventHandler OnPlayerFires;


    [Header("In-Game Actions")]
    public InputActionReference Move;
    public InputActionReference Acceleration;
    public InputActionReference Fire;

    [Header("UI Actions")]
    public InputActionReference StartGame;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        InitMoveAction();
        InitAccelerationAction();
        InitFireAction();

        DisablePlayerActionsInGame();

        InitStartGameAction();
    }

    private void Start()
    {
        GameManager.Instance.OnStartLevel += (sender, e) => EnablePlayerActionsInGame();
        GameManager.Instance.Player.GetComponent<PlayerController>().OnStartDeath += (sender, e) => DisablePlayerActionsInGame();
        GameManager.Instance.OnResetGame += (sender, e) => StartGame.action.Enable();
    }

    private void EnablePlayerActionsInGame()
    {
        Move.action.Enable();
        Acceleration.action.Enable();
        Fire.action.Enable();
    }

    private void DisablePlayerActionsInGame()
    {
        Move.action.Disable();
        Acceleration.action.Disable();
        Fire.action.Disable();
    }

    private void InitStartGameAction()
    {
        StartGame.action.Enable();
        StartGame.action.performed += callback =>
        {
            OnStartPressed?.Invoke(this, EventArgs.Empty);
            StartGame.action.Disable();
        };
    }

    private void InitMoveAction()
    {
        Move.action.Disable();
        Move.action.performed += callback => OnPlayerMoves?.Invoke(this, callback.ReadValue<float>());
        Move.action.canceled += callback => OnPlayerStopsMoving?.Invoke(this, EventArgs.Empty);
    }
    private void InitAccelerationAction()
    {
        Acceleration.action.Disable();
        Acceleration.action.performed += callback => OnPlayerAccelerating?.Invoke(this, callback.ReadValue<float>());
        Acceleration.action.canceled += callback => OnPlayerAccelerating?.Invoke(this, callback.ReadValue<float>());
    }
    private void InitFireAction()
    {
        Fire.action.Disable();
        Fire.action.performed += callback => OnPlayerFires?.Invoke(this, EventArgs.Empty);
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }
}
