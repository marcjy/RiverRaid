using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event EventHandler OnStartNewGame;
    public event EventHandler OnEndGame;
    public event EventHandler OnResetGame;


    public PlayerController Player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Player.OnDeath += HandlePlayerDeath;
    }

    private void HandlePlayerDeath(object sender, System.EventArgs e)
    {

    }
}
