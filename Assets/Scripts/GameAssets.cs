using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;


    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameObjects/GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    [System.Serializable]
    public class MusicAudioClip
    {
        public AudioClip audioClip;
        public SoundManager.Music sound;
    }

    [System.Serializable]
    public class SFXAudioClip
    {
        public AudioClip audioClip;
        public SoundManager.SFX sound;
    }

    public MusicAudioClip[] soundAudioClips;
    public SFXAudioClip[] sfxAudioClips;

    

 
}
