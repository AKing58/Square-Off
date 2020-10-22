using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerHandler>())
        {
            collision.gameObject.GetComponent<PlayerHandler>().Health = 0;
        }
    }
}
