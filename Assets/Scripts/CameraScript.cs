using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameManager gm;

    Camera thisCam;

    public List<Transform> targets;
    private Vector3 offset;
    public float smoothTime = 0.25f;

    private Vector3 velocity;

    // Start is called before the first frame update
    public void InitCam()
    {
        offset = new Vector3(0, 5f, -7.5f);
        thisCam = GetComponent<Camera>();
        foreach(GameObject go in gm.Players)
        {
            targets.Add(go.transform);
        }
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;
        move();
    }

    void move()
    {
        Vector3 center = GetCenterPoint();
        Vector3 newPosition = center + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
    
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            if(targets[i].GetComponent<PlayerHandler>().Health >= 0)
                bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}
