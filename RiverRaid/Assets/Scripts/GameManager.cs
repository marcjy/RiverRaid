using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event EventHandler OnStartNewGame;
    public event EventHandler OnEndGame;
    public event EventHandler OnResetGame;


    private PlayerController _player;
    private Vector3 _playerInitialPosition;

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
    }

    private void HandleStartGameAnimationCompleted(object sender, EventArgs e)
    {
        _player.transform.position = _playerInitialPosition;
        OnStartNewGame?.Invoke(this, EventArgs.Empty);
    }

    private void HandlePlayerDeath(object sender, System.EventArgs e)
    {
        OnEndGame?.Invoke(this, EventArgs.Empty);
    }
}
