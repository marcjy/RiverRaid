using System;

public static class UIEvents
{
    public static event EventHandler OnStartGameAnimationCompleted;

    public static void TriggerIntroAnimaitonCompleted() => OnStartGameAnimationCompleted?.Invoke(null, EventArgs.Empty);
}
