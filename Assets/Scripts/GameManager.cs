using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> Characters = new Dictionary<string, GameObject>();

    public List<GameObject> Players;

    public GameObject PlayerInfoPanels;

    public GameObject VictoryScreen;

    [SerializeField]
    private Transform[] playerSpawns = null;

    public Camera MainCam;
    public bool roundEnd = false;
    private bool winnerFound = false;
   

    void Awake()
    {
        Application.targetFrameRate = 60;
        Characters.Add("RedComet", Resources.Load<GameObject>("GameObjects/Characters/RedComet"));
        Characters.Add("BlueMoon", Resources.Load<GameObject>("GameObjects/Characters/BlueMoon"));
    }

    void Start()
    {
        winnerFound = false;
        roundEnd = false;
        MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Players = new List<GameObject>();

        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        //instantiating characters should depend on playerConfigs[i].CharacterName
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            GameObject player = Instantiate(Characters[playerConfigs[i].CharacterName], playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);

            player.name = "Player" + (Players.Count + 1);

            Players.Add(player);
               
            PlayerHandler ph = player.GetComponent<PlayerHandler>();

            ph.controllable = true;
            ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
            ph.CharacterName = playerConfigs[i].CharacterName;
            ph.PlayerName = "Player " + Players.Count;

            ph.InitPlayer(playerConfigs[i]);
            ph.GM = this;
            Players[Players.Count - 1].GetComponent<PlayerHandler>().RotTowards(Vector3.zero);
        }
        if(playerConfigs.Length == 1)
        {
            GameAssets.Initialize();
            GameObject AIChar = Instantiate(Characters["RedComet"], playerSpawns[1].position, playerSpawns[1].rotation, gameObject.transform);
            AIChar.AddComponent<RedCometAI>();
            AIChar.GetComponent<RedCometAI>().InitializeAI(this);

            Players.Add(AIChar);

            PlayerHandler ph = AIChar.GetComponent<PlayerHandler>();

            ph.controllable = !true;
            ph.IsAI = true;
            ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
            ph.CharacterName = "RedComet";
            ph.PlayerName = "AI1";

            ph.InitAI();
            ph.GM = this;
            Players[Players.Count - 1].GetComponent<PlayerHandler>().RotTowards(Vector3.zero);
        }
        RemoveExtraPlayerPanels();
        MainCam.gameObject.GetComponent<CameraScript>().InitCam();
        PlayRandomMusic();
    }

    void PlayRandomMusic() {
        switch (Random.Range(0, 2))
        {
            case 0:
                SoundManager.PlayMusic(SoundManager.Music.GridStage, true);
                break;
            case 1:
                SoundManager.PlayMusic(SoundManager.Music.RoundStage, true);
                break;
            default:
                Debug.LogError("Not 0 or 1");
                break;
        }
    }

    void Update() {
        if (!roundEnd) {
            if (Players.Count > 1)
            {
                int deadPlayers = 0;

                foreach (GameObject player in Players)
                {
                    if (player.GetComponent<PlayerHandler>().Health <= 0)
                    {
                        deadPlayers++;
                    }
                }
                if (deadPlayers >= Players.Count - 1)
                {
                    if (PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray().Length == 1) {
                        PlayerHandler ph = Players[0].GetComponent<PlayerHandler>();
                        if (ph.Health > 0) {
                            GameAssets.UpdateScore(true, ph.CharacterName);
                            
                        }
                    }
                    roundEnd = true;
                    SoundManager.PlayOneShotUI(SoundManager.SFX.RoundEnd, 0.20f);
                    StartCoroutine(EndMatch());
                }
            }
        }
          
    }

    //Used to end the match after the second to last character is killed
    private IEnumerator EndMatch() {
        yield return new WaitForSeconds(2.0f);
        //VictoryScreen.GetComponentInChildren<MultiplayerEventSystem>().firstSelectedGameObject = VictoryScreen.transform.Find("Panel/Restart").gameObject;
    
        PlayerInfoPanels.SetActive(false);
        VictoryScreen.SetActive(true);
        if (PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray().Length == 1)
        {
            PlayerHandler ph = Players[0].GetComponent<PlayerHandler>();
            string winText = ph.CharacterName + " Wins: ";
            winText += GameAssets.GetScore(ph.CharacterName);
            VictoryScreen.transform.Find("Panel/Wins").GetComponent<Text>().text = winText;
        }
        else {
            foreach (GameObject player in Players) {
                if (player.GetComponent<PlayerHandler>().Health > 0) {
                    VictoryScreen.transform.Find("Panel/Wins").GetComponent<Text>().text = player.GetComponent<PlayerHandler>().PlayerName + " Wins!";
                    winnerFound = true;
                }
            }
            if (!winnerFound) {
                VictoryScreen.transform.Find("Panel/Wins").GetComponent<Text>().text = "Squared Off!";
            }
        }
        VictoryScreen.transform.Find("Panel/Restart").gameObject.GetComponent<Button>().Select();
        VictoryScreen.transform.Find("Panel/Restart").gameObject.GetComponent<Button>().OnSelect(null);

        var ph2 = Players[0].gameObject.GetComponent<PlayerHandler>();
        ph2.SetUIInput(VictoryScreen.GetComponentInChildren<InputSystemUIInputModule>());
        pausePlayers(true);
    }
    
    //Removes the player panels of any player not in the game
    private void RemoveExtraPlayerPanels() {
        if (Players.Count != Constants.MAX_PLAYERS) {
            for (int i = Players.Count+1; i <= Constants.MAX_PLAYERS; i++) {
                PlayerInfoPanels.transform.Find("Player" + i + "Panel").gameObject.SetActive(false);
            }
        }
    }

    //Makes players non controllable 
    public void TempDisableControls()
    {
        foreach(GameObject go in Players)
        {
            StartCoroutine(Pause(go));
        }
    }

    //Makes the playerhandler on a GameObject uncrontrollable for a period of time
    public IEnumerator Pause(GameObject go)
    {
        go.GetComponent<PlayerHandler>().targetVec = Vector3.zero;
        go.GetComponent<PlayerHandler>().controllable = false;
        yield return new WaitForSeconds(5.5f); 
        go.GetComponent<PlayerHandler>().controllable = true;
    }

    //Restarts the current scene and resets the Time.timeScale
    public void RestartScene()
    {
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    //Loads a specific string based on the string passed through
    public void LoadScene(string scn) {
        if (!scn.Equals("StageSelect")) {
            PlayerConfigurationManager.Instance.ResetPlayers();
        }
        if (scn.Equals("StageSelect") || scn.Equals("CharacterSelect")) {
            SoundManager.PlayMusic(SoundManager.Music.MainMenus, false);
        }
        SceneManager.LoadScene(scn);
        Time.timeScale = 1.0f;
    }

    //Returns a list of all playerhandler objects in the game
    public List<PlayerHandler> ReturnPlayerCharacters()
    {
        List<PlayerHandler> outputList = new List<PlayerHandler>();
        foreach(GameObject go in Players)
        {
            outputList.Add(go.GetComponent<PlayerHandler>());
        }
        return outputList;
    }

    /// <summary>
    /// Resumes the scene while setting the menu inactive
    /// </summary>
    public void ResumeScene()
    {
        //unpause all player inputs
        pausePlayers(false);

        Time.timeScale = 1.0f;
        if (PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray().Length == 1)
        {
            VictoryScreen.transform.Find("Panel/Wins").gameObject.SetActive(false);
        }      

        VictoryScreen.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);      
        VictoryScreen.SetActive(false);
    }

    /// <summary>
    /// Brings up the menu while pausing the game
    /// </summary>
    public void PauseMenu()
    {
        Time.timeScale = 0f;

        //pause all player inputs
        pausePlayers(true);

        if (PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray().Length == 1)
        {
            VictoryScreen.transform.Find("Panel/Wins").gameObject.SetActive(true);
        }
        else {
            VictoryScreen.transform.Find("Panel/Wins").gameObject.SetActive(false);
        }

        //give of the ui to the player
        var ph = Players[0].gameObject.GetComponent<PlayerHandler>();
        ph.SetUIInput(VictoryScreen.GetComponentInChildren<InputSystemUIInputModule>());

        //set first selected button
       
        VictoryScreen.SetActive(true);
        VictoryScreen.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
        VictoryScreen.transform.Find("Panel/Resume").gameObject.GetComponent<Button>().Select();
        VictoryScreen.transform.Find("Panel/Resume").gameObject.GetComponent<Button>().OnSelect(null);


    }

    private void pausePlayers(bool pause) {
        foreach (GameObject player in Players)
        {
            player.GetComponent<PlayerHandler>().Paused = pause;
        }
    }
}
