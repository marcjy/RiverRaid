using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
    private static AudioSource _BGM;
    private static AudioSource _SFX;



    public static void Init()
    {
        _BGM = new GameObject("BGMPlayer").AddComponent<AudioSource>();
        _SFX = new GameObject("SFXPlayer").AddComponent<AudioSource>();

        AudioMixerGroup[] groups = Resources.FindObjectsOfTypeAll(typeof(AudioMixerGroup)) as AudioMixerGroup[];
        _BGM.outputAudioMixerGroup = groups.FirstOrDefault(g => g.name == "BGM");
        _SFX.outputAudioMixerGroup = groups.FirstOrDefault(g => g.name == "SFX");
    }

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
}
