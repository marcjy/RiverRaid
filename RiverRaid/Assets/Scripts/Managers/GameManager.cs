using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event EventHandler OnStartNewGame;
    //public event EventHandler OnStartLevel;
    public event EventHandler OnResetLevel;
    public event EventHandler OnResetGame;
    public event EventHandler OnEndGame;


    private PlayerController _player;
    private Vector3 _playerInitialPosition;
    private bool _playerIsAlive;

    private bool _isStartGameAnimationCompleted;

    private int _currentPlayerLifes;
    private int _playerMaxLifes;

    private int _levelsCleared;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _player = FindAnyObjectByType<PlayerController>();
        _playerInitialPosition = _player.transform.position;
        _playerMaxLifes = 3;
        _currentPlayerLifes = _playerMaxLifes;
    }

    private void Start()
    {
        _player.OnDeath += HandlePlayerDeath;

        UIEvents.OnStartGameAnimationCompleted += HandleStartGameAnimationCompleted;
        LevelManager.OnReachedNewLevel += HandleReachedNewLevel;

        StartCoroutine(GameLoop());
    }


    #region Event Handling
    private void HandleStartGameAnimationCompleted(object sender, EventArgs e) => _isStartGameAnimationCompleted = true;

    private void HandlePlayerDeath(object sender, System.EventArgs e) => _playerIsAlive = false;
    private void HandleReachedNewLevel(object sender, EventArgs e) => _levelsCleared++;
    #endregion


    #region GameFlow
    private IEnumerator GameLoop()
    {
        GameStart();

        while (!_isStartGameAnimationCompleted)
            yield return null;

        while (HasLivesLeft())
        {
            RoundStart();
            yield return StartCoroutine(RoundPlaying());
            RoundEnd();
        }

        GameEnd();
    }

    private void GameStart()
    {
        _currentPlayerLifes = _playerMaxLifes;
        _playerIsAlive = true;

        _isStartGameAnimationCompleted = false;

        _levelsCleared = 0;
    }
    private void RoundStart()
    {
        ResetPlayerPosition();
    }
    private IEnumerator RoundPlaying()
    {
        while (IsPlayerAlive())
        {
            yield return null;
        }
    }
    private void RoundEnd()
    {
        _currentPlayerLifes--;

        if (_currentPlayerLifes > 0)
        {
            _playerIsAlive = true;
            OnResetLevel?.Invoke(this, EventArgs.Empty);
        }
    }
    private void GameEnd()
    {
        OnEndGame?.Invoke(this, EventArgs.Empty);
    }

    private void ResetGame()
    {
        OnResetGame?.Invoke(this, EventArgs.Empty);
        StartCoroutine(GameLoop());
    }

    private bool HasLivesLeft() => _currentPlayerLifes > 0;
    private bool IsPlayerAlive() => _playerIsAlive;
    private void ResetPlayerPosition() => _player.transform.position = _playerInitialPosition;
    #endregion
}
