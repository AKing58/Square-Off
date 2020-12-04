using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/*
 Singleton that Handles pairing the player/controller with information that they've chosen about the game such as their character.
 */
public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    //determines max number of players currently in the game
    [SerializeField]
    private int MaxPlayers = 1;

    
    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && this != Instance)
        {
            //Destroys the copy of this object in the Character Select Scene when reloading
            Destroy(gameObject);
                 
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

        if (playerConfigs.Count != MaxPlayers) 
        {
            //sets max players to load next scene when all players are ready
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
        
        pi.transform.SetParent(transform);
        //if the player index doesnt exist in playerConfigs, add it and set it as a child
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex)) 
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }

       
    }

    //resets the list of players
    public void ResetPlayers()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        playerConfigs.Clear();
    }

    public void DisableJoining() {
        gameObject.GetComponent<PlayerInputManager>().DisableJoining();
    }

    public void EnableJoining() {
        gameObject.GetComponent<PlayerInputManager>().EnableJoining();
    }
}

//the player configuration object storing the controller and their unique player index
public class PlayerConfiguration
{
    
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }

    // player is ready to move onto the next scene for Character Select
    public bool IsReady { get; set; } 

    //the character they have chosen in the previous scene
    public string CharacterName;

}