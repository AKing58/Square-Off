using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> Characters = new Dictionary<string, GameObject>();

    public List<GameObject> Players;

    public GameObject PlayerInfoPanels;

    private void Awake()
    {
        Characters.Add("RedComet", Resources.Load<GameObject>("GameObjects/Characters/RedComet"));
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

    public void spawnRedCometAI()
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

    // Start is called before the first frame update
    void Start()
    {
        //Players.Add(Instantiate(Characters["RedComet"], new Vector3(-2.5f, 0, 0), Quaternion.identity));
        //Players[0].name = "Player1";

        //PlayerHandler ph1 = Players[0].GetComponent<PlayerHandler>();

        //ph1.controllable = true;
        //ph1.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player1Panel").gameObject;
        //ph1.CharacterName = "Red Comet";
        //ph1.PlayerName = "Player 1";
        //ph1.InitPlayer();

        //Players.Add(Instantiate(Characters["RedComet"], new Vector3(2.5f, 0, 0), Quaternion.identity));
        //Players[1].name = "Player2";

        //PlayerHandler ph2 = Players[1].GetComponent<PlayerHandler>();
        //ph2.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player2Panel").gameObject;
        //ph2.CharacterName = "Red Comet";
        //ph2.PlayerName = "Player 2";
        //ph2.InitPlayer();

        //foreach (GameObject player in Players) 
        //{
        //    player.GetComponent<PlayerHandler>().rotTowards(Vector3.zero);
        //}   
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
