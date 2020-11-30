using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFire : MonoBehaviour
{
    public GameObject pillar;
    public GameObject fire1;
    public GameObject fire2;
    public GameObject fire3;
    public GameObject fire4;

    public float rotSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fire1.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
        fire2.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
        fire3.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
        fire4.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
    }
    
}
