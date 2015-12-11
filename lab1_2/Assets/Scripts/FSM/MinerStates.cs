using UnityEngine;
using System.Collections;

public sealed class EnterMineAndDigForNugget:State<Miner>
{

	public override void Enter (Miner m)
	{
		if (m.Location != Locations.goldmine) {
			Debug.Log ("Miner Bob:  Entering the mine...");
//			m.OnRoad (GameObject.FindGameObjectWithTag ("goldmine").transform.position);
		}
		//if (Vector3.Distance (m.transform.position, GameObject.FindGameObjectWithTag ("goldmine").transform.position) == 0.0f) {
			m.ChangeLocation (Locations.goldmine);//}
	}

	public override void Execute (Miner m)
	{
	//	if (m.Location==Locations.goldmine){//Vector3.Distance (m.transform.position, GameObject.FindGameObjectWithTag ("goldmine").transform.position) == 0.0f) {
			m.AddToGoldCarried (1);
			//m.Thirst++;
			Debug.Log ("Picking up nugget and that's..." + m.GoldCarried);
			m.IncreaseFatigue ();
			if (m.PocketsFull ()) {
				m.ChangeState (new VisitBankAndDepositGold ());
			}
			if (m.Thirsty ()) {
				m.ChangeState (new QuenchThirst ());
			}
	//	}
	}

	public override void Exit (Miner m)
	{
		Debug.Log ("Miner Bob:  Leaving the mine with my pockets full...");
	}

	public override bool OnMessage (Miner agent, Telegram telegram)
	{
		return false;    
	}
}

public sealed class VisitBankAndDepositGold:State<Miner>
{

	public override void Enter (Miner m)
	{
		if (m.Location != Locations.bank) {
			Debug.Log ("Miner Bob:  Entering the Bank...");
			m.ChangeLocation (Locations.bank);
		}
	}

	public override void Execute (Miner m)
	{
		m.MoneyInBank += m.GoldCarried;
		m.GoldCarried = 0;
		Debug.Log ("Miner Bob:  Depositing gold to bank...");
		if (m.RichEnough ()) {
			Debug.Log ("Miner Bob:  WooHoo...rich enough to get back to my honey....");
			m.ChangeState (new GoHomeAndSleepTillReseted ());
		} else {
			m.ChangeState (new EnterMineAndDigForNugget ());
		}
	}

	public override void Exit (Miner m)
	{
		Debug.Log ("Miner Bob:  Leaving the bank with rich enough");
	}

	public override bool OnMessage (Miner agent, Telegram telegram)
	{
		return false;
	}
}

public sealed class GoHomeAndSleepTillReseted:State<Miner>
{
	public override void Enter (Miner m)
	{
		if (m.Location != Locations.home) {
			Debug.Log ("Miner Bob: Going back to home");
			m.ChangeLocation (Locations.home);
			Messaging.DispatchMessage (0, m.ID, m.wifeId, MessageType.HiHoneyImHome);
		}
	}

	public override void Execute (Miner m)
	{
		if (m.Fatigue < m.Tiredness) {
			Debug.Log ("Miner Bob:  All mah fatigued has drained");
			m.ChangeState (new EnterMineAndDigForNugget ());
		} else {
			m.Fatigue--;
			Debug.Log ("Miner Bob:  ZZZZZ.....");
		}
	}

	public override void Exit (Miner m)
	{
	}

	public override bool OnMessage (Miner m, Telegram telegram)
	{
		switch (telegram.messageType) {
		case MessageType.HiHoneyImHome:
			return false;
		case MessageType.StewsReady:
			Debug.Log ("Miner Bob:  Message handled by Miner at time " + Time.frameCount);
			Debug.Log ("Miner Bob:  Okay Hun, ahm a comin'!");
			m.ChangeState (new EatStew ());
			return true; 
		default:
			return false;
		}
	}
}

public sealed class QuenchThirst:State<Miner>
{
	public override void Enter (Miner m)
	{
		if (m.Location != Locations.bar) {
			Debug.Log ("Miner Bob:  need some fun dude...");
			m.ChangeLocation (Locations.bar);
		}
	}

	public override void Execute (Miner m)
	{
		m.Thirst = 0;
		m.MoneyInBank -= 2;
		Debug.Log ("Miner Bob:  That's mighty tidy funny....");
		m.ChangeState (new EnterMineAndDigForNugget ());
	}

	public override void Exit (Miner m)
	{
		Debug.Log ("Miner Bob:  better leave the bar.....");
	}

	public override bool OnMessage (Miner agent, Telegram telegram)
	{
		return false;
	}
}

public class EatStew : State<Miner>
{
	public override void Enter (Miner m)
	{
		Debug.Log ("Miner Bob: Smells Reaaal goood Elsa!");
	}
	
	public override void Execute (Miner m)
	{
		Debug.Log ("Miner Bob: Tastes real good too!");
		m.RevertToPreviousState ();
	}

	public override void Exit (Miner m)
	{
		Debug.Log ("Miner Bob: Thankya li'lle lady. Ah better get back to whatever ah wuz doin'");
	}
	
	public override bool OnMessage (Miner agent, Telegram telegram)
	{
		return false;
	}
}

public sealed class MinerGlobalState:State<Miner>
{
	public override void Enter (Miner m)
	{
	}

	public override void Execute (Miner m)
	{
	}

	public override void Exit (Miner m)
	{
	}

	public override bool OnMessage (Miner agent, Telegram telegram)
	{
		return false;
	}
}


