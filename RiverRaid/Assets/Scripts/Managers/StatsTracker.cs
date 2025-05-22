using System;

public static class StatsTracker
{
    public static event EventHandler<int> OnScoreChanges;

    private static int _score;
    private static int _levelsCleared;
    private static int _enemiesKilled;
    private static int _fuelCanistersCollected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Init()
    {
        ClearStats();

        GameManager.Instance.OnStartNewGame += HandleStartNewGame;
        GameManager.Instance.OnResetGame += HandleResetGame;

        LevelManager.OnReachedNewLevel += HandleNewLevelReached;

        CollectableGenerator.OnCollectibleSpawn += HandleCollectibleSpawned;
        CollectableGenerator.OnCollectibleReleased += HandleCollectibleReleased;

        EnemyGenerator.OnEnemySpawn += HandleEnemySpawned;
        EnemyGenerator.OnEnemyReleased += HandleEnemyReleased;
    }

    #region Event Handling
    private static void HandleStartNewGame(object sender, System.EventArgs e) => ClearStats();
    private static void HandleResetGame(object sender, System.EventArgs e) => ClearStats();

    private static void HandleNewLevelReached(object sender, System.EventArgs e) => _levelsCleared++;

    private static void HandleCollectibleSpawned(object sender, BaseCollectible collectible)
    {
        //Check if the collectible can be destroyed. If it can, deduct the score by the corresponding value.
        if (collectible.TryGetComponent(out BaseDestructible baseDestructible))
            baseDestructible.OnDestroyed += HandleCollectibleDestroyed;

        collectible.OnCollected += HandleCollectibleCollected;
    }
    private static void HandleCollectibleReleased(object sender, BaseCollectible collectible)
    {
        if (collectible.TryGetComponent(out BaseDestructible baseDestructible))
            baseDestructible.OnDestroyed -= HandleCollectibleDestroyed;

        collectible.OnCollected -= HandleCollectibleCollected;
    }
    private static void HandleCollectibleCollected(object sender, int score)
    {
        _score += score;
        OnScoreChanges?.Invoke(null, _score);

        if (sender.GetType() == typeof(FuelCanister))
            _fuelCanistersCollected++;
    }
    private static void HandleCollectibleDestroyed(object sender, int scorePenalization)
    {
        _score += scorePenalization;
        OnScoreChanges?.Invoke(null, _score);
    }

    private static void HandleEnemySpawned(object sender, BaseEnemyBehaviour enemy)
    {
        if (enemy.TryGetComponent(out EnemyDestructible enemyDestructible))
            enemyDestructible.OnDestroyed += HandleEnemyDestroyed;
    }
    private static void HandleEnemyReleased(object sender, BaseEnemyBehaviour enemy)
    {
        if (enemy.TryGetComponent(out EnemyDestructible enemyDestructible))
            enemyDestructible.OnDestroyed -= HandleEnemyDestroyed;
    }
    private static void HandleEnemyDestroyed(object sender, int score)
    {
        _score += score;
        OnScoreChanges?.Invoke(null, _score);

        _enemiesKilled++;
    }
    #endregion

    public static GameStats GetGameStats() => new GameStats(_levelsCleared, _enemiesKilled, _fuelCanistersCollected);
    private static void ClearStats()
    {
        _score = 0;
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


