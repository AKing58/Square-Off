using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public GameObject westObs;
    public GameObject eastObs;
    public GameObject southObs;
    public GameObject northObs;

    public GameObject button;

    private bool buttonOn = false;
    private bool active = false;

    public float moveSpeed = 0.01f;

    private Vector3 startPosition1;
    private Vector3 startPosition2;
    private Vector3 startPosition3;
    private Vector3 startPosition4;

    private Vector3 endPosition1;
    private Vector3 endPosition2;
    private Vector3 endPosition3;
    private Vector3 endPosition4;


    // Start is called before the first frame update
    void Start()
    {
        startPosition1 = GameObject.Find("StartLoc1").transform.position;
        startPosition2 = GameObject.Find("StartLoc2").transform.position;
        startPosition3 = GameObject.Find("StartLoc3").transform.position;
        startPosition4 = GameObject.Find("StartLoc4").transform.position;

        endPosition1 = GameObject.Find("EndLoc1").transform.position;
        endPosition2 = GameObject.Find("EndLoc2").transform.position;
        endPosition3 = GameObject.Find("EndLoc3").transform.position;
        endPosition4 = GameObject.Find("EndLoc4").transform.position;

        button.GetComponent<Renderer>().material.color = Color.red;
    }

    void Update()
    {
        if (active && buttonOn)
        {

            westObs.transform.position = Vector3.MoveTowards(westObs.transform.position, endPosition1, moveSpeed * Time.deltaTime);
            eastObs.transform.position = Vector3.MoveTowards(eastObs.transform.position, endPosition2, moveSpeed * Time.deltaTime);
            southObs.transform.position = Vector3.MoveTowards(southObs.transform.position, endPosition3, moveSpeed * Time.deltaTime);
            northObs.transform.position = Vector3.MoveTowards(northObs.transform.position, endPosition4, moveSpeed * Time.deltaTime);
            button.GetComponent<Renderer>().material.color = Color.green;
        }

        if (active && !buttonOn)
        {
            westObs.transform.position = Vector3.MoveTowards(westObs.transform.position, startPosition1, moveSpeed * Time.deltaTime);
            eastObs.transform.position = Vector3.MoveTowards(eastObs.transform.position, startPosition2, moveSpeed * Time.deltaTime);
            southObs.transform.position = Vector3.MoveTowards(southObs.transform.position, startPosition3, moveSpeed * Time.deltaTime);
            northObs.transform.position = Vector3.MoveTowards(northObs.transform.position, startPosition4, moveSpeed * Time.deltaTime);
            button.GetComponent<Renderer>().material.color = Color.red;
        }

    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (active) return;
            active = true;
            buttonOn = !buttonOn;

            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        active = false;
    }
}
