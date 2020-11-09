using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCometSuperHandler : MonoBehaviour
{
    public float startingHeight;
    public bool delayComplete = false;
    public GameObject Opponent;
    // Start is called before the first frame update
    void Start()
    {
        startingHeight = transform.position.y;
        StartCoroutine(StartingDelay());
    }

    private IEnumerator StartingDelay()
    {
        yield return new WaitForSeconds(3.25f);
        delayComplete = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (delayComplete && transform.position.y <= startingHeight + 0.1f)
        {
            gameObject.GetComponent<PlayerHandler>().SetAnimParam("LandedParam");
            Opponent.GetComponent<PlayerHandler>().SetAnimParam("LandedParam");
            Opponent.transform.SetParent(null);
            Opponent.GetComponent<PlayerHandler>().Health -= 100;
            Destroy(this);
        }
    }

}
