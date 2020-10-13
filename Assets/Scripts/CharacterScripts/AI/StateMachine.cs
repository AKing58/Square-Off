using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	protected enum State { SPAWNEDIN, SLEEP, SEEK, ATTACK, RETREAT, DEAD }

	[SerializeField]
	protected State curState = State.SLEEP;
	[SerializeField]
	protected State prevState = State.SLEEP;
	[SerializeField]
	protected bool pauseAI = false;

	protected PlayerHandler parent;

	//[SerializeField]
	//protected Dictionary<string,string> states = new Dictionary<string,string>();

	//# Runs every physics tick
	void FixedUpdate() {
		if (curState != null && !pauseAI) {
			StateLogic();
			State transition = GetTransition();
			if (transition != null) {
				SetState(transition);
			}
		}
	}

	//# Performs a voidtion if the current state should constantly perform a funciton
	virtual protected void StateLogic() { }

	//# Determine the next state that the state machine should transition into, given certain conditions are met
	virtual protected State GetTransition() => State.SPAWNEDIN;

	virtual protected void EnterState(State newState, State oldState) { }

	virtual protected void ExitState(State oldState, State newState) { }

	protected void SetState(State newState) {
		prevState = curState;
		curState = newState;

		if (prevState != null)
			ExitState(prevState, newState);
		if (newState != null)
			EnterState(newState, prevState);
	}

	//protected void AddState(string stateName)
 //   {
	//	states.Add(stateName);
	//}
		

}
