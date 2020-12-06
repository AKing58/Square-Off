﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    int frameCount = 0;
    bool dropPrepped = false;

    float shaderTimer = -1f;

    float dropTime;

    //Preps dropping this tile by changing the tile shader and starting the timer for dropping
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

    //Drops the tile after a certain amount of time
    IEnumerator DropMe()
    {
        dropPrepped = true;
        yield return new WaitForSeconds(dropTime);
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
