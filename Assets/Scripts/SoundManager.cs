using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SoundManager
{
    public enum Music { 
        MainMenus,
        GridStage,
        RoundStage
    }

    public enum SFX { 
        UISelect,
        UIConfirm,
        UICancel,
        SuperSlam,
        Hit,
        Hit2,
        Hit3,
        HeavyHit,
        HeavyHit2,
        HeavyHit3,
        Dizzy
    }

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;
    private static GameObject MenuBGM;
    private static bool fightMode = false;

    public static void PlayMusic(Music sound, bool fightOn=false) {
        if (!fightOn) {
            fightMode = fightOn;
        }

        if (!fightMode) {
            if (MenuBGM == null)
            {
                MenuBGM = new GameObject("BGM");
                AudioSource audioSource = MenuBGM.AddComponent<AudioSource>();
                audioSource.volume = 0.05f;
                audioSource.spatialBlend = 0.8f;
                audioSource.loop = true;                            
                Object.DontDestroyOnLoad(MenuBGM.gameObject);
               
            }
            AudioSource audiosrc = MenuBGM.gameObject.GetComponent<AudioSource>();
            audiosrc.clip = GetAudioClip(sound);
            audiosrc.Play();
            fightMode = fightOn;
        }              
    }

    public static void DestroyMusic() {
        Object.Destroy(MenuBGM);
    }

    public static void PlayOneShot(SFX sound, float volume=0.05f) {
        if (oneShotGameObject == null) {
            oneShotGameObject = new GameObject("Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            oneShotAudioSource.volume = volume;
            oneShotAudioSource.spatialBlend = 0.8f;
        }
              
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(SFX sound)
    {
        foreach (GameAssets.SFXAudioClip soundAudioClip in GameAssets.i.sfxAudioClips)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    private static AudioClip GetAudioClip(Music sound)
    {
        foreach (GameAssets.MusicAudioClip soundAudioClip in GameAssets.i.soundAudioClips) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

}
