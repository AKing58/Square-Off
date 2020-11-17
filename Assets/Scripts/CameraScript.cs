using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform defaultPosition;

    public GameManager gm;

    [SerializeField]
    private float zoomAmount = 0;

    Camera thisCam;

    Plane[] planes;

    bool shouldZoom = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform;
        thisCam = GetComponent<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(thisCam);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (zoomAmount > 0 && !shouldZoom)
            zoomIn();
        else if (shouldZoom)
        {
            zoomOut();
        }
    }

    bool characterViewable(GameObject go)
    {
        return (GeometryUtility.TestPlanesAABB(planes, go.GetComponent<Collider>().bounds));
    }

    bool allCharactersViewable()
    {
        bool output = true;
        foreach(GameObject go in gm.Players)
        {
            if(!(go.GetComponent<PlayerHandler>().Health <= 0))
                output = output && characterViewable(go);
        }
        return output;
    }

    void zoomOut()
    {
        zoomAmount += 1;
        gameObject.transform.position += transform.forward * -0.25f;
    }

    public void SuperZoomOut()
    {
        StartCoroutine(zoomShort());
    }

    IEnumerator zoomShort()
    {
        yield return new WaitForSeconds(3f);
        shouldZoom = true;
        yield return new WaitForSeconds(.75f);
        shouldZoom = false;
    }

    void zoomIn()
    {
        zoomAmount -= 4f;
        gameObject.transform.position += transform.forward;
    }
}
