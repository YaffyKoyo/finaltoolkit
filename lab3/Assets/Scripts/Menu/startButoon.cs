using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class startButoon : MonoBehaviour {

	public void StartGame(){
		SceneManager.LoadScene("lab3_v1");
	}
}
