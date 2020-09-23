using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private Animator anim;
    private float rotSpeed = 100.0f;
    public float speed = 1.5f;
    public bool controllable = false;

    [SerializeField]
    public GameObject grabHitbox;

    void Start()
    {
        speed = 1.5f;
        anim = gameObject.GetComponent<Animator>();

        SetActiveFrames("Grab", "GrabHitbox", 15, 19);
        SetActiveFrames("Rekka1", "Rekka1Hitbox", 15, 20);

        //AnimationEvent animEventGrabbing = new AnimationEvent();
        //animEventGrabbing.intParameter = 1;
        //animEventGrabbing.time = 0.0f;
        //animEventGrabbing.functionName = "setGrab";

        //AnimationEvent animEventGrabEnd = new AnimationEvent();
        //animEventGrabEnd.intParameter = 0;
        //animEventGrabEnd.time = 0.165f;
        //animEventGrabEnd.functionName = "setGrab";

        //foreach (AnimationClip tempClip in anim.runtimeAnimatorController.animationClips)
        //{
        //    if (tempClip.name == "Armature|GrabActive")
        //    {
        //        animClip = tempClip;
        //    }
        //}

        //if (animClip != null)
        //{
        //    animClip.AddEvent(animEventGrabbing);
        //    animClip.AddEvent(animEventGrabEnd);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (controllable)
        {
            if (Input.GetKeyDown("z"))
            {
                if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rekka1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rekka1 0")))
                    anim.SetTrigger("Rekka1Param");
            }else if (Input.GetKeyDown("x"))
            {
                if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rekka1 0")))
                    anim.SetTrigger("GrabStartParam");
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Vector3 targetVec = new Vector3();
                if (Input.GetKey("up"))
                {
                    anim.SetFloat("WalkParam", speed);
                    targetVec += new Vector3(0, 0, 1);
                }
                if (Input.GetKey("down"))
                {
                    anim.SetFloat("WalkParam", speed);
                    targetVec += new Vector3(0, 0, -1);
                }
                if (Input.GetKey("left"))
                {
                    anim.SetFloat("WalkParam", speed);
                    targetVec += new Vector3(-1, 0, 0);
                }
                if (Input.GetKey("right"))
                {
                    anim.SetFloat("WalkParam", speed);
                    targetVec += new Vector3(1, 0, 0);
                }
                if (targetVec == new Vector3())
                {
                    anim.SetFloat("WalkParam", 0.0f);
                }
                if (targetVec != new Vector3())
                {
                    targetVec = targetVec.normalized;
                    rotTowards(targetVec + transform.position);
                    transform.Translate(targetVec.x * speed * Time.deltaTime, targetVec.y * speed * Time.deltaTime, targetVec.z * speed * Time.deltaTime, Space.World);
                }
            }
        }
    }

    protected void SetActiveFrames(string moveName, string hitboxName, int startTime, int endTime)
    {
        AnimationClip animClip = null;
        foreach (AnimationClip tempClip in anim.runtimeAnimatorController.animationClips)
        {
            if (tempClip.name == moveName)
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
        animEventStart.time = startTime * (1.0f / Constants.ANIMATION_FRAME_RATE);
        animEventStart.stringParameter = hitboxName;
        animEventStart.functionName = "setHitbox";

        AnimationEvent animEventEnd = new AnimationEvent();
        animEventEnd.intParameter = 0;
        animEventEnd.time = endTime * (1.0f / Constants.ANIMATION_FRAME_RATE);
        animEventEnd.stringParameter = hitboxName;
        animEventEnd.functionName = "setHitbox";

        animClip.AddEvent(animEventStart);
        animClip.AddEvent(animEventEnd);
    }

    public void setHitbox(AnimationEvent ae)
    {
        GameObject.Find("Hitboxes/" + ae.stringParameter).GetComponent<BoxCollider>().enabled = ae.intParameter == 1;
    }

    public void grabConnected()
    {
        anim.SetTrigger("GrabConnectParam");
    }

    public bool canBeGrabbed()
    {
        return !(anim.GetCurrentAnimatorStateInfo(0).IsName("Grabbed")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("GetUp")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("GrabConnect"));
    }

    public bool canBeStriked()
    {
        return !(anim.GetCurrentAnimatorStateInfo(0).IsName("Grabbed")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("GetUp")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("GrabConnect"));
    }

    public void grabMe()
    {
        anim.SetTrigger("GrabbedParam");
    }

    public void strikeMe()
    {
        anim.SetTrigger("StrikedFrontParam");
    }

    public void rotTowards(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
