using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> Characters = new Dictionary<string, GameObject>();

    public Vector3[] spawnLocations =
    {
        new Vector3(-2.5f, 0, 2.5f),
        new Vector3(2.5f, 0, 2.5f),
        new Vector3(-2.5f, 0, -2.5f),
        new Vector3(2.5f, 0, -2.5f)
    };

    public List<GameObject> Players;
    public Avatar RedCometAvatar;

    public GameObject PlayerInfoPanels;

    void Awake()
    {
        Characters.Add("RedComet", Resources.Load<GameObject>("GameObjects/Characters/RedComet"));
    }

    void Start()
    {
        Players = new List<GameObject>();
    }

    public void spawnPlayer()
    {
        GameObject player = Instantiate(Characters["RedComet"], spawnLocations[Players.Count], Quaternion.identity);
        
        player.AddComponent<Animator>();
        player.name = "Player" + (Players.Count+1);

        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("Models/RedCometStuff/RedCometPController"));
        playerAnim.avatar = Instantiate(RedCometAvatar);

        Players.Add(player);
        PlayerHandler ph = player.GetComponent<PlayerHandler>();

        ph.controllable = true;
        ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
        ph.CharacterName = "Red Comet";
        ph.PlayerName = "Player " + Players.Count;
        ph.InitPlayer();
    }

    public void spawnRedCometAI()
    {
        GameObject player = Instantiate(Characters["RedComet"], spawnLocations[Players.Count], Quaternion.identity);
        player.name = "RedCometAI";
        Destroy(player.GetComponent<PlayerInput>());
        player.AddComponent<RedCometAI>();
        player.GetComponent<RedCometAI>().InitializeAI(this);
        player.AddComponent<Animator>();
        player.name = "RedCometAI" + (Players.Count + 1);

        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.runtimeAnimatorController = (RuntimeAnimatorController)Instantiate(Resources.Load("Models/RedCometStuff/RedCometPController"));
        playerAnim.avatar = Instantiate(RedCometAvatar);

        Players.Add(player);
        PlayerHandler ph = player.GetComponent<PlayerHandler>();

        ph.controllable = false;
        ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player" + Players.Count + "Panel").gameObject;
        ph.CharacterName = "Red Comet";
        ph.PlayerName = "AI " + Players.Count;
        ph.InitPlayer();
    }

    public void spawnPlayers() 
    {
        Players.Add(Instantiate(Characters["RedComet"], new Vector3(-2.5f, 0, 0), Quaternion.identity));
        Players[Players.Count-1].name = "Player1";

        PlayerHandler ph = Players[Players.Count - 1].GetComponent<PlayerHandler>();

        ph.controllable = true;
        ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player1Panel").gameObject;
        ph.CharacterName = "Red Comet";
        ph.PlayerName = "Player 1";
        ph.InitPlayer();
    }

    public void spawnPlayer2() {
        Players.Add(Instantiate(Characters["RedComet"], new Vector3(2.5f, 0, 0), Quaternion.identity));
        Players[Players.Count - 1].name = "Player2";

        PlayerHandler ph = Players[Players.Count - 1].GetComponent<PlayerHandler>();
        ph.controllable = true;
        ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player2Panel").gameObject;
        ph.CharacterName = "Red Comet";
        ph.PlayerName = "Player 2";
        ph.InitPlayer();

        Players[Players.Count - 1].GetComponent<PlayerHandler>().rotTowards(Vector3.zero);   
    }

    public void spawnRedCometAI1()
    {
        Players.Add(Instantiate(Characters["RedComet"], new Vector3(2.5f, 0, 0), Quaternion.identity));
        Players[Players.Count - 1].name = "RedCometAI";
        Destroy(Players[Players.Count - 1].GetComponent<PlayerInput>());
        Players[Players.Count - 1].AddComponent<RedCometAI>();
        Players[Players.Count - 1].GetComponent<RedCometAI>().InitializeAI(this);

        PlayerHandler ph = Players[1].GetComponent<PlayerHandler>();

        ph.controllable = false;
        ph.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player2Panel").gameObject;
        ph.CharacterName = "Red Comet";
        ph.PlayerName = "RedCometAI";
        ph.InitPlayer();
    }

    public void RestartScene()
    {
        Application.LoadLevel(Application.loadedLevel);
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
}
