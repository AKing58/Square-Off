using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour
{
    public GameObject parentGO;

    protected void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), transform.GetComponent<Collider>());
    }

    void OnTriggerStay(Collider collider)
    {
        PlayerHandler targetPH = collider.gameObject.GetComponent<PlayerHandler>();
        PlayerHandler selfPH = parentGO.GetComponent<PlayerHandler>();

        if (selfPH.AttackOther(targetPH, selfPH.CurrentMove.Name))
            GetComponent<BoxCollider>().enabled = false;
    }
}
