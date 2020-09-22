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
        AnimationClip animClip = null;
        anim = gameObject.GetComponent<Animator>();

        AnimationEvent animEventGrabbing = new AnimationEvent();
        animEventGrabbing.intParameter = 1;
        animEventGrabbing.time = 0.0f;
        animEventGrabbing.functionName = "setGrab";

        AnimationEvent animEventGrabEnd = new AnimationEvent();
        animEventGrabEnd.intParameter = 0;
        animEventGrabEnd.time = 0.165f;
        animEventGrabEnd.functionName = "setGrab";

        foreach (AnimationClip tempClip in anim.runtimeAnimatorController.animationClips)
        {
            if (tempClip.name == "Armature|GrabActive")
            {
                animClip = tempClip;
            }
        }

        if (animClip != null)
        {
            animClip.AddEvent(animEventGrabbing);
            animClip.AddEvent(animEventGrabEnd);
        }
        


    }

    // Update is called once per frame
    void Update()
    {
        if (controllable)
        {
            if (Input.GetKeyDown("z"))
            {
                if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Idle")))
                anim.SetTrigger("GrabStartParam");
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Idle"))
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

    public void setGrab(int value)
    {
        grabHitbox.GetComponent<BoxCollider>().enabled = value == 1;
    }

    public void grabConnected()
    {
        anim.SetTrigger("GrabConnectParam");
    }

    public bool canBeGrabbed()
    {
        return !(anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Grabbed")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|GetUp")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|GrabConnect"));
    }

    public void grabMe()
    {
        anim.SetTrigger("GrabbedParam");
    }

    public void rotTowards(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
