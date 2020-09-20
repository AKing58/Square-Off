using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public float grabAnimDistance = 3.0f;
    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), transform.parent.GetComponent<Collider>());
    }
    private void OnTriggerEnter(Collider collider)
    {
        PlayerHandler targetPH = collider.gameObject.transform.Find("BaseModel").GetComponent<PlayerHandler>();
        PlayerHandler selfPH = transform.parent.Find("BaseModel").GetComponent<PlayerHandler>();
        if (!targetPH.canBeGrabbed())
            return;

        Vector3 grabLoc = Vector3.MoveTowards(targetPH.transform.parent.position, transform.position, 100.0f);
        targetPH.transform.parent.position = new Vector3(grabLoc.x, targetPH.transform.parent.position.y, grabLoc.z);
        targetPH.rotTowards(selfPH.gameObject.transform.position);
        targetPH.grabMe();
        selfPH.grabConnected();
    }
}
