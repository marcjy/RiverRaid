using UnityEngine;

public class StatsTracker : MonoBehaviour
{
    public static StatsTracker Instance;

    private int _levelsCleared;
    private int _enemiesKilled;
    private int _fuelCanistersCollected;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearStats();

        GameManager.Instance.OnStartNewGame += HandleStartNewGame;

        LevelManager.OnReachedNewLevel += HandleNewLevelReached;
    }


    #region Event Handling
    private void HandleStartNewGame(object sender, System.EventArgs e) => ClearStats();

    private void HandleNewLevelReached(object sender, System.EventArgs e) => _levelsCleared++;
    #endregion

    public GameStats GetGameStats() => new GameStats(_levelsCleared, _enemiesKilled, _fuelCanistersCollected);

    private void ClearStats()
    {
        _levelsCleared = 0;
        _enemiesKilled = 0;
        _fuelCanistersCollected = 0;
    }


    public class GameStats
    {
        public GameStats(int levelsCleared, int enemiesKilled, int fuelCanistersCollected)
        {
            LevelsCleared = levelsCleared;
            EnemiesKilled = enemiesKilled;
            FuelCanistersCollected = fuelCanistersCollected;
        }

        public int LevelsCleared { get; private set; }
        public int EnemiesKilled { get; private set; }
        public int FuelCanistersCollected { get; private set; }
    }
}


