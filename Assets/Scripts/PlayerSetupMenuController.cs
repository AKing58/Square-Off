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
    //amount of time to ignore input from the player
    private float ignoreInputTime = 0.5f;
    //enables input for the player
    private bool inputEnabled;

    //GameObject holding text for name of the player
    public GameObject charNameText;
    //Gameobject holding text for type of player
    public GameObject charTypeText;
    //Gameobject holding text for stats of the character
    public GameObject charStats;
    //GameObject for character stats box.
    public GameObject characterBox;

    
    public void SetPlayerIndex(int pi) 
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    //ignores inputs to avoid too fast selection
    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    /*
        Handles all the character stat box changing
        Changes Name, Type and Stats(via image)
     */
    public void ChangeCharacterStats(string charName) {
        charNameText.GetComponent<TMP_Text>().text = charName;
        if (charName == "Comet")
        {
            charTypeText.GetComponent<TMP_Text>().text = "Type: Grabber";
            charStats.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameObjects/UI/Images/CometStats");
        }
        else {
            charTypeText.GetComponent<TMP_Text>().text = "Type: Pusher";
            charStats.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameObjects/UI/Images/MoonStats");
        }
    }

    //sets the character of the player and displays next UI
    public void SetCharacter(string charName)
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.SetPlayerCharacter(PlayerIndex, charName);

        characterBox.SetActive(false);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    //Readies the player and disables the ready button
    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
    }

    //When player presses cancel from the ready up button
    public void ReturnToCharacterSelect() {
        readyPanel.SetActive(false);
        menuPanel.SetActive(true);
        characterBox.SetActive(true);
        defaultCharacter.Select();
    }
}
