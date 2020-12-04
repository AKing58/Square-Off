﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> Characters = new Dictionary<string, GameObject>();

    //public Vector3[] spawnLocations =
    //{
    //    new Vector3(-2.5f, 0, 2.5f),
    //    new Vector3(2.5f, 0, 2.5f),
    //    new Vector3(-2.5f, 0, -2.5f),
    //    new Vector3(2.5f, 0, -2.5f)
    //};

    public List<GameObject> Players;
    //public Avatar RedCometAvatar;

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
        //Debug.Log("Player configs.length + " + playerConfigs.Length);

        //instantiating characters should depend on playerConfigs[i].CharacterName
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            
            GameObject player = Instantiate(Characters[playerConfigs[i].CharacterName], playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);

            //player.AddComponent<Animator>();
            player.name = "Player" + (Players.Count + 1);

            //Animator playerAnim = player.GetComponent<Animator>();
            
            //playerAnim.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("Models/RedCometStuff/RedCometPController"));
            //playerAnim.avatar = Instantiate(RedCometAvatar);

            Players.Add(player);
               
            PlayerHandler ph = player.GetComponent<PlayerHandler>();

            ph.controllable = true;
            ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
            ph.CharacterName = playerConfigs[i].CharacterName;
            ph.PlayerName = "Player " + Players.Count;

            ph.InitPlayer(playerConfigs[i]);
            ph.GM = this;
            Players[Players.Count - 1].GetComponent<PlayerHandler>().RotTowards(Vector3.zero);
            //var player = Instantiate(RedCometAvatar, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            //player.GetComponent<PlayerHandler>().InitializePlayer(playerConfigs[i]);
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

        switch(Random.Range(0,2))
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

    private IEnumerator EndMatch() {
        yield return new WaitForSeconds(2.0f);

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
    }

    private void RemoveExtraPlayerPanels() {
        if (Players.Count != Constants.MAX_PLAYERS) {
            for (int i = Players.Count+1; i <= Constants.MAX_PLAYERS; i++) {
                PlayerInfoPanels.transform.Find("Player" + i + "Panel").gameObject.SetActive(false);
            }
        }
    }

    public void TempDisableControls()
    {
        foreach(GameObject go in Players)
        {
            StartCoroutine(Pause(go));
        }
    }

    public IEnumerator Pause(GameObject go)
    {
        go.GetComponent<PlayerHandler>().targetVec = Vector3.zero;
        go.GetComponent<PlayerHandler>().controllable = false;
        yield return new WaitForSeconds(5.5f); 
        go.GetComponent<PlayerHandler>().controllable = true;
    }

    //public void spawnPlayer()
    //{
    //    GameObject player = Instantiate(Characters["BlueMoon"], spawnLocations[Players.Count], Quaternion.identity);
        
    //    player.AddComponent<Animator>();
    //    player.name = "Player" + (Players.Count+1);

    //    Animator playerAnim = player.GetComponent<Animator>();
    //    playerAnim.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("Models/BlueMoon/BlueMoonPController"));
    //    playerAnim.avatar = Instantiate(RedCometAvatar);

    //    ph.controllable = true;
    //    ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
    //    ph.CharacterName = "Blue Moon";
    //    ph.PlayerName = "Player " + Players.Count;
    //    ph.InitPlayer();
    //}


    //public void spawnPlayer()
    //{
    //    GameObject player = Instantiate(Characters["RedComet"], spawnLocations[Players.Count], Quaternion.identity);
        
    //    player.AddComponent<Animator>();
    //    player.name = "Player" + (Players.Count+1);

    //    Animator playerAnim = player.GetComponent<Animator>();
    //    playerAnim.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("Models/RedCometStuff/RedCometPController"));
    //    playerAnim.avatar = Instantiate(RedCometAvatar);

    //    Players.Add(player);
    //    PlayerHandler ph = player.GetComponent<PlayerHandler>();


    //    ph.controllable = true;
    //    ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
    //    ph.CharacterName = "Red Comet";
    //    ph.PlayerName = "Player " + Players.Count;
        
    //    ph.InitPlayer();
    //}

    //}


    

    //public void spawnRedCometAI()
    //{
    //    GameObject player = Instantiate(Characters["RedComet"], spawnLocations[Players.Count], Quaternion.identity);
    //    player.name = "RedCometAI";
    //    Destroy(player.GetComponent<PlayerInput>());
    //    player.AddComponent<RedCometAI>();
    //    player.GetComponent<RedCometAI>().InitializeAI(this);
    //    player.AddComponent<Animator>();
    //    player.name = "RedCometAI" + (Players.Count + 1);

    //    Animator playerAnim = player.GetComponent<Animator>();
    //    playerAnim.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("Models/RedCometStuff/RedCometPController"));
    //    playerAnim.avatar = Instantiate(RedCometAvatar);

    //    Players.Add(player);
    //    PlayerHandler ph = player.GetComponent<PlayerHandler>();

    //    ph.controllable = false;
    //    ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
    //    ph.CharacterName = "Red Comet";
    //    ph.PlayerName = "AI " + Players.Count;
    //    ph.InitPlayer();
    //}

    //public void spawnPlayers() 
    //{
    //    Players.Add(Instantiate(Characters["RedComet"], new Vector3(-2.5f, 0, 0), Quaternion.identity));
    //    Players[Players.Count-1].name = "Player1";

    //    PlayerHandler ph = Players[Players.Count - 1].GetComponent<PlayerHandler>();

    //    ph.controllable = true;
    //    ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player1Panel").gameObject;
    //    ph.CharacterName = "Red Comet";
    //    ph.PlayerName = "Player 1";
    //    ph.InitPlayer();
    //}

    //public void spawnPlayer2() {
    //    Players.Add(Instantiate(Characters["RedComet"], new Vector3(2.5f, 0, 0), Quaternion.identity));
    //    Players[Players.Count - 1].name = "Player2";

    //    PlayerHandler ph = Players[Players.Count - 1].GetComponent<PlayerHandler>();
    //    ph.controllable = true;
    //    ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player2Panel").gameObject;
    //    ph.CharacterName = "Red Comet";
    //    ph.PlayerName = "Player 2";
    //    ph.InitPlayer();

    //    Players[Players.Count - 1].GetComponent<PlayerHandler>().rotTowards(Vector3.zero);   
    //}

    //public void spawnRedCometAI1()
    //{
    //    Players.Add(Instantiate(Characters["RedComet"], new Vector3(2.5f, 0, 0), Quaternion.identity));
    //    Players[Players.Count - 1].name = "RedCometAI";
    //    Destroy(Players[Players.Count - 1].GetComponent<PlayerInput>());
    //    Players[Players.Count - 1].AddComponent<RedCometAI>();
    //    Players[Players.Count - 1].GetComponent<RedCometAI>().InitializeAI(this);

    //    PlayerHandler ph = Players[1].GetComponent<PlayerHandler>();

    //    ph.controllable = false;
    //    ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player2Panel").gameObject;
    //    ph.CharacterName = "Red Comet";
    //    ph.PlayerName = "RedCometAI";
    //    ph.InitPlayer();
    //}

    public void RestartScene()
    {
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

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
        if (PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray().Length == 1)
        {
            VictoryScreen.transform.Find("Panel/Wins").gameObject.SetActive(true);
        }
        else {
            VictoryScreen.transform.Find("Panel/Wins").gameObject.SetActive(false);
        }
       
        VictoryScreen.SetActive(true);
        VictoryScreen.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
    }
}
