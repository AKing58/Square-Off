using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeHandler : HitBoxScript
{
    public string strikeDirection;
    void OnTriggerStay(Collider collider)
    {
        Debug.Log(collider.name);
        PlayerHandler targetPH = collider.gameObject.GetComponent<PlayerHandler>();
        if (!targetPH.canBeStriked())
            return;
        //targetPH.rotTowards(selfPH.transform.position);
        targetPH.strikeMe();
        GetComponent<BoxCollider>().enabled = false;
    }
}
