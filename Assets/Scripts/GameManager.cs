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
        Players.Add(PlayerInput.Instantiate(Characters["RedComet"], new Vector3(-2.5f, 0, 0), Quaternion.identity));
        Players[0].name = "Player1";

        PlayerHandler ph1 = Players[0].GetComponent<PlayerHandler>();

        ph1.controllable = true;
        ph1.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player1Panel").gameObject;
        ph1.CharacterName = "Red Comet";
        ph1.PlayerName = "Player 1";
        ph1.InitPlayer();
    }

    public void spawnPlayer2() {
        Players.Add(PlayerInput.Instantiate(Characters["RedComet"], new Vector3(2.5f, 0, 0), Quaternion.identity));
        Players[1].name = "Player2";

        PlayerHandler ph2 = Players[1].GetComponent<PlayerHandler>();
        ph2.controllable = true;
        ph2.PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player2Panel").gameObject;
        ph2.CharacterName = "Red Comet";
        ph2.PlayerName = "Player 2";
        ph2.InitPlayer();

        Players[1].GetComponent<PlayerHandler>().rotTowards(Vector3.zero);   
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
}
