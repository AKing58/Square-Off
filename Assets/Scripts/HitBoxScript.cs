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
        if(parentGO.GetComponent<PlayerHandler>().CurrentMove == null)
        {
            GetComponent<BoxCollider>().enabled = false;
            return;
        }
        PlayerHandler targetPH = collider.gameObject.GetComponent<PlayerHandler>();
        if (targetPH == null)
            return;
        PlayerHandler selfPH = parentGO.GetComponent<PlayerHandler>();

        if (selfPH.AttackOther(targetPH, selfPH.CurrentMove.Name))
            GetComponent<BoxCollider>().enabled = false;
    }
}
