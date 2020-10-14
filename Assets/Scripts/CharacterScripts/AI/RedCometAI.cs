using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RedCometAI : StateMachine
{
	

	public void InitializeAI(GameManager gmInput)
    {
		SetState(State.SLEEP);
		parent = gameObject.GetComponent<PlayerHandler>();
		gmRef = gmInput;
	}


	override protected void StateLogic() {
		if (curState == State.SEEK)
			seekOpponent();
		else if (curState == State.APPROACH)
			approach();
	}


	override protected State GetTransition() {
		switch (curState)
        {
			case State.SPAWNEDIN:
				return State.SEEK;
			case State.SLEEP:
				if (shouldAttack())
					return State.ATTACK;
				else if (shouldSeek())
					return State.SEEK;
				else if (shouldApproach())
					return State.APPROACH;
				break;
			case State.SEEK:
				if (shouldAttack())
					return State.ATTACK;
				else if (shouldApproach())
					return State.APPROACH;
				break;
			case State.APPROACH:
				if (shouldAttack())
					return State.ATTACK;
				else if (shouldSeek())
					return State.SEEK;
				else if (shouldApproach())
					break;
				break;
			case State.ATTACK:
				if (hasAttacked())
					return State.SLEEP;
				break;
			case State.RETREAT:
				break;
			case State.DEAD:
				pauseAI = true;
				parent.setTargetVec(new Vector3());
				break;
			default:
				break;

		}
			
		return curState;
	}


	override protected void EnterState(State newState, State oldState) {
		switch (newState)
        {
			case State.SLEEP:
				break;
			case State.SEEK:
				seekOpponent();
				break;
			case State.ATTACK:
				attack();
				break;
			default:
				break;
		}
	}
		

	override protected void ExitState(State oldState, State newState){

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
			parent.GetComponent<PlayerHandler>().InValidAnim(new string[] { "Walk", "Idle" }) &&
			Vector3.Distance(parent.transform.position, opponentRef.transform.position) > 1
			)
		{
			return true;
		}
		return false;
	}

	void approach()
    {
		parent.setTargetVec((opponentRef.transform.position - parent.transform.position).normalized);
    }

	bool shouldAttack()
	{
		//Debug.Log("s attack");
		if (
			opponentRef != null &&
			parent.GetComponent<PlayerHandler>().InValidAnim(new string[]{ "Walk", "Idle" }) &&
			Vector3.Distance(parent.transform.position, opponentRef.transform.position) < 1
			)
        {
			return true;
        }
		return false;
	}

	void attack()
	{
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
