using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public sealed class EnterMineAndDigForNugget:State<Miner>
{


	IEnumerator wait(int t){
		yield return new WaitForSeconds (t);
	}

	public override void Enter (Miner m)
	{
		//Debug.Log(Time.frameCount);
		if (m.Location != Locations.goldmine) {
			m.stateText.text = "Miner Bob:  Entering the mine...";
			//if (Vector3.Distance (m.transform.position, GameObject.FindGameObjectWithTag ("goldmine").transform.position) >= 0.01f) 
			m.OnRoad (GameObject.FindGameObjectWithTag ("goldmine").transform.position);
			m.Location = Locations.goldmine;
		}
	}



	public override void Execute (Miner m)
	{
		if (Vector3.Distance (m.transform.position, GameObject.FindGameObjectWithTag ("goldmine").transform.position) == 0.0f) {
			//StartCoroutine (wait (2));
			m.AddToGoldCarried (1);
			//m.Thirst++;
			m.stateText.text = "Picking up nugget and that's..." + m.GoldCarried;
			Debug.Log ("Picking up nugget and that's..." + m.GoldCarried);
			m.IncreaseFatigue ();
			if (m.PocketsFull ()) {
				m.ChangeState (new VisitBankAndDepositGold ());
			}
			if (m.Thirsty ()) {
				m.ChangeState (new QuenchThirst ());
			}
		}
	}

	public override void Exit (Miner m)
	{
		m.stateText.text = "Miner Bob:  Leaving the mine with my pockets full...";
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
			m.stateText.text = "Miner Bob:  Entering the Bank...";
			Debug.Log ("Miner Bob:  Entering the Bank...");
			m.OnRoad (GameObject.FindGameObjectWithTag ("bank").transform.position);
			m.ChangeLocation (Locations.bank);
		}
	}

	public override void Execute (Miner m)
	{
		if (Vector3.Distance (m.transform.position, GameObject.FindGameObjectWithTag ("bank").transform.position) == 0.0f) {

		m.MoneyInBank += m.GoldCarried;
		m.GoldCarried = 0;
			m.stateText.text = "Miner Bob:  Depositing gold to bank...";
		Debug.Log ("Miner Bob:  Depositing gold to bank...");
		if (m.RichEnough ()) {
				m.stateText.text = "Miner Bob:  WooHoo...rich enough to get back to my honey....";
			Debug.Log ("Miner Bob:  WooHoo...rich enough to get back to my honey....");
			m.ChangeState (new GoHomeAndSleepTillReseted ());
		} else {
			m.ChangeState (new EnterMineAndDigForNugget ());
		}
		}
	}

	public override void Exit (Miner m)
	{
		m.stateText.text = "Miner Bob:  Leaving the bank with rich enough";
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
			m.stateText.text = "Miner Bob: Going back to home";
			Debug.Log ("Miner Bob: Going back to home");
			m.OnRoad (GameObject.FindGameObjectWithTag ("home").transform.position);
			m.ChangeLocation (Locations.home);
			Messaging.DispatchMessage (0, m.ID, m.wifeId, MessageType.HiHoneyImHome);
		}
	}

	public override void Execute (Miner m)
	{
		if (Vector3.Distance (m.transform.position, GameObject.FindGameObjectWithTag ("home").transform.position) == 0.0f) {
			if (m.Fatigue < m.Tiredness) {
				m.stateText.text = "Miner Bob:  All mah fatigued has drained";
				Debug.Log ("Miner Bob:  All mah fatigued has drained");
				m.ChangeState (new EnterMineAndDigForNugget ());
			} else {
				m.Fatigue--;
				m.stateText.text = "Miner Bob:  ZZZZZ.....";
				Debug.Log ("Miner Bob:  ZZZZZ.....");
			}
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
			m.stateText.text = "Miner Bob:  Message handled by Miner at time " + Time.frameCount;
			Debug.Log ("Miner Bob:  Message handled by Miner at time " + Time.frameCount);
			m.stateText.text = "Miner Bob:  Okay Hun, ahm a comin'!";
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
			m.stateText.text = "Miner Bob:  need some fun dude...";
			Debug.Log ("Miner Bob:  need some fun dude...");
			m.OnRoad (GameObject.FindGameObjectWithTag ("bar").transform.position);
			m.ChangeLocation (Locations.bar);
		}
	}

	public override void Execute (Miner m)
	{
		if (Vector3.Distance (m.transform.position, GameObject.FindGameObjectWithTag ("bar").transform.position) == 0.0f) {
			m.Thirst = 0;
			m.MoneyInBank -= 2;
			m.stateText.text = "Miner Bob:  That's mighty tidy funny....";
			Debug.Log ("Miner Bob:  That's mighty tidy funny....");
			m.ChangeState (new EnterMineAndDigForNugget ());
		}
	}

	public override void Exit (Miner m)
	{
		m.stateText.text = "Miner Bob:  better leave the bar.....";
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
	


