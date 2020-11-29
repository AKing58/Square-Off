using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RedCometAI : StateMachine
{
	

	public void InitializeAI(GameManager gmInput)
    {
		SetState(AIState.SLEEP);
		parent = gameObject.GetComponent<RedComet>();
		gmRef = gmInput;
	}


	override protected void StateLogic() {
		if (curState == AIState.SEEK)
			seekOpponent();
		else if (curState == AIState.APPROACH)
			approach();
	}


	override protected AIState GetTransition() {
		switch (curState)
        {
			case AIState.SPAWNEDIN:
				return AIState.SEEK;
			case AIState.SLEEP:
				if (shouldAttack())
					return AIState.ATTACK;
				else if (shouldSeek())
					return AIState.SEEK;
				else if (shouldApproach())
					return AIState.APPROACH;
				break;
			case AIState.SEEK:
				if (shouldAttack())
					return AIState.ATTACK;
				else if (shouldApproach())
					return AIState.APPROACH;
				break;
			case AIState.APPROACH:
				if (shouldAttack())
					return AIState.ATTACK;
				else if (shouldSeek())
					return AIState.SEEK;
				else if (shouldApproach())
					break;
				break;
			case AIState.ATTACK:
				if (shouldApproach())
					return AIState.APPROACH;
				break;
			case AIState.RETREAT:
				break;
			case AIState.DEAD:
				pauseAI = true;
				parent.setTargetVec(new Vector3());
				break;
			default:
				break;

		}
			
		return curState;
	}


	override protected void EnterState(AIState newState, AIState oldState) {
		switch (newState)
        {
			case AIState.SLEEP:
				break;
			case AIState.SEEK:
				seekOpponent();
				break;
			case AIState.ATTACK:
				attack();
				break;
			default:
				break;
		}
	}
		

	override protected void ExitState(AIState oldState, AIState newState){

    }

	bool shouldSleep()
    {
		//Debug.Log("s Sleep");
		return opponentRef == null;
    }

	bool shouldSeek()
	{
		//Debug.Log("s seek");
		return opponentRef == null;
	}
	bool shouldApproach()
	{
		//Debug.Log("s approach");
		if (
			opponentRef != null &&
			parent.GetComponent<PlayerHandler>().InValidAnim(new string[] { "Walk", "Idle"}) &&
			Vector3.Distance(parent.transform.position, opponentRef.transform.position) > 1
			)
		{
			return true;
		}
		else if (
			opponentRef != null &&
			parent.GetComponent<PlayerHandler>().InValidAnim("Stance") &&
			Vector3.Distance(parent.transform.position, opponentRef.transform.position) > 2.5
            )
        {
			return true;
        }
			return false;
	}

	void approach()
    {
        if (parent.InValidAnim("Stance"))
        {
			parent.ActivateInputD();
        }
		parent.setTargetVec((opponentRef.transform.position - parent.transform.position).normalized);
    }

	bool shouldAttack()
	{
		//Debug.Log("s attack");
		if (
			opponentRef != null &&
			parent.GetComponent<PlayerHandler>().InValidAnim(new string[] { "Walk", "Idle" }) &&
			Vector3.Distance(parent.transform.position, opponentRef.transform.position) < 1 &&
			isFacing(opponentRef.transform)
			)
        {
			return true;
        }else if (
			opponentRef != null &&
			parent.GetComponent<PlayerHandler>().InValidAnim("Stance") &&
			Vector3.Distance(parent.transform.position, opponentRef.transform.position) < 2.5f &&
			isFacing(opponentRef.transform)
			)
        {
			return true;
        }
			return false;
	}

	bool isFacing(Transform target)
    {
		return Vector3.Dot((target.position - transform.position).normalized, transform.forward) > 0.9f;
    }

	void attack()
	{
		if (parent.InValidAnim("Rekka1"))
		{
			Debug.Log("Combo Attack");
			parent.ActivateInputA();
		}
		else if(parent.InValidAnim("Stance"))
		{
			Debug.Log("Stance Attack");
			if (Vector3.Distance(parent.transform.position, opponentRef.transform.position) < 1)
				parent.ActivateInputA();
			if (Vector3.Distance(parent.transform.position, opponentRef.transform.position) < 2.5f)
				parent.ActivateInputB();
		}
		else if (parent.InValidAnim(new string[] { "Walk", "Idle" }))
		{
			if(parent.Energy >= 100)
            {
				parent.ActivateInputC();
            }
			int attackID = Random.Range(0, 3);
			switch (attackID)
			{
				case 0:
					parent.ActivateInputA();
					break;
				case 1:
					parent.ActivateInputB();
					break;
				case 2:
					parent.ActivateInputD();
					break;
				default:
					break;
			}
        }
        else
        {
			Debug.Log("No Attack Available");
        }
		/*
		int attackID = Random.Range(0, 2);
		switch (attackID)
		{
			case 0:
				parent.ActivateInputA();
				break;
			case 1:
				parent.ActivateInputB();
				break;
			default:
				break;
		}
		*/
	}

	bool hasAttacked()
    {
		return parent.GetComponent<PlayerHandler>().InValidAnim(new string[] { "Grab", "Rekka1" });
    }

	

	void seekOpponent()
    {
		Debug.Log("seeking");
		List<GameObject> phList = gmRef.Players;
		GameObject opponent = phList[0];
		foreach(GameObject go in phList)
        {
			if(go.GetComponent<PlayerHandler>().Health > opponent.GetComponent<PlayerHandler>().Health)
            {
				opponent = go;
            }
        }
		opponentRef = opponent;
    }
}
