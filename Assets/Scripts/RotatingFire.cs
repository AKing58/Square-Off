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

    private float fireSoundCooldown = 6.0f;
    private float lastTimePlayedFire = 1;

    // Start is called before the first frame update
    void Start()
    {
        lastTimePlayedFire = 1;
    }

    // Update is called once per frame
    /// <summary>
    /// Plays sound effect on cooldown and rotates the fireorbs around the pillar.
    /// </summary>
    void Update()
    {
        if (Time.time - lastTimePlayedFire > fireSoundCooldown) {
            SoundManager.PlayOneShotUI(SoundManager.SFX.Flamethrower, 0.30f);
            lastTimePlayedFire = Time.time;
        }
        fire1.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
        fire2.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
        fire3.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
        fire4.transform.RotateAround(pillar.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
    }
    
}
