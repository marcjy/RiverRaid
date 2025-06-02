using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
    private static AudioSource _BGM;
    private static AudioSource _SFX;

    private static AudioLibrary _audioLibrary;


    public static void Init(AudioLibrary audioLibrary)
    {
        _audioLibrary = audioLibrary;

        _BGM = new GameObject("BGMPlayer").AddComponent<AudioSource>();
        _SFX = new GameObject("SFXPlayer").AddComponent<AudioSource>();

        AudioMixerGroup[] groups = Resources.FindObjectsOfTypeAll(typeof(AudioMixerGroup)) as AudioMixerGroup[];
        _BGM.outputAudioMixerGroup = groups.FirstOrDefault(g => g.name == "BGM");
        _SFX.outputAudioMixerGroup = groups.FirstOrDefault(g => g.name == "SFX");

        GameManager.Instance.OnStartNewGame += HandleStartNewGame;
        GameManager.Instance.OnEndGame += HandleEndGame;

        PlayMusic(_audioLibrary.MainMenuTheme);
    }

    private static void HandleStartNewGame(object sender, System.EventArgs e) => CoroutineRunner.Instance.StartCoroutine(PlayMainTheme());
    private static void HandleEndGame(object sender, System.EventArgs e) => PlayMusic(_audioLibrary.GameOverTheme, false);

    public static void PlayMusic(AudioClip audioClip, bool enableLoop = true)
    {
        _BGM.clip = audioClip;
        _BGM.loop = enableLoop;

        _BGM.Play();
    }
    public static void PlaySFX(AudioClip audioClip)
    {
        _SFX.PlayOneShot(audioClip);
    }

    private static IEnumerator PlayMainTheme()
    {
        PlayMusic(_audioLibrary.MainThemeIntro, false);

        yield return new WaitForSeconds(_audioLibrary.MainThemeIntro.length);

        PlayMusic(_audioLibrary.MainTheme);
    }
}
