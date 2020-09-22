using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, GameObject> Characters = new Dictionary<string, GameObject>();

    public List<GameObject> Players;

    private void Awake()
    {
        Characters.Add("RedComet", Resources.Load<GameObject>("GameObjects/Characters/RedComet"));
    }

    // Start is called before the first frame update
    void Start()
    {
        Players.Add(Instantiate(Characters["RedComet"], new Vector3(2.5f ,0 ,0 ), Quaternion.identity));
        Players[0].GetComponent<PlayerHandler>().controllable = true;
        Players.Add(Instantiate(Characters["RedComet"], new Vector3(-2.5f, 0, 0), Quaternion.identity));

        foreach(GameObject player in Players)
        {
            player.GetComponent<PlayerHandler>().rotTowards(Vector3.zero);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
