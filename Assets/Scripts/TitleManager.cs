using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public void Start() {
        if (PlayerConfigurationManager.Instance != null) {
            PlayerConfigurationManager.Instance.DisableJoining();
        }
        SoundManager.PlayMusic(SoundManager.Music.MainMenu);
    }

    public void LoadScene(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
