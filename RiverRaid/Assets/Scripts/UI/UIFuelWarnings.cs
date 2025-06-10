using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIFuelWarnings : MonoBehaviour
{
    public GameObject FuelWarning;
    public TextMeshProUGUI FuelWarningText;

    public Volume PostProcesingVolume;
    public float FlashScreenInterval = 1.0f;
    private Coroutine _flashScreenCoroutine;


    private PlayerFuelManager _playerFuelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerFuelManager = GameManager.Instance.Player.GetComponent<PlayerFuelManager>();
        _playerFuelManager.OnNormalFuelLevel += HandleNormalFuelLevel;
        _playerFuelManager.OnLowFuelLevel += HandleLowFuelLevel;
        _playerFuelManager.OnCriticalFuelLevel += HandleCriticalFuelLevel;

        GameManager.Instance.OnEndGame += HandleEndGame;
    }

    #region Event Handling
    private void HandleNormalFuelLevel(object sender, System.EventArgs e)
    {
        HideLowFuelLevelWarning();
        DisableFlashScreen();
    }
    private void HandleLowFuelLevel(object sender, System.EventArgs e)
    {
        ShowLowFuelLevelWarning();
        FuelWarningText.text = "LOW FUEL LEVEL";
    }
    private void HandleCriticalFuelLevel(object sender, System.EventArgs e)
    {
        FuelWarningText.text = "CRITICAL FUEL LEVEL";
        _flashScreenCoroutine = StartCoroutine(FlashScreen());
    }

    private void HandleEndGame(object sender, System.EventArgs e)
    {
        HideLowFuelLevelWarning();

        if (_flashScreenCoroutine != null)
            DisableFlashScreen();
    }

    #endregion

    private void ShowLowFuelLevelWarning() => FuelWarning.SetActive(true);
    private void HideLowFuelLevelWarning() => FuelWarning.SetActive(false);
    private void DisableFlashScreen()
    {
        PostProcesingVolume.weight = 0.0f;
        StopCoroutine(_flashScreenCoroutine);
    }

    private IEnumerator FlashScreen()
    {
        while (true)
        {
            yield return FadePostProcesing(0.75f, 1.0f, FlashScreenInterval);
            yield return FadePostProcesing(1.0f, 0.75f, FlashScreenInterval);
        }
    }
    private IEnumerator FadePostProcesing(float from, float target, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            PostProcesingVolume.weight = Mathf.Lerp(from, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        PostProcesingVolume.weight = target;
    }
}
