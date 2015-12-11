using UnityEngine;
using System.Collections;

public class Elsa : Agent {

	public int husbandId = 0;
	public bool Cooking = false;
	//public AgentManager manager;

	private FiniteStateMachine<Elsa> FSM;
	public void Awake() {

		AgentManager.AddAgent(this);

		Debug.Log("Elsa awakes...");
		FSM = new FiniteStateMachine<Elsa>();
		FSM.Configure(this, new DoHouseWork());
		FSM.GlobalState = new GlobalState();
		Cooking = false;
	}

	public void ChangeState(State<Elsa> e) {
		FSM.ChangeState(e);
	}
	
	public override void Update() {
		FSM.Update();
		Messaging.SendDelayedMessages();
		transform.position = GameObject.FindGameObjectWithTag("home").transform.position;
	}

	public override bool HandleMessage(Telegram telegram)
	{
		return FSM.HandleMessage(telegram);    
	}

	public void RevertToPreviousState(){
		FSM.RevertToPreviousState();
	}

	//private bool cooking;
}
