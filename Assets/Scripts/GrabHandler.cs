using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public GameObject parentGO;
    public float grabAnimDistance = 3.0f;
    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), transform.GetComponent<Collider>());
    }
    private void OnTriggerEnter(Collider collider)
    {
        PlayerHandler targetPH = collider.gameObject.GetComponent<PlayerHandler>();
        PlayerHandler selfPH = parentGO.GetComponent<PlayerHandler>();
        if (!targetPH.canBeGrabbed())
            return;

        Vector3 grabLoc = Vector3.MoveTowards(targetPH.transform.position, transform.position, 100.0f);
        targetPH.transform.position = new Vector3(grabLoc.x, targetPH.transform.position.y, grabLoc.z);
        targetPH.rotTowards(selfPH.transform.position);
        targetPH.grabMe();
        selfPH.grabConnected();
    }
}
