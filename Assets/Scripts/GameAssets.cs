using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This class is used to store reference audio clips and keep track of scores
 */
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

    //store red comet wins
    private static int RCWins;
    //blue moon wins
    private static int BMWins;


    public static void Initialize() {
        LoadScore();
    }

    public static void LoadScore() {      
        RCWins = PlayerPrefs.GetInt("RCWins");
        BMWins = PlayerPrefs.GetInt("BMWins");
    }

    public static void SaveScore(string charName) {    
        PlayerPrefs.SetInt("RCWins", RCWins);
        PlayerPrefs.SetInt("BMWins", BMWins);
    }

    public static void UpdateScore(bool win, string charName) {
        if (win) {
            if (charName == "RedComet")
            {
                Debug.Log("More wins for Red Comet");
                RCWins++;
            }
            else
            {
                Debug.Log("More wins for Blue Moon");
                BMWins++;
            }
            SaveScore(charName);
        }         
    }

    public static int GetScore(string charName) {
        if (charName == "RedComet")
        {
            return RCWins;
        }
        else {
            return BMWins;
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
