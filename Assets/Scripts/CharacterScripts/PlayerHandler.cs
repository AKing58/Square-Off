using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    protected Animator anim;
    protected float rotSpeed = 1000.0f;
    public float speed = 1.5f;
    public float gravMod = 2.5f;
    protected float dodgeForce;
    public bool controllable = false;

    private PlayerConfiguration playerConfig;
    private PlayerControls controls;

    //Used for movement
    public Vector3 targetVec;

    protected Rigidbody rb;

    public GameObject PlayerInfoPanel;
    public string CharacterName;
    public string PlayerName;

    public GameObject[] bodyPieces;

    public float LastTimeHit;
    public float LastTimeStunned;

    public bool GrabImmune;
    public bool StrikeImmune;


    public GameObject GrabInvulnIndicator;
    public GameObject StrikeInvulnIndicator;

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

    protected bool superAvailable = false;
    private Color colorStart;
    private Color colorEnd;
    private float lerpControl = 0;
    private float colorRate = 2;

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
            Energy += (health - value) * 1.5f;
            health = Mathf.Clamp(value, 0, maxHealth);
            LastTimeHit = Time.time;
            PlayerInfoPanel.transform.Find("HPBar").GetComponent<Image>().fillAmount = value / MaxHealth;
            PlayerInfoPanel.transform.Find("HPBar/HPText").GetComponent<Text>().text = value + "/" + MaxHealth;
            PlayerInfoPanel.transform.Find("HPBar").GetComponent<Image>().color = DetermineBarColor(health, maxHealth, false, false);
            
            if (health <= 0)
            {
                anim.SetBool("DeadParam", true);
                transform.Find("WorldSpaceUI/Canvas").gameObject.SetActive(false);
                Stun = 0;
                //enableKinematics();
            }
            if (LastTimeStunned != 0)
            {
                TurnStunOff();
            }
        }
    }

    [SerializeField]
    private float maxEnergy;

    public float MaxEnergy
    {
        get { return maxEnergy; }
    }

    [SerializeField]
    private float energy;

    public float Energy
    {
        get { return energy; }
        set
        {
            energy = Mathf.Clamp(value, 0, maxEnergy);
            superAvailable = (energy == maxEnergy);
            PlayerInfoPanel.transform.Find("SuperBar").GetComponent<Image>().fillAmount = energy / maxEnergy;
            
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

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name == controls.Gameplay.Move.name)
        {
            OnMove(obj);    
        }
        if (obj.action.name == controls.Gameplay.A.name) {
            OnA(obj);
        }
        if (obj.action.name == controls.Gameplay.B.name)
        {
            OnB(obj);
        }
        if (obj.action.name == controls.Gameplay.C.name)
        {
            OnC(obj);
        }
        if (obj.action.name == controls.Gameplay.D.name)
        {
            OnD(obj);
        }
    }

    protected void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        colorStart = Color.white;
        colorEnd = GetRandomLightColor();
    }

    public void InitPlayer(PlayerConfiguration pc)
    {
        PlayerInfoPanel.transform.Find("PlayerName").GetComponent<Text>().text = PlayerName;
        PlayerInfoPanel.transform.Find("CharacterName").GetComponent<Text>().text = CharacterName;

        maxHealth = 100;
        Health = MaxHealth;
        maxStun = 100;
        maxEnergy = 100;
        Energy = 0;
        Stun = 0;
        LastTimeStunned = 0;

        playerConfig = pc;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;

        Material myMaterial;
        if (playerConfig.PlayerIndex == 0)
        {
             myMaterial = Resources.Load<Material>("Materials/Red");
            transform.Find("WorldSpaceUI/Canvas/DirIndicator").GetComponent<Image>().color = new Color(1,0,0,0.6f);
        }
        else if (playerConfig.PlayerIndex == 1)
        {
             myMaterial = Resources.Load<Material>("Materials/Blue");
             transform.Find("WorldSpaceUI/Canvas/DirIndicator").GetComponent<Image>().color = new Color(0, 0, 1, 0.6f);
        }
        else if (playerConfig.PlayerIndex == 2)
        {
             myMaterial = Resources.Load<Material>("Materials/Green");
             transform.Find("WorldSpaceUI/Canvas/DirIndicator").GetComponent<Image>().color = new Color(0, 1, 0, 0.6f);
        }
        else {
             myMaterial = Resources.Load<Material>("Materials/Yellow");
             transform.Find("WorldSpaceUI/Canvas/DirIndicator").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0, 0.6f);
        }

        for (int i = 0; i < bodyPieces.Length; i++)
        {
            bodyPieces[i].GetComponent<SkinnedMeshRenderer>().material = myMaterial;
        }
        CurrentMove = null;
    }

    void Update()
    {
        if (anim.IsInTransition(0))
        {
            ResetCurrentMove();
            //CurrentMove = MoveList[anim.GetNextAnimatorStateInfo(0)];
        }
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

        if (superAvailable) {
            DetermineSuperBarColor();         
        }

        if(rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y *(gravMod) * Time.deltaTime;

        if(CurrentMove != null && CurrentMove.Name != "")
            HandleCurrentMove();
    }

    private void DetermineSuperBarColor() {
        lerpControl += Time.deltaTime * colorRate;

        PlayerInfoPanel.transform.Find("SuperBar").GetComponent<Image>().color = Color.Lerp(colorStart, colorEnd, lerpControl);

        if (lerpControl >= 1)
        {
            lerpControl = 0;
            colorStart = PlayerInfoPanel.transform.Find("SuperBar").GetComponent<Image>().color;
            if (colorEnd == Color.white)
            {
                colorEnd = GetRandomLightColor();
            }
            else
            {
                colorEnd = Color.white;
            }
        }
    }

    private Color GetRandomLightColor()
    {

        return new Color(UnityEngine.Random.Range(0.4f, 1.0f), UnityEngine.Random.Range(0.4f, 1.0f), UnityEngine.Random.Range(0.4f, 1.0f));
    }

    public void HandleCurrentMove()
    {
        CurrentMove.CurFrame += 1f * anim.GetCurrentAnimatorStateInfo(0).speed;

        if(GrabInvulnIndicator.active && StrikeInvulnIndicator.active)
        {
            if (CurrentMove.CurFrame >= CurrentMove.GrabInvulnFrames[0] && CurrentMove.CurFrame <= CurrentMove.GrabInvulnFrames[1])
                GrabInvulnIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
            else
                GrabInvulnIndicator.GetComponent<MeshRenderer>().material.color = Color.green;

            if (CurrentMove.CurFrame >= CurrentMove.StrikeInvulnFrames[0] && CurrentMove.CurFrame <= CurrentMove.StrikeInvulnFrames[1])
                StrikeInvulnIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
            else
                StrikeInvulnIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    public void ResetCurrentMove()
    {
        if(CurrentMove != null)
            CurrentMove.CurFrame = 0;
        GrabInvulnIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        StrikeInvulnIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        CurrentMove = null;
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
    }

    public void SetAnimParam(string input)
    {
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
        animEventStart.time = startTime / 60f;
        animEventStart.stringParameter = hitboxName;
        animEventStart.functionName = "SetHitbox";

        AnimationEvent animEventEnd = new AnimationEvent();
        animEventEnd.intParameter = 0;
        animEventEnd.time = endTime / 60f;
        animEventEnd.stringParameter = hitboxName;
        animEventEnd.functionName = "SetHitbox";

        animClip.AddEvent(animEventStart);
        animClip.AddEvent(animEventEnd);
    }

    protected void SetMoveFirstFrame(string moveName)
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
            Debug.LogError("Error: Could not find animation clip to bind active frames to, " + moveName);
            return;
        }
        AnimationEvent animEventStart = new AnimationEvent();
        animEventStart.time = 0f;
        animEventStart.stringParameter = moveName;
        animEventStart.functionName = "SetMove";

        animClip.AddEvent(animEventStart);
    }

    public void SetMove(string moveName)
    {
        CurrentMove = MoveList[moveName];
        CurrentMove.CurFrame = 0;
    }

    public void SetHitbox(AnimationEvent ae)
    {
        gameObject.transform.Find("Hitboxes/" + ae.stringParameter).GetComponent<BoxCollider>().enabled = ae.intParameter == 1;
    }

    public void GrabConnected()
    {
        anim.SetTrigger("GrabConnectParam");
        StopHitboxes();
    }

    public bool CanBeGrabbed()
    {
        if (Health <= 0 || !controllable)
            return false;
        if ((CurrentMove == null || CurrentMove.Name == "") && Health >=0)
            return true;
        if(CurrentMove.GrabInvulnFrames[1] == 999)
            if (InValidAnim(CurrentMove.Name))
                return false;
        if (CurrentMove.CurFrame >= CurrentMove.GrabInvulnFrames[0] && CurrentMove.CurFrame <= CurrentMove.GrabInvulnFrames[1])
            return false;
        return true;
    }

    public bool CanBeStriked()
    {
        if (Health <= 0 || !controllable)
            return false;
        if ((CurrentMove == null || CurrentMove.Name == ""))
            return true;
        if (CurrentMove.StrikeInvulnFrames[1] == 999)
            if (InValidAnim(CurrentMove.Name))
                return false;
        if (CurrentMove.CurFrame >= CurrentMove.StrikeInvulnFrames[0] && CurrentMove.CurFrame <= CurrentMove.StrikeInvulnFrames[1])
            return false;

        return true;
    }

    public bool AttackOther(PlayerHandler target, string moveName)
    {
        if (MoveList[moveName].Type == "Grab" && target.CanBeGrabbed())
        {
            target.gameObject.transform.position = gameObject.transform.position + transform.forward;
            anim.SetTrigger("GrabConnectParam");
            target.GrabMe(this);
        }
        else if (MoveList[moveName].Type == "SuperGrab" && target.CanBeGrabbed())
        {
            target.gameObject.transform.position = gameObject.transform.position + transform.forward;
            anim.SetTrigger("SuperConnectParam");
            target.SuperMe(this);
        }
        else if (MoveList[moveName].Type == "Strike" && target.CanBeStriked())
            target.StrikeMe();
        else
            return false;
        target.Health -= MoveList[moveName].Damage;
        target.Stun += MoveList[moveName].Stun;
        return true;
    }

    public void GrabMe(PlayerHandler grabber)
    {
        Vector3 grabLoc = Vector3.MoveTowards(transform.position, grabber.transform.position, 100.0f);
        //transform.position = new Vector3(grabLoc.x, transform.position.y, grabLoc.z);
        RotTowards(grabber.transform.position);
        anim.SetTrigger(Constants.CharacterInitials[grabber.CharacterName] + "GrabbedParam");
    }

    public void SuperMe(PlayerHandler grabber)
    {
        Vector3 grabLoc = Vector3.MoveTowards(transform.position, grabber.transform.position, 100.0f);
        //transform.position = new Vector3(grabLoc.x, transform.position.y, grabLoc.z);
        RotTowards(grabber.transform.position);
        anim.SetTrigger(Constants.CharacterInitials[grabber.CharacterName] + "SuperedParam");
        GameObject.Find("GameManager").GetComponent<GameManager>().TempDisableControls();
    }

    public void GrabMe()
    {
        anim.SetTrigger("GrabbedParam");
        StopHitboxes();
    }

    public void StrikeMe()
    {
        anim.SetTrigger("StrikedFrontParam");
        anim.ResetTrigger("GrabConnected");
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
        public float Damage;
        public float Stun;
        public int[] ActiveFrames;
        public int[] GrabInvulnFrames;
        public int[] StrikeInvulnFrames;
        public string HitboxName;
        public int TotalFrames;

        
        public float CurFrame;


        public Move() : this("", null, 0f, 0f, null, null, null, "", 0) { }
        public Move(string Name) : this(Name, null, 0f, 0f, null, null, null , "", 0) { }
        public Move(string Name, string Type, float Damage, float Stun, int[] ActiveFrames, int[] GrabInvulnFrames, int[] StrikeInvulnFrames, string HitboxName, int TotalFrames)
        {
            this.Name = Name;
            if (Type == null)
                this.Type = "NA";
            else
                this.Type = Type;

            this.Damage = Damage;
            this.Stun = Stun;

            if(ActiveFrames == null)
                this.ActiveFrames = new int[] { -1, -1 };
            else
                this.ActiveFrames = new int[] { ConvertFramesToSixty(ActiveFrames[0]), ConvertFramesToSixty(ActiveFrames[1])};

            if(GrabInvulnFrames == null)
                this.GrabInvulnFrames = new int[] { -1, -1 };
            else
                this.GrabInvulnFrames = new int[] { ConvertFramesToSixty(GrabInvulnFrames[0]), ConvertFramesToSixty(GrabInvulnFrames[1]) };

            if (StrikeInvulnFrames == null)
                this.StrikeInvulnFrames = new int[] { -1, -1 };
            else
                this.StrikeInvulnFrames = new int[] { ConvertFramesToSixty(StrikeInvulnFrames[0]), ConvertFramesToSixty(StrikeInvulnFrames[1]) };

            this.HitboxName = HitboxName;

            this.TotalFrames = ConvertFramesToSixty(TotalFrames);
        }

        //Converts 24 fps frame data to 60 fps
        public int ConvertFramesToSixty(int input)
        {
            float output = input;
            return (int) (output * (60f / 24f));
        }
        public int ConvertFramesFromSixty(int input)
        {
            float output = input;
            return (int) output * (24/60);
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
                MoveList[m.Name] = new Move(m.Name, m.Type, m.Damage, m.Stun, m.ActiveFrames, m.GrabInvulnFrames, m.StrikeInvulnFrames, m.HitboxName, m.TotalFrames);
            else
                MoveList.Add(m.Name, new Move(m.Name, m.Type, m.Damage, m.Stun, m.ActiveFrames, m.GrabInvulnFrames, m.StrikeInvulnFrames, m.HitboxName, m.TotalFrames));
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
            //Debug.Log("Setup move named: " + m.Name + ", " + m.HitboxName + " :"+ MoveList[m.Name].ActiveFrames[0] + ", " + MoveList[m.Name].ActiveFrames[1]);
            SetActiveFrames(m.Name, m.HitboxName, MoveList[m.Name].ActiveFrames[0], MoveList[m.Name].ActiveFrames[1]);
        }
    }

    protected void SetupMoveStarts()
    {
        foreach (Move m in AbilityData.Moves)
        {
            SetMoveFirstFrame(m.Name);
        }
        foreach (Move m in AbilityData.InvulnStates)
        {
            SetMoveFirstFrame(m.Name);
        }
    }

    protected void ResetSuper() {
        Energy = 0;
        superAvailable = false;
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
