using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMoon : PlayerHandler
{

    protected Vector3 dodgeTargetLocation;
    new void Start()
    {
        base.Start();

        speed = 3.5f;
        dodgeForce = 12f;

        abilityJSON = Resources.Load<TextAsset>("GameObjects/Characters/BlueMoonMoveInfo");
        AbilityData = JsonUtility.FromJson<Abilities>(abilityJSON.text);

        PopulateMoveList();
        SetupActiveMoves();
        SetupMoveStarts();

        //setupSuperEvent();
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

    /*
    protected void SuperLaunch()
    {
        float launchForce = 15f;
        print("launching " + transform.Find("Hitboxes/GrabHitbox").GetChild(0).name);
        PlayerHandler opponent = transform.Find("Hitboxes/GrabHitbox").GetChild(0).GetComponent<PlayerHandler>();
        Launch(transform.up, launchForce);
        opponent.Launch(transform.up, launchForce);

    }
    */

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
            anim.SetTrigger("BootParam");
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
        if (InValidAnim(new string[] { "Walk", "Idle", "GetUp" }))
        {
            anim.SetTrigger("DodgeParam");
            if (targetVec == new Vector3())
                dodgeTargetLocation = transform.position + transform.forward * dodgeForce;
            else
            {
                dodgeTargetLocation = transform.position + targetVec * dodgeForce;
            }
            RotTowards(transform.position);
            //CurrentForce = transform.forward*dodgeForce;
        }
    }

    override protected void Super() { }

}
