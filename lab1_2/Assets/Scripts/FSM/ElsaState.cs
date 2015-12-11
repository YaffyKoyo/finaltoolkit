using UnityEngine;
using System.Collections;

public sealed class DoHouseWork:State<Elsa>
{

	public override void Enter (Elsa e)
	{
		Debug.Log ("Elsa: Time to do some housework....");
	}

	public override void Execute (Elsa e)
	{
		if (Random.value > 0.2) {
			Debug.Log ("Elsa: washing dishes.............");
		} else {
			e.ChangeState (new VisitBathroom ());
		}
	}

	public override void Exit (Elsa e)
	{
		Debug.Log ("Elsa: Everthing is clean.....");
	}

	public override bool OnMessage (Elsa e, Telegram telegram)
	{
		return false;
	}
}

public sealed class VisitBathroom:State<Elsa>
{
	public override void Enter (Elsa e)
	{
		Debug.Log ("Elsa: Walkin' into the can, need to wash me pretty....");
	}

	public override void Execute (Elsa e)
	{
		Debug.Log ("Elsa: ALLLLL!!!!!!!!!!!!!!!!!");
		e.RevertToPreviousState ();
	}

	public override void Exit (Elsa e)
	{
		Debug.Log ("Elsa: Leaving the bathRoom....");
	}

	public override bool OnMessage (Elsa e, Telegram telegram)
	{
		return false;
	}
}

public sealed class CookStew:State<Elsa>
{
	public override void Enter (Elsa e)
	{
		if (!e.Cooking) {
			Debug.Log ("Elsa: Putting stew in kitchen....");
			Messaging.DispatchMessage (2, e.ID, e.ID, MessageType.StewsReady);
			Debug.Log("Elsa: dont't foget to get the stew at"+Time.frameCount);
			e.Cooking = true;
		}
	}

	public override void Execute (Elsa e)
	{
		Debug.Log ("Elsa: fussin over food");
	}

	public override void Exit (Elsa e)
	{
		Debug.Log ("Elsa: putting the stew on the table....");
	}

	public override bool OnMessage (Elsa e, Telegram telegram)
	{
		switch (telegram.messageType) {
		case MessageType.HiHoneyImHome:
			// Ignored here; handled in WifesGlobalState below
			return false;
		case MessageType.StewsReady:
			// Tell Miner that the stew is ready now by sending a message with no delay
			Debug.Log ("Elsa: Message StewsReady handled by Elsa");
			Debug.Log ("Elsa: StewReady! Lets eat");
			Messaging.DispatchMessage (0, e.ID, e.husbandId, MessageType.StewsReady);
			e.Cooking = false;
			e.ChangeState (new DoHouseWork ());
			return true;
		default:
			return false;
		}
	}
	
}

public sealed class GlobalState:State<Elsa>
{
	public override void Enter (Elsa e)
	{
		
	}

	public override void Execute (Elsa e)
	{
		
	}

	public override void Exit (Elsa e)
	{
	}

	public override bool OnMessage (Elsa e, Telegram telegram)
	{
		switch (telegram.messageType) {
		case MessageType.HiHoneyImHome:
			Debug.Log ("Elsa: Message HiHoneImHome handled by Elsa");
			Debug.Log ("Elsa: Hi honey. Let me make you some of mah fine country stew");
			e.ChangeState (new CookStew ());
			return true;
		case MessageType.StewsReady:
			return false;
		default:
			return false;
		}                 
	}
}