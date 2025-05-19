using System;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static event EventHandler<float> OnSpeedChanged;

    public static float CurrentSpeed => _currentSpeed;
    private static float _currentSpeed;

    [Header("Scroll Speed")]
    public float NormalSpeed = 2.0f;
    public float SlowSpeed = 1.0f;
    public float FastSpeed = 4.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentSpeed = 0.0f;
        InputManager.Instance.OnPlayerAccelerating += HandlePlayerAccelerating;

        GameManager.Instance.OnStartLevel += HandleStartLevel;
        GameManager.Instance.OnResetGame += HandleGameStateReset;
        GameManager.Instance.OnEndGame += HandleGameStateReset;
    }

    #region Event Handling
    private void HandleStartLevel(object sender, System.EventArgs e)
    {
        _currentSpeed = NormalSpeed;
        OnSpeedChanged?.Invoke(this, _currentSpeed);
    }
    private void HandleGameStateReset(object sender, System.EventArgs e)
    {
        _currentSpeed = 0.0f;
        OnSpeedChanged?.Invoke(this, _currentSpeed);
    }

    private void HandlePlayerAccelerating(object sender, float acceleration)
    {
        CalculateSpeed(acceleration);
        OnSpeedChanged?.Invoke(this, _currentSpeed);
    }
    #endregion

    private void CalculateSpeed(float acceleration)
    {
        if (acceleration == 0)
            _currentSpeed = NormalSpeed;
        else
            if (acceleration < 0)
            _currentSpeed = SlowSpeed;
        else
            _currentSpeed = FastSpeed;
    }
}
