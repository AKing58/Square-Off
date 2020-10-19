﻿using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    protected Animator anim;
    protected float rotSpeed = 1000.0f;
    public float speed = 1.5f;
    public float gravMod = 2.5f;
    protected float dodgeForce = 20f;
    public bool controllable = false;

    //Used for movement
    protected Vector3 targetVec;

    protected Rigidbody rb;

    public GameObject PlayerInfoPanel;
    public string CharacterName;
    public string PlayerName;

    public float LastTimeHit;
    public float LastTimeStunned;

    protected Vector2 movementInput;

    private bool abilityAInput;
    private bool abilityBInput;
    private bool abilityCInput;
    private bool abilityDInput;

    protected bool AbilityAInput
    {
        get 
        {
            if (abilityAInput)
            {
                abilityAInput = false;
                return true;
            }
            return abilityAInput;
        }
    }
    protected bool AbilityBInput
    {
        get
        {
            if (abilityBInput)
            {
                abilityBInput = false;
                return true;
            }
            return abilityBInput;
        }
    }
    protected bool AbilityCInput
    {
        get
        {
            if (abilityCInput)
            {
                abilityCInput = false;
                return true;
            }
            return abilityCInput;
        }
    }
    protected bool AbilityDInput
    {
        get
        {
            if (abilityDInput)
            {
                abilityDInput = false;
                return true;
            }
            return abilityDInput;
        }
    }

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
            if(health <= 0)
            {
                anim.SetBool("DeadParam", true);
                Stun = 0;
                enableKinematics();
            }
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
                transform.Find("WorldSpaceUI/Canvas/StunMeter").GetComponent<Image>().color = new Color(150, 0, 0, (100f / 255f));
                anim.SetBool("StunnedParam", true);
            }
        }
    }

    protected void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
    protected void Update()
    {
        if (InValidAnim(new string[] { "Walk", "Idle" }))
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
        //Stun Handling
        if (Stun > 0 && Time.time > LastTimeHit + 5f && LastTimeStunned == 0)
        {
            Stun -= 0.05f;
        }

        if(InValidAnim("Stunned") && LastTimeStunned == 0)
        {
            LastTimeStunned = Time.time;
        }

        if(LastTimeStunned != 0 && Time.time > LastTimeStunned + 3f)
        {
            TurnStunOff();
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y *(gravMod) * Time.deltaTime;
        }
    }

    public void Launch(Vector3 dir, float force)
    {
        GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.VelocityChange);
        //transform.position = transform.positionVector3.up * 5f;
    }

    public void SetAnimParam(string input)
    {
        print(name + " has set " + input + "anim");
        anim.SetTrigger(input);
    }

    private void enableKinematics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().detectCollisions = false;
    }

    private void disableKinematics()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().detectCollisions = true;
    }

    virtual protected void HandleAbilityInputs() { }

    virtual protected void HandleMovementInputs() { }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawCube(dodgeTargetLocation, new Vector3(0.1f, 0.1f, 0.1f));
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(dodgeTargetLocation, new Vector3(0.1f, 0.1f, 0.1f));
    //}

    public void TurnStunOff()
    {
        LastTimeStunned = 0;
        Stun = 0;
        transform.Find("WorldSpaceUI/Canvas/StunMeter").GetComponent<Image>().color = new Color(0, 150, 0, (100f/255f));
        anim.SetBool("StunnedParam", false);
    }

    //Single Argument Overload
    public bool InValidAnim(string arg)
    {
        return InValidAnim(new string[] { arg });
    }
    //Multi Argument
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
        gameObject.transform.Find("Hitboxes/" + ae.stringParameter).GetComponent<BoxCollider>().enabled = ae.intParameter == 1;
    }

    

    public void grabConnected()
    {
        anim.SetTrigger("GrabConnectParam");
        stopHitboxes();
    }

    public bool canBeGrabbed()
    {
        return !(InValidAnim(new string[] { "Grabbed", "GetUp", "GrabConnect", "Dodge", "SuperStart", "SuperConnect" }) &&
            Health <= 0);
    }

    public bool canBeStriked()
    {
        return !(InValidAnim(new string[] { "Grabbed", "GetUp", "GrabConnect", "StrikedFront", "Dodge", "SuperStart", "SuperConnect", "SuperLanding", "SuperGrabbedFalling", "SuperGrabbedLanding"}) &&
            Health <= 0);
    }

    public void grabMe()
    {
        anim.SetTrigger("GrabbedParam");
        Health -= 10;
        Stun += 50;
        stopHitboxes();
    }

    public void strikeMe()
    {
        anim.SetTrigger("StrikedFrontParam");
        Health -= 5;
        Stun += 50;
        stopHitboxes();
    }

    public void stopHitboxes()
    {
        foreach (Transform t in gameObject.transform.Find("Hitboxes").transform)
        {
            t.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void rotTowards(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    virtual protected void AbilityA() { }
    virtual protected void AbilityB() { }
    virtual protected void AbilityC() { }
    virtual protected void AbilityD() { }
    virtual protected void Super() { }

    public void ActivateInputA() { abilityAInput = true; }
    public void ActivateInputB() { abilityBInput = true; }
    public void ActivateInputC() { abilityCInput = true; }
    public void ActivateInputD() { abilityDInput = true; }

    public void setTargetVec(Vector3 target)
    {
        targetVec = target;
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnA(InputAction.CallbackContext ctx) => abilityAInput = ctx.ReadValueAsButton();

    public void OnB(InputAction.CallbackContext ctx) => abilityBInput = ctx.ReadValueAsButton();

    public void OnC(InputAction.CallbackContext ctx) => abilityCInput = ctx.ReadValueAsButton();

    public void OnD(InputAction.CallbackContext ctx) => abilityDInput = ctx.ReadValueAsButton();

}
