using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    private Animator anim;
    private float rotSpeed = 1000.0f;
    public float speed = 1.5f;
    private float dodgeForce = 20f;
    public bool controllable = false;

    public GameObject PlayerInfoPanel;
    public string CharacterName;
    public string PlayerName;

    public float LastTimeHit;
    public float LastTimeStunned;

    private Vector2 movementInput;
    private bool abilityAInput;
    private bool abilityBInput;
    private bool abilityCInput;
    private bool abilityDInput;
    private Vector3 dodgeTargetLocation;

    [SerializeField]
    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
    }

    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
        set 
        { 
            health = value;
            LastTimeHit = Time.time;
            PlayerInfoPanel.transform.Find("HPBar").GetComponent<Image>().fillAmount = value / MaxHealth;
            PlayerInfoPanel.transform.Find("HPBar/HPText").GetComponent<Text>().text = value + "/" + MaxHealth;
            if(LastTimeStunned != 0)
            {
                TurnStunOff();
            }
        }
    }

    [SerializeField]
    private float maxStun;
    public float MaxStun
    {
        get { return maxStun; }
    }

    [SerializeField]
    private float stun;
    public float Stun
    {
        get { return stun; }
        set
        {
            stun = value;
            transform.Find("WorldSpaceUI/Canvas/StunMeter").GetComponent<Image>().fillAmount = value / MaxStun;
            if(stun >= maxStun)
            {
                print("stunned");
                LastTimeStunned = Time.time;
                anim.SetBool("StunnedParam", true);
            }
        }
    }



    [SerializeField]
    public GameObject grabHitbox;

    void Start()
    {
       
        speed = 3f;
        anim = gameObject.GetComponent<Animator>();

        dodgeTargetLocation = transform.position;

        SetActiveFrames("Grab", "GrabHitbox", 15, 19);
        SetActiveFrames("Rekka1", "Rekka1Hitbox", 15, 20);

    }

    public void InitPlayer()
    {
        PlayerInfoPanel.transform.Find("PlayerName").GetComponent<Text>().text = PlayerName;
        PlayerInfoPanel.transform.Find("CharacterName").GetComponent<Text>().text = CharacterName;

        maxHealth = 100;
        Health = MaxHealth;
        maxStun = 100;
        Stun = 0;

        LastTimeStunned = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (controllable)
        {
            if (abilityAInput)
            {
                //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rekka1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rekka1 0"))
                if (InValidAnim(new string[] { "Walk", "Idle", "Rekka1", "Rekka1 0" }))
                {
                    anim.SetTrigger("Rekka1Param");
                }
                //turn off the bool after processing
                abilityAInput = false;
            }
            else if (abilityBInput)
            {
                //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Rekka1 0"))
                if (InValidAnim(new string[] { "Walk", "Idle", "Rekka1 0" }))
                {
                    anim.SetTrigger("GrabStartParam");
                }
                //turn off the bool after processing
                abilityBInput = false;
            }

            Vector3 targetVec;
            float h = movementInput.x;
            float v = movementInput.y;
            //if (Input.GetKey("up"))
            //{
            //    targetVec += new Vector3(0, 0, 1);
            //}
            //if (Input.GetKey("down"))
            //{
            //    targetVec += new Vector3(0, 0, -1);
            //}
            //if (Input.GetKey("left"))
            //{
            //    targetVec += new Vector3(-1, 0, 0);
            //}
            //if (Input.GetKey("right"))
            //{
            //    targetVec += new Vector3(1, 0, 0);
            //}
            targetVec = new Vector3(h, 0, v);
            targetVec = targetVec.normalized;

            if (abilityDInput)
            {
                if (targetVec == new Vector3())
                    dodgeTargetLocation = transform.position + transform.forward * dodgeForce;
                else
                {
                    dodgeTargetLocation = transform.position + targetVec * dodgeForce;
                }
                Dodge(dodgeTargetLocation);
                //turn off the bool after processing
                abilityDInput = false;
            }

            if (InValidAnim(new string[] { "Walk", "Idle"}))
            {
                if (targetVec == new Vector3())
                {
                    anim.SetFloat("WalkParam", 0.0f);
                }
                else
                {
                    anim.SetFloat("WalkParam", speed);
                }
                if (targetVec != new Vector3())
                {
                    rotTowards(targetVec + transform.position);
                    transform.Translate(targetVec.x * speed * Time.deltaTime, targetVec.y * speed * Time.deltaTime, targetVec.z * speed * Time.deltaTime, Space.World);
                }
            }
        }

        //Stun Handling
        if(Stun > 0 && Time.time > LastTimeHit + 5f && LastTimeStunned == 0)
        {
            Stun -= 0.05f;
        }

        if(LastTimeStunned != 0 && Time.time > LastTimeStunned + 3f)
        {
            TurnStunOff();
        }
    }

    public void Dodge(Vector3 dir)
    {
        rotTowards(dir + transform.position);
        GetComponent<Rigidbody>().AddForce(dir + transform.up*2f, ForceMode.VelocityChange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(dodgeTargetLocation, new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(dodgeTargetLocation, new Vector3(0.1f, 0.1f, 0.1f));
    }

    public void TurnStunOff()
    {
        LastTimeStunned = 0;
        Stun = 0;
        anim.SetBool("StunnedParam", false);
    }

    public bool InValidAnim(string[] args)
    {
        bool output = false;
        foreach(string s in args)
        {
            output = output || anim.GetCurrentAnimatorStateInfo(0).IsName(s);
        }
        return output;
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
            || anim.GetCurrentAnimatorStateInfo(0).IsName("GrabConnect")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("StrikedFront"));
    }

    public void grabMe()
    {
        anim.SetTrigger("GrabbedParam");
        Health -= 10;
        Stun += 20;
    }

    public void strikeMe()
    {
        anim.SetTrigger("StrikedFrontParam");
        Health -= 5;
        Stun += 10;
    }

    public void rotTowards(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnA(InputAction.CallbackContext ctx) => abilityAInput = ctx.ReadValueAsButton();

    public void OnB(InputAction.CallbackContext ctx) => abilityBInput = ctx.ReadValueAsButton();

    public void OnC(InputAction.CallbackContext ctx) => abilityCInput = ctx.ReadValueAsButton();

    public void OnD(InputAction.CallbackContext ctx) => abilityDInput = ctx.ReadValueAsButton();

}
