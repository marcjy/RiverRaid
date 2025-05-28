using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public AudioClip MainThemeIntro;
    public AudioClip MainTheme;
    public AudioClip GameOverTheme;
}
