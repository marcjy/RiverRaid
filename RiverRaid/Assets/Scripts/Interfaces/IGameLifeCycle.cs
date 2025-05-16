using System;

public interface IGameLifeCycle
{
    public event EventHandler OnStartNewGame;

    public event EventHandler OnStartLevel;
    public event EventHandler OnResetLevel;

    public event EventHandler OnEndGame;
    public event EventHandler OnResetGame;
}
