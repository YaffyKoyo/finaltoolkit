using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public enum MessageType
{
	HiHoneyImHome,
	StewsReady
}

public struct Telegram
{
	public double DispatchTime;
	public int Sender;
	public int Receiver;
	public MessageType messageType;
	
	public Telegram (double dt, int s, int r, MessageType mt)
	{
		DispatchTime = dt;
		Sender = s;
		Receiver = r;
		messageType = mt;
	}
}

public static class Messaging
{
	public static List<Telegram> telegramQueue = new List<Telegram> ();

	public static void DispatchMessage (double delay, int sender, int receiver, MessageType messageType)
	{//////////  Index error.....
		Agent sendingAgent = AgentManager.GetAgent (sender);
		Agent receivingAgent = AgentManager.GetAgent (receiver);
		///////////////	
		Telegram telegram = new Telegram (0, sender, receiver, messageType);
		if (delay <= 0.0f) {
			Debug.Log ("Instant telegram dispatched by " + sender + " for " + receiver + " message is " + MessageToString (messageType));
			SendMessage (receivingAgent, telegram);
		} else {
			telegram.DispatchTime = (int)Time.frameCount + delay;
			telegramQueue.Add (telegram);
			Debug.Log ("Delayed telegram from " + sender + "to" + receiver + " recorded at time " + Time.frameCount);
		}
	}

	public static void SendDelayedMessages ()
	{
		for (int i = 0; i < telegramQueue.Count; i++) {
			if (telegramQueue [i].DispatchTime <= Time.frameCount) {
				Agent receivingAgent = AgentManager.GetAgent (telegramQueue [i].Receiver);
				SendMessage (receivingAgent, telegramQueue [i]);
				telegramQueue.RemoveAt (i);
			}
		}
	}

	public static void SendMessage (Agent agent, Telegram telegram)
	{
		if (!agent.HandleMessage (telegram)) {
			Debug.Log ("Message not handled");
		}
	}

	public static String MessageToString (MessageType messageType)
	{
		switch (messageType) {
		case MessageType.HiHoneyImHome:
			return "Hi Honey I'm Home";
		case MessageType.StewsReady:
			return "Stew's Ready";
		default:
			return "Not recognized";
		}
	}
}
