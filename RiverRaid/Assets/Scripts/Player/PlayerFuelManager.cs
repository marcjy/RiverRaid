using System;
using System.Collections;
using UnityEngine;

public class PlayerFuelManager : MonoBehaviour
{
    private enum FuelLevelState { Normal, Low, Critical, Empty }

    public event EventHandler OnOutOfFuel;
    public event EventHandler<float> OnCurrentFuelChanged;
    public event EventHandler OnNormalFuelLevel;
    public event EventHandler OnLowFuelLevel;
    public event EventHandler OnCriticalFuelLevel;

    public float MaxFuel;
    public float FuelConsumptionPerTick;
    public float FuelConsumptionTickRate;

    [Header("Fuel Warnings")]
    [Range(0, 1)]
    public float LowFuelLevels;
    [Range(0, 1)]
    public float CriticalFuelLevels;
    private FuelLevelState _currenFuelLevelState = FuelLevelState.Normal;

    private float _currentFuel;
    private Coroutine _fuelConsumptionCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnStartLevel += HandleStartLevel;
        GameManager.Instance.OnLevelEnds += HandleLevelEnds;
    }

    public void AddFuelPercentage(float fuelPercentage) => _currentFuel = Mathf.Clamp(_currentFuel + (MaxFuel * fuelPercentage), 0, MaxFuel);

    #region Event Handling
    private void HandleStartLevel(object sender, EventArgs e)
    {
        _currentFuel = MaxFuel;
        _fuelConsumptionCoroutine = StartCoroutine(ConsumeFuel());
    }
    private void HandleLevelEnds(object sender, EventArgs e)
    {
        StopCoroutine(_fuelConsumptionCoroutine);
        _fuelConsumptionCoroutine = null;
    }
    #endregion
    private IEnumerator ConsumeFuel()
    {
        while (true)
        {
            yield return new WaitForSeconds(FuelConsumptionTickRate);

            _currentFuel -= FuelConsumptionPerTick;
            OnCurrentFuelChanged?.Invoke(this, (_currentFuel / MaxFuel) * 100.0f);

            EvaluateFuelLevel();
        }
    }

    private void EvaluateFuelLevel()
    {
        float fuelPercentage = _currentFuel / MaxFuel;
        FuelLevelState newState;

        if (_currentFuel <= 0.0f)
        {
            newState = FuelLevelState.Empty;
        }
        else if (fuelPercentage < CriticalFuelLevels)
        {
            newState = FuelLevelState.Critical;
        }
        else if (fuelPercentage < LowFuelLevels)
        {
            newState = FuelLevelState.Low;
        }
        else
        {
            newState = FuelLevelState.Normal;
        }

        if (_currenFuelLevelState != newState)
        {
            _currenFuelLevelState = newState;

            switch (newState)
            {
                case FuelLevelState.Normal:
                    OnNormalFuelLevel?.Invoke(this, EventArgs.Empty);
                    break;
                case FuelLevelState.Low:
                    OnLowFuelLevel?.Invoke(this, EventArgs.Empty);
                    break;
                case FuelLevelState.Critical:
                    OnCriticalFuelLevel?.Invoke(this, EventArgs.Empty);
                    break;
                case FuelLevelState.Empty:
                    OnOutOfFuel?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }
}