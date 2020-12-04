using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Contains functions for loading scenes on the Title Screen
public class TitleManager : MonoBehaviour
{

    public void Start() {
        if (PlayerConfigurationManager.Instance != null) {
            PlayerConfigurationManager.Instance.DisableJoining();
        }
        SoundManager.PlayMusic(SoundManager.Music.MainMenus);
    }

    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
