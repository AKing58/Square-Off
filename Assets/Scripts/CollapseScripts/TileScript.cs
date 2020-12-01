using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    int frameCount = 0;
    bool dropPrepped = false;

    float shaderTimer = -1f;

    float dropTime;
    public void PrepDrop(float dropDelay)
    {
        GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/DiscoTile");
        dropTime = dropDelay;
        StartCoroutine(DropMe());
    }

    private void FixedUpdate()
    {
        if (dropPrepped)
        {
            float timeIncrement = dropTime * 10;
            frameCount++;
            if (frameCount % timeIncrement == 0)
            {
                shaderTimer += 0.4f;
                GetComponent<MeshRenderer>().material.SetFloat("Vector1_73A614A1", shaderTimer);
            }

        }
    }

    IEnumerator DropMe()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        dropPrepped = true;
        yield return new WaitForSeconds(dropTime);
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
