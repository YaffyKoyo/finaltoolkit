using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class Agent : MonoBehaviour {
	private static int agents = 0;

	public int ID;

	public Agent(){
		ID = agents++;
	}

	abstract public void Update();
	abstract public bool HandleMessage(Telegram telegram);
}
