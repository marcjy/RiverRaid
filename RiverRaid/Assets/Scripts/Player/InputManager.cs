using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event EventHandler<float> OnPlayerMoves;
    public event EventHandler OnPlayerStopsMoving;

    public event EventHandler<float> OnPlayerAccelerating;

    [Header("Actions")]
    public InputActionReference Move;
    public InputActionReference Acceleration;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        InitMoveAction();
        InitAccelerationAction();
    }

    private void InitMoveAction()
    {
        Move.action.Enable();
        Move.action.performed += callback => OnPlayerMoves?.Invoke(this, callback.ReadValue<float>());
        Move.action.canceled += callback => OnPlayerStopsMoving?.Invoke(this, EventArgs.Empty);
    }
    private void InitAccelerationAction()
    {
        Acceleration.action.performed += callback => OnPlayerAccelerating?.Invoke(this, callback.ReadValue<float>());
        Acceleration.action.canceled += callback => OnPlayerAccelerating?.Invoke(this, callback.ReadValue<float>());
    }
}
