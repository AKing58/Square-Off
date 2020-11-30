using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float fireDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerHandler>().CanBeStriked())
        {
            other.gameObject.GetComponent<PlayerHandler>().Health -= fireDamage;
            other.gameObject.GetComponent<PlayerHandler>().StrikeMe();

            if (other.gameObject.GetComponent<PlayerHandler>().Health <= 0)
            {
                other.gameObject.GetComponent<PlayerHandler>().StageKillMe();
            }
        }
    }
}
