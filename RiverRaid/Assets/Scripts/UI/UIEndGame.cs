using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndGame : MonoBehaviour
{
    public RectTransform EndGameWindow;

    [Header("Stat Values")]
    public TextMeshProUGUI LevelsClearedText;
    public TextMeshProUGUI EnemiesKilledText;
    public TextMeshProUGUI FuelCanistersCollectedText;

    [Header("Buttons")]
    public Button PlayAgainButton;
    public Button QuitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnStartNewGame += HandleStartNewGame;
        GameManager.Instance.OnEndGame += HandleEndGame;

        InitButtons();
    }

    #region Event Handling
    private void HandleStartNewGame(object sender, System.EventArgs e)
    {
        ResetStatsValues();
        HideEndGameWindow();
    }
    private void HandleEndGame(object sender, System.EventArgs e)
    {
        StatsTracker.GameStats stats = StatsTracker.Instance.GetGameStats();
        SetStatsValues(stats);

        ShowEndGameWindow();
    }
    #endregion

    private void InitButtons()
    {
        PlayAgainButton.onClick.AddListener(() => UIEvents.TriggerPlayAgain());
        QuitButton.onClick.AddListener(() => UIEvents.TriggerQuit());
    }

    private void SetStatsValues(StatsTracker.GameStats stats)
    {
        LevelsClearedText.text = stats.LevelsCleared.ToString();
        EnemiesKilledText.text = stats.EnemiesKilled.ToString();
        FuelCanistersCollectedText.text = stats.FuelCanistersCollected.ToString();
    }
    private void ResetStatsValues()
    {
        LevelsClearedText.text = 0.ToString();
        EnemiesKilledText.text = 0.ToString();
        FuelCanistersCollectedText.text = 0.ToString();
    }

    private void ShowEndGameWindow() => EndGameWindow.gameObject.SetActive(true);
    private void HideEndGameWindow() => EndGameWindow.gameObject.SetActive(false);
}
