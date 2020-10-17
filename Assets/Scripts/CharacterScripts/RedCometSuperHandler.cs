using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCometSuperHandler : MonoBehaviour
{
    public float startingHeight;
    public bool delayComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        //print("added to " + transform.parent.name);
        startingHeight = transform.position.y;
        StartCoroutine(StartingDelay());
    }

    private IEnumerator StartingDelay()
    {
        yield return new WaitForSeconds(1.5f);
        delayComplete = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (delayComplete && transform.position.y <= startingHeight + 1f)
        {
            gameObject.GetComponent<PlayerHandler>().SetAnimParam("LandedParam");
            if(transform.parent != null && transform.parent.name == "GrabHitBox")
            {
                transform.SetParent(null);
                GetComponent<CapsuleCollider>().enabled = true;
                GetComponent<PlayerHandler>().Health -= 100;
            }
            Destroy(this);
        }
    }
}
