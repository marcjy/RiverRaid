using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event EventHandler<float> OnPlayerMoves;
    public event EventHandler OnPlayerStopsMoving;

    [Header("Actions")]
    public InputActionReference Move;

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
    }

    private void InitMoveAction()
    {
        Move.action.Enable();
        Move.action.performed += callback => OnPlayerMoves?.Invoke(this, callback.ReadValue<float>());
        Move.action.canceled += callback => OnPlayerStopsMoving?.Invoke(this, EventArgs.Empty);
    }
}
