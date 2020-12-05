using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float fireDamage = 10;

    /// <summary>
    /// When player enters area, deals strike damage to them or kills them if they don't have enough health
    /// </summary>
    /// <param name="other"></param>
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
