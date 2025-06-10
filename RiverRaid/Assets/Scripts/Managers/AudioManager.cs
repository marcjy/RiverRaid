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
        GameManager.Instance.OnResetGame += HandleResetGame;

        PlayerFuelManager fuelManager = GameManager.Instance.Player.GetComponent<PlayerFuelManager>();
        fuelManager.OnLowFuelLevel += HandlePlayerLowFuelLevel;
        fuelManager.OnCriticalFuelLevel += HandlePlayerCriticalFuelLevel;

        PlayerController playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        playerController.OnStartDeath += HandlePlayerDeath;


        PlayMusic(_audioLibrary.MainMenuTheme);
    }


    private static void HandlePlayerLowFuelLevel(object sender, System.EventArgs e) => PlaySFX(_audioLibrary.LowFuelLevel);
    private static void HandlePlayerCriticalFuelLevel(object sender, System.EventArgs e) => PlaySFX(_audioLibrary.CriticalFuelLevel);

    private static void HandleStartNewGame(object sender, System.EventArgs e) => CoroutineRunner.Instance.StartCoroutine(PlayMainTheme());
    private static void HandleEndGame(object sender, System.EventArgs e) => PlayMusic(_audioLibrary.GameOverTheme, false);
    private static void HandleResetGame(object sender, System.EventArgs e) => PlayMusic(_audioLibrary.MainMenuTheme);

    private static void HandlePlayerDeath(object sender, System.EventArgs e) => PlaySFX(_audioLibrary.PlayerDeath);

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
