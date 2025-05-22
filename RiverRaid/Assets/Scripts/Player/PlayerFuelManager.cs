using System;
using System.Collections;
using UnityEngine;

public class PlayerFuelManager : MonoBehaviour
{
    public event EventHandler OnOutOfFuel;
    public event EventHandler<float> OnCurrentFuelChanged;

    public float MaxFuel;
    public float FuelConsumptionPerTick;
    public float FuelConsumptionTickRate;

    private float _currentFuel;
    private Coroutine _fuelConsumptionCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnStartLevel += HandleStartLevel;
        GameManager.Instance.OnLevelEnds += HandleLevelEnds;
    }

    public void AddFuelPercentage(float fuelPercentage) => _currentFuel = Mathf.Clamp(_currentFuel + _currentFuel * fuelPercentage, 0, MaxFuel);

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

            if (_currentFuel <= 0.0f)
                OnOutOfFuel?.Invoke(this, EventArgs.Empty);
        }
    }
}