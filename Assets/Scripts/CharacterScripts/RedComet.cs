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

    /// <summary>
    /// Light attack
    /// Rekka style attack, allows you to do two in a row.
    /// Can cancel into Ability B after doing the second punch.
    /// </summary>
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

    /// <summary>
    /// Stance move.
    /// Performs a long range lunging grab when Ability B is performed in this stance.
    /// Can Ability A or Ability D out of this stance.
    /// </summary>
    override protected void AbilityB() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle", "Rekka2" }))
            anim.SetTrigger("StanceParam");
        if (InValidAnim("Rekka2"))
            anim.SetTrigger("StanceParam");
        if (InValidAnim("Stance"))
        {
            anim.SetTrigger("StanceDiveParam");
            //CurrentForce = transform.forward * 50f;
        }
    }

    /// <summary>
    /// Grab move.
    /// Performs the super move instead when sufficient energy is held.
    /// </summary>
    override protected void AbilityC() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle" }))
        {
            if (superAvailable)
            {
                StartCoroutine(PauseTime(0.005f));
                transform.Find("ParticleSystem").GetComponent<ParticleSystem>().Play();
                anim.SetTrigger("SuperStartParam");
                ResetSuper();
            }
            else
            {
                anim.SetTrigger("GrabStartParam");
            }
            
        }
    }

    /// <summary>
    /// Dodge move.
    /// Has invulnerability on startup so can avoid attacks.
    /// Can dodge out of the getup animation to avoid meaty attacks (attacks that are timed to hit an opponent right as they get up from being knocked down).
    /// </summary>
    override protected void AbilityD() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle", "Stance", "GetUp" }))
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

    override protected void Super() { }

}
