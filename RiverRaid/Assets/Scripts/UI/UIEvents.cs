using System;

public static class UIEvents
{
    // UIStartGame
    public static event EventHandler OnStartGameAnimationCompleted;

    public static void TriggerIntroAnimaitonCompleted() => OnStartGameAnimationCompleted?.Invoke(null, EventArgs.Empty);

    //UIEndGame
    public static event EventHandler OnPlayAgain;
    public static event EventHandler OnQuit;

    public static void TriggerPlayAgain() => OnPlayAgain?.Invoke(null, EventArgs.Empty);
    public static void TriggerQuit() => OnQuit?.Invoke(null, EventArgs.Empty);

}
