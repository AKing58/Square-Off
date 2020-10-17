using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : HitBoxScript
{
    private bool isSuper = false;
    public bool IsSuper
    {
        get
        {
            if (isSuper)
            {
                isSuper = false;
                return true;
            }
            return isSuper;
        }
        set
        {
            isSuper = value;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        PlayerHandler targetPH = collider.gameObject.GetComponent<PlayerHandler>();
        PlayerHandler selfPH = parentGO.GetComponent<PlayerHandler>();
        Debug.Log(collider.name);
        if (!targetPH.canBeGrabbed())
            return;

        Vector3 grabLoc = Vector3.MoveTowards(targetPH.transform.position, transform.position, 100.0f);
        targetPH.transform.position = new Vector3(grabLoc.x, targetPH.transform.position.y, grabLoc.z);
        targetPH.rotTowards(selfPH.transform.position);
        if (isSuper)
        {
            targetPH.SetAnimParam("SuperedParam");
            targetPH.gameObject.transform.SetParent(transform);
            targetPH.gameObject.AddComponent<RedCometSuperHandler>();
            selfPH.gameObject.AddComponent<RedCometSuperHandler>();
        }
        else
        {
            targetPH.grabMe();
        }
        selfPH.grabConnected();
    }
}
