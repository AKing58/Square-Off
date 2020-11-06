using System;
using System.Collections;
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

    public bool GrabImmune;
    public bool StrikeImmune;

    public Dictionary<string, Move> MoveList = new Dictionary<string, Move>();
    public Move CurrentMove;

    protected TextAsset abilityJSON;
    protected Abilities AbilityData;

    public enum PlayerState { GRABINVULN, STRIKEINVULN, CANCELABLE }

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
            health = Mathf.Clamp(value,0,100);
            LastTimeHit = Time.time;
            PlayerInfoPanel.transform.Find("HPBar").GetComponent<Image>().fillAmount = value / MaxHealth;
            PlayerInfoPanel.transform.Find("HPBar/HPText").GetComponent<Text>().text = value + "/" + MaxHealth;
            PlayerInfoPanel.transform.Find("HPBar").GetComponent<Image>().color = DetermineBarColor(health, maxHealth, false, false);
            if (health <= 0)
            {
                anim.SetBool("DeadParam", true);
                Stun = 0;
                //enableKinematics();
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
            if (stun >= maxStun)
            {
                anim.SetBool("StunnedParam", true);
            }
            transform.Find("WorldSpaceUI/Canvas/StunMeter").GetComponent<Image>().color = DetermineBarColor(stun, maxStun, true, true);
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

        CurrentMove = null;
    }

    // Update is called once per frame
    protected void FixedUpdate()
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
                RotTowards(targetVec + transform.position);
                transform.Translate(targetVec.x * speed * Time.deltaTime, targetVec.y * speed * Time.deltaTime, targetVec.z * speed * Time.deltaTime, Space.World);
            }
        }
        HandleStun();

        if(rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y *(gravMod) * Time.deltaTime;

        if(CurrentMove != null)
            HandleCurrentMove();
    }

    public void HandleCurrentMove()
    {
        CurrentMove.CurFrame++;
        if (CurrentMove.CurFrame == CurrentMove.TotalFrames)
        {
            CurrentMove.CurFrame = 0;
            CurrentMove = null;
        }
    }

    public void HandleStun()
    {
        if (Stun > 0 && Time.time > LastTimeHit + 5f && LastTimeStunned == 0)
            Stun -= 0.25f;

        if (InValidAnim("Stunned") && LastTimeStunned == 0)
            LastTimeStunned = Time.time;

        if (LastTimeStunned != 0 && Time.time > LastTimeStunned + 3f)
            TurnStunOff();
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

    /// <summary>
    /// Returns a color based on the value and max value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxValue"></param>
    /// <param name="reversed">
    /// false: returns value==maxValue as green
    /// true: returns value==maxValue as red
    /// </param>
    /// <param name="partialTransparency"></param>
    /// <returns></returns>
    public Color DetermineBarColor(float value, float maxValue, bool reversed, bool partialTransparency)
    {
        float colval = value - (maxValue / 2);
        if (reversed)
            colval *= -1;
        Color output;
        if (colval >= 0)
            output =  new Color(1 - colval / (maxValue / 2), 1, 0);
        else
            output =  new Color(1, 1 - Mathf.Abs(colval) / (maxValue / 2), 0);
        if (partialTransparency)
            output.a = 0.5f;
        return output;
    }

    public void StageKillMe()
    {
        Health = 0;
        anim.CrossFade("LayOnGround", 1f, -1, 0, 0.5f);
    }

    public void TurnStunOff()
    {
        LastTimeStunned = 0;
        Stun = 0;
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
        animEventStart.functionName = "SetHitbox";

        AnimationEvent animEventEnd = new AnimationEvent();
        animEventEnd.intParameter = 0;
        animEventEnd.time = endTime * (1.0f / Constants.ANIMATION_FRAME_RATE);
        animEventEnd.stringParameter = hitboxName;
        animEventEnd.functionName = "SetHitbox";

        animClip.AddEvent(animEventStart);
        animClip.AddEvent(animEventEnd);
    }

    public void SetHitbox(AnimationEvent ae)
    {
        gameObject.transform.Find("Hitboxes/" + ae.stringParameter).GetComponent<BoxCollider>().enabled = ae.intParameter == 1;
    }

    protected void SetInvulnFrames(string moveName, string type, int startTime, int endTime)
    {
        if (startTime == -1 || endTime == -1)
            return;
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
            Debug.LogError("Error: Could not find animation clip to bind invulnerability frames to");
            return;
        }
        AnimationEvent animEventStart = new AnimationEvent();
        animEventStart.intParameter = 1;
        animEventStart.time = startTime * (1.0f / Constants.ANIMATION_FRAME_RATE);
        animEventStart.stringParameter = type;
        animEventStart.functionName = "SetInvuln";

        AnimationEvent animEventEnd = new AnimationEvent();
        animEventEnd.intParameter = 0;
        animEventEnd.time = endTime * (1.0f / Constants.ANIMATION_FRAME_RATE);
        animEventEnd.stringParameter = type;
        animEventEnd.functionName = "SetInvuln";

        animClip.AddEvent(animEventStart);
        animClip.AddEvent(animEventEnd);
    }

    public void SetInvuln(AnimationEvent ae)
    {
        if (ae.stringParameter == "Grab")
            GrabImmune = ae.intParameter == 1;
        else if (ae.stringParameter == "Strike")
            StrikeImmune = ae.intParameter == 1;
        else
            Debug.Log("No invuln set");
    }

    public void GrabConnected()
    {
        anim.SetTrigger("GrabConnectParam");
        StopHitboxes();
    }

    public bool CanBeGrabbed()
    {
        if (CurrentMove == null || Health <=0)
            return true;
        if(CurrentMove.GrabInvulnFrames[1] == 999)
            if (InValidAnim(CurrentMove.Name))
                return false;
        if (CurrentMove.CurFrame >= CurrentMove.GrabInvulnFrames[0] && CurrentMove.CurFrame <= CurrentMove.GrabInvulnFrames[1])
            return false;
        Debug.Log("too far" + CurrentMove.Name);
        return true;
        //return !GrabImmune;
        //return !(InValidAnim(new string[] { "Grabbed", "GetUp", "GrabConnect", "Dodge", "LayOnGround", "SuperStart", "SuperConnect" }));
    }

    public bool CanBeStriked()
    {
        if (CurrentMove == null || Health <= 0)
            return true;
        if (CurrentMove.StrikeInvulnFrames[1] == 999)
            if (InValidAnim(CurrentMove.Name))
                return false;
        if (CurrentMove.CurFrame >= CurrentMove.StrikeInvulnFrames[0] && CurrentMove.CurFrame <= CurrentMove.StrikeInvulnFrames[1])
            return false;
        return true; ;
        //return !StrikeImmune;
        //return !(InValidAnim(new string[] { "Grabbed", "GetUp", "GrabConnect", "StrikedFront", "Dodge", "LayOnGround", "SuperStart", "SuperConnect", "SuperLanding", "SuperGrabbedFalling", "SuperGrabbedLanding"}));
    }

    public void GrabMe()
    {
        anim.SetTrigger("GrabbedParam");
        Health -= 30;
        Stun += 15;
        StopHitboxes();
    }

    public void StrikeMe()
    {
        anim.SetTrigger("StrikedFrontParam");
        Health -= 20;
        Stun += 10;
        StopHitboxes();
    }

    public void StopHitboxes()
    {
        foreach (Transform t in gameObject.transform.Find("Hitboxes").transform)
        {
            t.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void RotTowards(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }


    //Methods used for abilities
    [Serializable]
    public class Move
    {
        public string Name;
        public string Type;
        public int[] ActiveFrames;
        public int[] GrabInvulnFrames;
        public int[] StrikeInvulnFrames;
        public int TotalFrames;

        public int CurFrame = 0;

        public Move(string Name) : this(Name, null, null, null, null, 0) { }
        public Move(string Name, string Type, int[] ActiveFrames, int[] GrabInvulnFrames, int[] StrikeInvulnFrames, int TotalFrames)
        {
            this.Name = Name;
            if (Type == null)
                this.Type = "NA";
            else
                this.Type = Type;

            if(ActiveFrames == null)
                this.ActiveFrames = new int[] { -1, -1 };
            else
                this.ActiveFrames = ActiveFrames;

            if(GrabInvulnFrames == null)
                this.GrabInvulnFrames = new int[] { -1, -1 };
            else
                this.GrabInvulnFrames = GrabInvulnFrames;

            if(StrikeInvulnFrames == null)
                this.StrikeInvulnFrames = new int[] { -1, -1 };
            else
                this.StrikeInvulnFrames = StrikeInvulnFrames;

            this.TotalFrames = TotalFrames;

        }
    }

    [Serializable]
    public class Abilities
    {
        public List<Move> Moves;
        public List<Move> InvulnStates;
    }

    public void PopulateMoveList()
    {
        foreach(Move m in AbilityData.Moves)
        {
            if (MoveList.ContainsKey(m.Name))
                MoveList[m.Name] = new Move(m.Name, m.Type, m.ActiveFrames, m.GrabInvulnFrames, m.StrikeInvulnFrames, m.TotalFrames);
            else
                MoveList.Add(m.Name, new Move(m.Name, m.Type, m.ActiveFrames, m.GrabInvulnFrames, m.StrikeInvulnFrames, m.TotalFrames));
        }
        foreach(Move invulnState in AbilityData.InvulnStates)
        {
            if (MoveList.ContainsKey(invulnState.Name))
                MoveList[invulnState.Name].Name = invulnState.Name;
            else
                MoveList.Add(invulnState.Name, new Move(invulnState.Name));
            switch (invulnState.Type)
            {
                case "Grab":
                    MoveList[invulnState.Name].GrabInvulnFrames = new int[] { 0, 999 };
                    break;
                case "Strike":
                    MoveList[invulnState.Name].StrikeInvulnFrames = new int[] { 0, 999 };
                    break;
                case "Both":
                    MoveList[invulnState.Name].GrabInvulnFrames = new int[] { 0, 999 };
                    MoveList[invulnState.Name].StrikeInvulnFrames = new int[] { 0, 999 };
                    break;
                default:
                    break;
            }
        }
    }

    protected void SetupActiveMoves()
    {
        foreach(Move m in AbilityData.Moves)
        {
            Debug.Log("Setup move named: " + m.Name +":"+ MoveList[m.Name].ActiveFrames[0] + ", " + MoveList[m.Name].ActiveFrames[1]);
            SetActiveFrames(m.Name, m.Name + "Hitbox", MoveList[m.Name].ActiveFrames[0], MoveList[m.Name].ActiveFrames[1]);
            //SetInvulnFrames(m.Name, "Grab", MoveList[m.Name].GrabInvulnFrames[0], MoveList[m.Name].GrabInvulnFrames[1]);
            //SetInvulnFrames(m.Name, "Strike", MoveList[m.Name].StrikeInvulnFrames[0], MoveList[m.Name].StrikeInvulnFrames[1]);
        }
    }

    protected void SetupInvulnStates()
    {
        foreach (Move m in AbilityData.InvulnStates)
        {
            if(m.Type == "Grab")
                SetInvulnFrames(m.Name, m.Type, MoveList[m.Name].GrabInvulnFrames[0], MoveList[m.Name].GrabInvulnFrames[1]);
            if(m.Type == "Strike")
                SetInvulnFrames(m.Name, m.Type, MoveList[m.Name].StrikeInvulnFrames[0], MoveList[m.Name].StrikeInvulnFrames[1]);
            if(m.Type == "Both")
                SetInvulnFrames(m.Name, m.Type, MoveList[m.Name].StrikeInvulnFrames[0], MoveList[m.Name].StrikeInvulnFrames[1]);
        }
    }


    public void PrintAllAbilities()
    {
        string[] movenamelist = { "Grabbed", "GetUp", "GrabConnect", "StrikedFront", "Dodge", "LayOnGround", "SuperStart", "SuperConnect", "SuperLanding", "SuperGrabbedFalling", "SuperGrabbedLanding" }; ;
        foreach(string move in movenamelist)
        {
            Move tempMove = MoveList[move];
            Debug.Log(tempMove.Name + ", " + tempMove.Type + ", GrabInvulnFrames: (" + tempMove.GrabInvulnFrames[0] + ", " + tempMove.GrabInvulnFrames[1] + "), StrikeInvulnFrames: (" + tempMove.StrikeInvulnFrames[0] + ", " + tempMove.StrikeInvulnFrames[1] + ")");
        }
    }


    //Methods used for inputs

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
