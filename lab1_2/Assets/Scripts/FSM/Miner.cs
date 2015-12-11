using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Locations { goldmine, bar, bank, home };

public class Miner : Agent {
	
	private FiniteStateMachine<Miner> FSM;
	
	public Locations  Location = Locations.home;
	public int           GoldCarried = 0;
	public int           MoneyInBank  = 0;
	public int           Thirst = 5;
	public int           Fatigue = 0;
	public int 			 Tiredness = 2;
	public int 			wifeId = 1;

	Unit unit;


	public void Start() {
		Debug.Log("Miner awakes...");
		FSM = new FiniteStateMachine<Miner>();
		FSM.Configure(this, new EnterMineAndDigForNugget());
		FSM.GlobalState = new MinerGlobalState();
		//wifeId = this.ID+1;
		AgentManager.AddAgent(this);
		unit = gameObject.AddComponent<Unit>();
	}
	
	public void ChangeState(State<Miner> e) {
		FSM.ChangeState(e);
	}

	
	public override void Update() {
		Thirst++;
		FSM.Update();
		Messaging.SendDelayedMessages();
		switch(Location){
		case Locations.bank:
			transform.position = GameObject.FindGameObjectWithTag("bank").transform.position;
			break;
		case Locations.home:
			transform.position = GameObject.FindGameObjectWithTag("home").transform.position;
			break;
		case Locations.goldmine:
			transform.position = GameObject.FindGameObjectWithTag("goldmine").transform.position;
			break;
		case Locations.bar:
			transform.position = GameObject.FindGameObjectWithTag("bar").transform.position;
			break;
		default:
			break;
		}
	}
	
	public void ChangeLocation(Locations l) {
		Location = l;
	}
	
	public void AddToGoldCarried(int amount) {
		GoldCarried += amount;
	}
	
	public void AddToMoneyInBank(int amount ) {
		MoneyInBank += amount;
		GoldCarried = 0;
	}

	public void OnRoad(Vector3 targetPos){
		unit.findingPath(targetPos);
	}
	
	public bool RichEnough() {
		if(MoneyInBank>=10){
			return true;
		}else{
			return false;
		}
	}
	
	public bool PocketsFull() {
		bool full = GoldCarried >=  2 ? true : false;
		return full;
	}
	
	public bool Thirsty() {
		bool thirsty = Thirst >= 10 ? true : false;
		return thirsty;
	}

	public void RevertToPreviousState(){
		FSM.RevertToPreviousState();
	}

	public override bool HandleMessage(Telegram telegram)
	{
		return FSM.HandleMessage(telegram);    
	}
	
	public void IncreaseFatigue() {
		Fatigue++;
	}
}
