using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> Characters = new Dictionary<string, GameObject>();

    public List<GameObject> Players;

    public GameObject PlayerInfoPanels;

    private void Awake()
    {
        Characters.Add("RedComet", Resources.Load<GameObject>("GameObjects/Characters/RedComet"));
    }

    // Start is called before the first frame update
    void Start()
    {
        Players.Add(Instantiate(Characters["RedComet"], new Vector3(-2.5f ,0 ,0 ), Quaternion.identity));
        Players[0].name = "Player1";
        Players[0].GetComponent<PlayerHandler>().controllable = true;
        Players[0].GetComponent<PlayerHandler>().PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player1Panel").gameObject;
        Players[0].GetComponent<PlayerHandler>().CharacterName = "Red Comet";
        Players[0].GetComponent<PlayerHandler>().PlayerName = "Player 1";
        Players[0].GetComponent<PlayerHandler>().InitPlayer();

        Players.Add(Instantiate(Characters["RedComet"], new Vector3(2.5f, 0, 0), Quaternion.identity));
        Players[1].name = "Player2";
        Players[1].GetComponent<PlayerHandler>().PlayerInfoPanel = PlayerInfoPanels.transform.Find("Player2Panel").gameObject;
        Players[1].GetComponent<PlayerHandler>().CharacterName = "Red Comet";
        Players[1].GetComponent<PlayerHandler>().PlayerName = "Player 2";
        Players[1].GetComponent<PlayerHandler>().InitPlayer();

        foreach (GameObject player in Players)
        {
            player.GetComponent<PlayerHandler>().rotTowards(Vector3.zero);
        }
    }
}
