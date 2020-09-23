using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxScript : MonoBehaviour
{
    public GameObject parentGO;
    protected void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), transform.GetComponent<Collider>());
    }
}
