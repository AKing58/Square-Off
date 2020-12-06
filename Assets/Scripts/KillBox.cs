using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    //Kills the player when they touch the killbox
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerHandler>())
        {
            collision.gameObject.GetComponent<PlayerHandler>().StageKillMe();
        }
    }

    //Used to destroy the tiles in squareagone
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tile"))
        {
            Destroy(other.gameObject);
        }
    }
}
