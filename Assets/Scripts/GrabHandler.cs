using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : HitBoxScript
{

    void OnTriggerStay(Collider collider)
    {
        PlayerHandler targetPH = collider.gameObject.GetComponent<PlayerHandler>();
        PlayerHandler selfPH = parentGO.GetComponent<PlayerHandler>();
        if (!targetPH.CanBeGrabbed())
            return;

        Vector3 grabLoc = Vector3.MoveTowards(targetPH.transform.position, transform.position, 100.0f);
        targetPH.transform.position = new Vector3(grabLoc.x, targetPH.transform.position.y, grabLoc.z);
        targetPH.RotTowards(selfPH.transform.position);
        if (selfPH.GetComponent<PlayerHandler>().InValidAnim("SuperStart"))
        {
            targetPH.SetAnimParam("SuperedParam");
            targetPH.gameObject.transform.SetParent(transform);
            selfPH.gameObject.AddComponent<RedCometSuperHandler>();
            selfPH.gameObject.GetComponent<RedCometSuperHandler>().Opponent = targetPH.gameObject;
        }
        else
        {
            targetPH.GrabMe();
        }
        selfPH.GrabConnected();
        GetComponent<BoxCollider>().enabled = false;
    }
}
