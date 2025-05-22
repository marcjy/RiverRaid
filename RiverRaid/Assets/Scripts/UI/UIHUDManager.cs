using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHUDManager : MonoBehaviour
{
    public GameObject HUD;
    public float FadeInAnimationDuration = 1.0f;
    private CanvasGroup _canvasGroupHUD;

    [Header("Lives")]
    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    private int _maxHearts;
    private int _currentFullHeartsIndex;

    [Header("Fuel")]
    public Slider FuelSlider;
    public TextMeshProUGUI FuelValue;

    [Header("Score")]
    public TextMeshProUGUI ScoreValue;

    private PlayerFuelManager _playerFuelManager;

    private void Awake()
    {
        _canvasGroupHUD = HUD.GetComponent<CanvasGroup>();
        _canvasGroupHUD.alpha = 0.0f;

        _maxHearts = Hearts.Length;
        _currentFullHeartsIndex = _maxHearts - 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerFuelManager = GameManager.Instance.Player.GetComponent<PlayerFuelManager>();
        _playerFuelManager.OnCurrentFuelChanged += HandleCurrentFuelChanged;

        GameManager.Instance.OnStartNewGame += HandleStartNewGame;
        GameManager.Instance.OnStartLevel += HandleStartLevel;
        GameManager.Instance.OnLevelEnds += HandlePlayerLosesLive;

        GameManager.Instance.OnResetGame += HandleResetGame;

        StatsTracker.OnScoreChanges += HandleNewScoreValue;
    }

    #region Event Handling
    private void HandleStartNewGame(object sender, System.EventArgs e)
    {
        ResetLivesUI();
        ResetFuelUI();
        ResetScoreUI();

        StartCoroutine(FadeInHUD());
    }
    private void HandleStartLevel(object sender, System.EventArgs e) => ResetFuelUI();
    private void HandleResetGame(object sender, System.EventArgs e) => _canvasGroupHUD.alpha = 0.0f;

    private void HandlePlayerLosesLive(object sender, System.EventArgs e) => LoseHeart();
    private void HandleCurrentFuelChanged(object sender, float currentFuel)
    {
        FuelValue.text = Mathf.RoundToInt(currentFuel).ToString();
        FuelSlider.value = currentFuel / 100;
    }
    private void HandleNewScoreValue(object sender, int newScoreValue) => ScoreValue.text = newScoreValue.ToString();
    #endregion

    private void ResetLivesUI()
    {
        foreach (Image image in Hearts)
            image.sprite = FullHeart;

        _currentFullHeartsIndex = _maxHearts - 1;
    }
    private void ResetFuelUI()
    {
        FuelSlider.value = 1;
        FuelValue.text = "100";
    }
    private void ResetScoreUI()
    {
        ScoreValue.text = "0";
    }

    private IEnumerator FadeInHUD()
    {
        float elapsedTime = 0.0f;
        float initialAlpha = 0.0f;
        float targetAlpha = 1.0f;

        while (elapsedTime < FadeInAnimationDuration)
        {
            _canvasGroupHUD.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / FadeInAnimationDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _canvasGroupHUD.alpha = targetAlpha;
    }

    private void LoseHeart()
    {
        Hearts[_currentFullHeartsIndex].sprite = EmptyHeart;
        _currentFullHeartsIndex--;
    }
}
