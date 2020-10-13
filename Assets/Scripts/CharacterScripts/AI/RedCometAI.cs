using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCometAI : StateMachine
{
	

	void Start()
    {
		SetState(State.SLEEP);
		parent = gameObject.GetComponent<PlayerHandler>();
	}


	override protected void StateLogic() {
		if (curState == State.SEEK)
        {
			seekOpponent();
		}
	}


	override protected State GetTransition() {
		//if (parent.is_dead())
		//	return State.DEAD;
		switch (curState)
        {
			case State.SLEEP:
				if (shouldAttack())
					return State.ATTACK;
				else if (shouldSeek())
					return State.SEEK;
				break;
			case State.SEEK:
				if (shouldSleep())
					return State.SLEEP;
				else if (shouldAttack())
					return State.ATTACK;
				break;
			case State.ATTACK:
				if (hasAttacked())
					return State.SLEEP;
				break;
			case State.DEAD:
				break;
			default:
				break;

		}
			
		return State.DEAD;
	}


	override protected void EnterState(State newState, State oldState) {
		switch (newState)
        {
			case State.SLEEP:
				break;
			case State.SEEK:
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
		return true;
    }

	bool shouldSeek()
	{
		return true;
	}

	bool shouldAttack()
	{
		return true;
	}

	bool hasAttacked()
    {
		return true;
    }

	void attack()
    {

    }

	void seekOpponent()
    {

    }
}
