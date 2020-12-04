using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Sound Manager handles playing music and SFX. Players play from their own audiosource while other 
 * sources play from the audiosources defined here.
 */
public static class SoundManager
{
    //list of music able to be chosen
    public enum Music { 
        MainMenus,
        GridStage,
        RoundStage
    }

    //list of SFX able to be chosen
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
        Dizzy,
        SuperZoop,
        Brakes,
        Crash,
        Vroom,
        Vroom2,
        BMSuperActivate,
        BMSuperEnd,
        RCSuperActivate,
        RCSuperEnd,
        Flamethrower,
        RoundEnd
    }

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    //game object holding the audiosource of background music of every scene
    private static GameObject MenuBGM;

    //tells the sound manager when the scene is battle scene to handle when to replay music 
    private static bool fightMode = false;

    //play background music
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
            AudioSource audioSrc = MenuBGM.gameObject.GetComponent<AudioSource>();
            audioSrc.clip = GetAudioClip(sound);
            audioSrc.Play();
            fightMode = fightOn;
        }              
    }

    public static void StopMusic() {
        AudioSource audiosrc = MenuBGM.gameObject.GetComponent<AudioSource>();
        audiosrc.Stop();      
    }

    //play sfx from UI
    public static void PlayOneShotUI(SFX sound, float volume = 0.10f)
    {
        if (oneShotGameObject == null)
        {
            oneShotGameObject = new GameObject("Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            oneShotAudioSource.volume = volume;
            oneShotAudioSource.spatialBlend = 0f;
        }

        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static AudioClip GetAudioClip(SFX sound)
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
