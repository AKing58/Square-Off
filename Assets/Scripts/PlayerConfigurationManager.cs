using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int MaxPlayers = 2;

    public static PlayerConfigurationManager Instance { get; private set; }

    //private int playerReadyCount = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance of singleton!!");
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerCharacter(int index, string charName)
    {

        playerConfigs[index].CharacterName = charName;
    }

    public void ReadyPlayer(int index) {
        playerConfigs[index].IsReady = true;

        //debugging for checking number of players
        //playerReadyCount = 0;
        //for (int i = 0; i < playerConfigs.Count; i++) {
        //    if (playerConfigs[i].IsReady) {
        //        playerReadyCount++;
        //    }
        //}
        //Debug.Log("Max players:" + MaxPlayers + " Number of players ready: " + playerReadyCount);

        if (playerConfigs.Count > MaxPlayers) 
        {
            MaxPlayers = playerConfigs.Count;
        }

        //load next scene if all players are ready
        if (playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player # " + pi.playerIndex + "Joined ");
        pi.transform.SetParent(transform);
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex)) 
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }

       
    }
}

public class PlayerConfiguration
{

    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }
    
    public bool IsReady { get; set; } // player is ready to move onto the next scene

    public string CharacterName;

    //public Material PlayerMaterial { get; set; }
}