using UnityEngine;
using System.Collections;

public class startButoon : MonoBehaviour {

	public void LoadScene(string name){
		Application.LoadLevel(name);
	}
}
