using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event EventHandler<float> OnPlayerMoves;

    [Header("Actions")]
    public InputActionReference Move;

    private void OnEnable()
    {
        InitMoveAction();
    }

    private void InitMoveAction()
    {
        Move.action.Enable();
        Move.action.performed += callback => OnPlayerMoves?.Invoke(this, callback.ReadValue<float>());
        Move.action.canceled += callback => OnPlayerMoves?.Invoke(this, 0);
    }
}
