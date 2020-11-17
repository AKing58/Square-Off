using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;
    [SerializeField]
    private TextMeshProUGUI titleText = null;
    [SerializeField]
    private GameObject readyPanel = null;
    [SerializeField]
    private GameObject menuPanel = null;
    [SerializeField]
    private Button readyButton = null;

    [SerializeField]
    private Button defaultCharacter = null;

    private float ignoreInputTime = 1.0f;

    private bool inputEnabled;

    public void SetPlayerIndex(int pi) 
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetCharacter(string charName)
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.SetPlayerCharacter(PlayerIndex, charName);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
    }

    public void ReturnToCharacterSelect() {
        readyPanel.SetActive(false);
        menuPanel.SetActive(true);
        defaultCharacter.Select();
    }
}
