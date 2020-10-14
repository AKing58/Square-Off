using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedComet : PlayerHandler
{

    protected Vector3 dodgeTargetLocation;
    new void Start()
    {
        base.Start();
        Debug.Log("RedComet Start");

        speed = 3f;
        SetActiveFrames("Grab", "GrabHitbox", 15, 19);
        SetActiveFrames("Rekka1", "Rekka1Hitbox", 15, 20);
    }

    // Update is called once per frame
    new void Update()
    {
        HandleAbilityInputs();
        if (controllable)
        {
            HandleMovementInputs();
        }
        base.Update();
    }

    protected override void HandleAbilityInputs()
    {
        if (AbilityAInput)
        {
            AbilityA();
        }
        else if (AbilityBInput)
        {
            AbilityB();
        }

        if (AbilityDInput)
        {
            AbilityD();
        }
    }
    protected override void HandleMovementInputs()
    {
        float h = movementInput.x;
        float v = movementInput.y;

        targetVec = new Vector3(h, 0, v);
        targetVec.Normalize();
    }

    override protected void AbilityA() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle", "Rekka1", "Rekka1 0" }))
        {
            anim.SetTrigger("Rekka1Param");
        }
    }
    override protected void AbilityB() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle", "Rekka1 0" }))
        {
            anim.SetTrigger("GrabStartParam");
        }
    }
    override protected void AbilityC() 
    { 
    
    }
    override protected void AbilityD() 
    {
        if (targetVec == new Vector3())
            dodgeTargetLocation = transform.position + transform.forward * dodgeForce;
        else
        {
            dodgeTargetLocation = transform.position + targetVec * dodgeForce;
        }
        rotTowards(dodgeTargetLocation + transform.position);
        GetComponent<Rigidbody>().AddForce(dodgeTargetLocation + transform.up * 2f, ForceMode.VelocityChange);
    }

    virtual protected void Super() { }

}
