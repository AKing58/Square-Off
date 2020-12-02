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

    private static int RCWins;
    private static int BMWins;


    public static void Initialize() {
        LoadScore();
    }

    public static void LoadScore() {      
        RCWins = PlayerPrefs.GetInt("RCWins");
        BMWins = PlayerPrefs.GetInt("BMWins");
        //Debug.Log("Current Scores are: " + RCWins + " and BlueMoon: " + BMWins);
    }

    public static void SaveScore(string charName) {    
            PlayerPrefs.SetInt("RCWins", RCWins);
        PlayerPrefs.SetInt("BMWins", BMWins);
    }

    public static void UpdateScore(bool win, string charName) {
        //Debug.Log("Char Name is" + charName);
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
        //Debug.Log("Run Get Score");
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
