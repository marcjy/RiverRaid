using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event EventHandler OnStartNewGame;

    public event EventHandler OnStartLevel;
    public event EventHandler OnResetLevel;

    public event EventHandler OnEndGame;
    public event EventHandler OnResetGame;


    private PlayerController _player;
    private Vector3 _playerInitialPosition;
    private bool _playerIsAlive;

    public int PlayerMaxLifes = 3;
    private int _currentPlayerLifes;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _player = FindAnyObjectByType<PlayerController>();
        _playerInitialPosition = _player.transform.position;
    }

    private void Start()
    {
        _player.OnDeath += HandlePlayerDeath;

        UIEvents.OnStartGameAnimationCompleted += HandleStartGameAnimationCompleted;

        UIEvents.OnPlayAgain += HandlePlayAgain;
        UIEvents.OnQuit += HandleQuit;
    }


    #region Event Handling
    private void HandleStartGameAnimationCompleted(object sender, EventArgs e) => StartCoroutine(GameLoop());

    private void HandlePlayerDeath(object sender, System.EventArgs e) => _playerIsAlive = false;

    private void HandlePlayAgain(object sender, EventArgs e) => ResetGame();
    private void HandleQuit(object sender, EventArgs e)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion


    #region GameFlow
    private IEnumerator GameLoop()
    {
        GameStart();

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
        _currentPlayerLifes = PlayerMaxLifes;
        _playerIsAlive = true;

        OnStartNewGame?.Invoke(this, EventArgs.Empty);
    }
    private void RoundStart()
    {
        ResetPlayerPosition();
    }
    private IEnumerator RoundPlaying()
    {
        OnStartLevel?.Invoke(this, EventArgs.Empty);

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
        ResetPlayerPosition();
        OnResetGame?.Invoke(this, EventArgs.Empty);
    }

    private bool HasLivesLeft() => _currentPlayerLifes > 0;
    private bool IsPlayerAlive() => _playerIsAlive;
    private void ResetPlayerPosition() => _player.transform.position = _playerInitialPosition;
    #endregion
}
