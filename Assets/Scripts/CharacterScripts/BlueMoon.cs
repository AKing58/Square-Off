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

    /// <summary>
    /// Light attack.
    /// Rekka style attack that can be done 3 times in a row.
    /// </summary>
    override protected void AbilityA() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle", "Rekka1", "Rekka1 0" }))
        {
            anim.SetTrigger("Rekka1Param");

        }
    }

    /// <summary>
    /// Forward moving forward kick.
    /// Knocks the enemy backwards upon being hit.
    /// </summary>
    override protected void AbilityB() 
    {
        if (InValidAnim(new string[] { "Walk", "Idle", "Rekka1 0" }))
        {
            anim.SetTrigger("BootParam");
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
