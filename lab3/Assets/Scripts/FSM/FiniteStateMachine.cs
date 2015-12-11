using UnityEngine;
using System.Collections;
	
public class FiniteStateMachine <T>  {
	private T Owner;
	public State<T> CurrentState;
	public State<T> PreviousState;
	public State<T> GlobalState;
	
	public void Awake() {
		CurrentState = null;
		PreviousState = null;
		GlobalState = null;
	}
	
	public void Configure(T owner, State<T> InitialState) {
		Owner = owner;
		ChangeState(InitialState);
	}
	
	public void  Update() {
		if (GlobalState != null)  GlobalState.Execute(Owner);
		if (CurrentState != null) CurrentState.Execute(Owner);
	}
	
	public void  ChangeState(State<T> NewState) {
		PreviousState = CurrentState;
		if (CurrentState != null)
			CurrentState.Exit(Owner);
		CurrentState = NewState;
		if (CurrentState != null)
			CurrentState.Enter(Owner);
	}
	
	public void  RevertToPreviousState() {
		if (PreviousState != null)
			ChangeState(PreviousState);
	}

	public bool IsInState(State<T> state){
		return state.Equals(CurrentState);
	}

	public bool HandleMessage(Telegram telegram)
	{
		if (GlobalState != null)
		{
			if (GlobalState.OnMessage(Owner, telegram))
			{
				return true;
			}			
		}
		if (CurrentState != null)
		{
			if (CurrentState.OnMessage(Owner, telegram))
			{
				return true;
			}
		}
		return false;
	}
};
