﻿using System.Collections;
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
        setupSuperEvent();
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

    private void setupSuperEvent()
    {
        AnimationClip animClip = null;
        foreach (AnimationClip tempClip in anim.runtimeAnimatorController.animationClips)
        {
            if (tempClip.name == "SuperConnect")
            {
                animClip = tempClip;
            }
        }
        if (animClip == null)
        {
            Debug.LogError("Error: Could not find animation clip to bind active frames to");
            return;
        }
        AnimationEvent animEventStart = new AnimationEvent();
        animEventStart.intParameter = 1;
        animEventStart.time = 68 * (1.0f / Constants.ANIMATION_FRAME_RATE);
        animEventStart.functionName = "SuperLaunchForce";

        animClip.AddEvent(animEventStart);
    }

    protected override void HandleAbilityInputs()
    {
        if (AbilityAInput)
            AbilityA();
        else if (AbilityBInput)
            AbilityB();
        else if (AbilityCInput)
            AbilityC();
        else if (AbilityDInput)
            AbilityD();
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
        if (InValidAnim(new string[] { "Walk", "Idle" }))
        {
            print(gameObject.transform.Find("Hitboxes/GrabHitbox").name);
            gameObject.transform.Find("Hitboxes/GrabHitbox").GetComponent<GrabHandler>().IsSuper = true;
            anim.SetTrigger("SuperStartParam");
        }
    }

    override protected void AbilityD() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle"}))
        {
            anim.SetTrigger("DodgeParam");
            if (targetVec == new Vector3())
                dodgeTargetLocation = transform.position + transform.forward * dodgeForce;
            else
            {
                dodgeTargetLocation = transform.position + targetVec * dodgeForce;
            }
            rotTowards(dodgeTargetLocation + transform.position);
            GetComponent<Rigidbody>().AddForce(dodgeTargetLocation + transform.up * 2f, ForceMode.VelocityChange);
        }
    }

    virtual protected void Super() { }

}
