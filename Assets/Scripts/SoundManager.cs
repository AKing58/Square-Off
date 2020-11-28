﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Music { 
        MainMenu,
        CharSelect,
        GridStage,
        RoundStage
    }

    public enum SFX { 
        UISelect,
        UIConfirm,
        UICancel
    }

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void PlayMusic(Music sound) {
        GameObject soundGameObject = new GameObject("BGM");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.05f;
        audioSource.spatialBlend = 0.8f;
        audioSource.loop = true;
        audioSource.clip = GetAudioClip(sound);
        audioSource.Play();
    }

    public static void PlayOneShot(SFX sound) {
        if (oneShotGameObject == null) {
            oneShotGameObject = new GameObject("Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            oneShotAudioSource.volume = 0.05f;
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