using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	protected enum AIState { SPAWNEDIN, SLEEP, SEEK, APPROACH, ATTACK, RETREAT, DEAD }

	[SerializeField]
	protected AIState curState = AIState.SLEEP;
	[SerializeField]
	protected AIState prevState = AIState.SLEEP;
	[SerializeField]
	protected bool pauseAI = false;

	[SerializeField]
	protected PlayerHandler parent;
	[SerializeField]
	protected GameManager gmRef;
	[SerializeField]
	protected GameObject opponentRef;

	//[SerializeField]
	//protected Dictionary<string,string> states = new Dictionary<string,string>();

	//# Runs every physics tick
	void FixedUpdate() {
		if (curState != null && !pauseAI) {
			StateLogic();
			AIState transition = GetTransition();
			if (transition != null) {
				SetState(transition);
			}
		}
	}

	// Performs a voidtion if the current state should constantly perform a funciton
	virtual protected void StateLogic() { }

	// Determine the next state that the state machine should transition into, given certain conditions are met
	virtual protected AIState GetTransition() => AIState.SPAWNEDIN;

	//Logic for when entering states from another
	virtual protected void EnterState(AIState newState, AIState oldState) { }

	//Logic for when exiting states
	virtual protected void ExitState(AIState oldState, AIState newState) { }

	//Runs the enter and exit state functions when changing states
	protected void SetState(AIState newState) {
		prevState = curState;
		curState = newState;

		if (prevState != null)
			ExitState(prevState, newState);
		if (newState != null)
			EnterState(newState, prevState);
	}
}
