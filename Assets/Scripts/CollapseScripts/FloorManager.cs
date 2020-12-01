using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public List<Transform> Tiles = new List<Transform>();

    bool tileDropTimer = false;

    void Start()
    {
        foreach(Transform tr in transform.Find("Cubes").transform)
        {
            if(tr.GetComponent<TileScript>())
                Tiles.Add(tr);
        }
    }

    void FixedUpdate()
    {
        if (tileDropTimer == false)
            StartCoroutine(TileDrop());
    }

    IEnumerator TileDrop()
    {
        tileDropTimer = true;
        yield return new WaitForSeconds(10f);
        DropTile();
        tileDropTimer = false;
    }

    void DropTile()
    {
        int count = 3;
        while(count-- > 0)
        {
            int randTile = Random.Range(0, Tiles.Count);
            Tiles[randTile].gameObject.GetComponent<TileScript>().PrepDrop(5f);
            Tiles.RemoveAt(randTile);
        }
    }
}
