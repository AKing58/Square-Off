using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//contains functions for displaying text on the Character Select Scene
public class CharacterSelectManager : MonoBehaviour
{
    public Text joinText;
    // Start is called before the first frame update

    void Start() {
        if (PlayerConfigurationManager.Instance != null) {
            PlayerConfigurationManager.Instance.EnableJoining();
        }
       
    }

    //Toggles join text on Character Select Scene
    public void showJoinText() {
        if (PlayerConfigurationManager.Instance.GetPlayerConfigs().Count == 0)
        {
            joinText.enabled = true;

        }
        else
        {
            joinText.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        showJoinText();
    }
}
