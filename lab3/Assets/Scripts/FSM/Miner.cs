using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
	public Text stateText;

	//Unit unit;

	//private Unit unit;


	public void Awake() {
		stateText = GameObject.Find ("stateText").GetComponent<Text>();
		Debug.Log("Miner awakes...");
		FSM = new FiniteStateMachine<Miner>();
		FSM.Configure(this, new EnterMineAndDigForNugget());
		FSM.GlobalState = new MinerGlobalState();
		//wifeId = this.ID+1;
		AgentManager.AddAgent(this);
		//unit = GetComponent<Unit>();
	}
	
	public void ChangeState(State<Miner> e) {
		FSM.ChangeState(e);
	}

	void Start(){
		//OnRoad(GameObject.FindGameObjectWithTag("bar").transform.position);
	}

	
	public override void Update() {
		Thirst++;
		//Debug.Log(Time.frameCount);
		FSM.Update();
		Messaging.SendDelayedMessages();
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
		findingPath(targetPos);
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
		bool thirsty = Thirst >= 1000 ? true : false;
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
