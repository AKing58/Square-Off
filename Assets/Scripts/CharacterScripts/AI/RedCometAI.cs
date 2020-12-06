using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RedCometAI : StateMachine
{
	bool StayInStance = false;

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

	//Determines when the ai should sleep
	bool shouldSleep()
    {
		return opponentRef == null;
    }

	//Determines when the ai should seek
	bool shouldSeek()
	{
		return opponentRef == null;
	}

	//Determines when the ai should approach
	bool shouldApproach()
	{
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

	//Logic for what to do when trying to approach the target
	void approach()
    {
        if (parent.InValidAnim("Stance"))
        {
			if(StayInStance)
				parent.setTargetVec((opponentRef.transform.position - parent.transform.position).normalized);
			else
				parent.ActivateInputD();
		}

		int attackChance = Random.Range(0, 100);

		if(attackChance <= 95)
			parent.setTargetVec((opponentRef.transform.position - parent.transform.position).normalized);
        else
        {
			parent.ActivateInputB();
			StartCoroutine(StanceDelay());
		}
    }

	//Makes the AI stay in stance for at least 2 seconds
	IEnumerator StanceDelay()
    {
		StayInStance = true;
		yield return new WaitForSeconds(2f);
		StayInStance = false;
    }

	//Determines if the ai should attack the player
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

	//Determines if the player is in front of the ai
	bool isFacing(Transform target)
    {
		return Vector3.Dot((target.position - transform.position).normalized, transform.forward) > 0.9f;
    }

	//Logic for attacking
	void attack()
	{
		if (parent.InValidAnim("Rekka1"))
		{
			parent.ActivateInputA();
		}
		else if(parent.InValidAnim("Stance"))
		{
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
			//Debug.Log("No Attack Available");
        }
	}

	
	//Used to change targets if the current player has died
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
