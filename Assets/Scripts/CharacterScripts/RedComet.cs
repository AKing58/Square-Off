using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedComet : PlayerHandler
{

    protected Vector3 dodgeTargetLocation;
    new void Start()
    {
        base.Start();

        speed = 3f;
        dodgeForce = 50f;

        abilityJSON = Resources.Load<TextAsset>("GameObjects/Characters/RedCometMoveInfo");
        AbilityData = JsonUtility.FromJson<Abilities>(abilityJSON.text);

        PopulateMoveList();
        SetupActiveMoves();
        SetupMoveStarts();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (InValidAnim("Stance"))
        {
            if (targetVec != new Vector3())
            {
                RotTowards(targetVec + transform.position);
            }
        }
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
        animEventStart.functionName = "SuperLaunch";

        animClip.AddEvent(animEventStart);
    }

    override protected void AbilityA()
    {
        float rekkaForce = 20.0f;
        if (InValidAnim(new string[] { "Walk", "Idle", "Rekka1"}))
        {            
            anim.SetTrigger("RekkaParam");
            if (InValidAnim(new string[] { "Walk", "Idle" }))
                rekkaForce = 25.0f;
            //CurrentForce = transform.forward * rekkaForce;
        }
        if (InValidAnim("Stance"))
            anim.SetTrigger("RekkaParam");
    }
    override protected void AbilityB() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle"}))
            anim.SetTrigger("GrabStartParam");
        if (InValidAnim("Rekka2"))
            anim.SetTrigger("StanceParam");
        if (InValidAnim("Stance"))
        {
            anim.SetTrigger("StanceDiveParam");
            //CurrentForce = transform.forward * 50f;
        }

            
    }
    override protected void AbilityC() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle" }) && superAvailable)
        {
            anim.SetTrigger("SuperStartParam");
            ResetSuper();
        }
    }

    override protected void AbilityD() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle", "Stance" }))
        {
            anim.SetTrigger("DodgeParam");
            if (targetVec == new Vector3())
                dodgeTargetLocation = transform.position + transform.forward * dodgeForce;
            else
            {
                dodgeTargetLocation = transform.position + targetVec * dodgeForce;
            }
            
            RotTowards(dodgeTargetLocation);
            //CurrentForce = transform.forward * dodgeForce;
        }
    }

    virtual protected void Super() { }

}
