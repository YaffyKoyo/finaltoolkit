using UnityEngine;
using System.Collections;

public class buttonReset: MonoBehaviour{

	public Texture2D buttonImage = null;

	private void OnGUI(string name){
		if(GUI.Button(new Rect(Screen.height-1,Screen.width-1,1.0f,1.0f), buttonImage)){
			Application.LoadLevel(name);	
		}
	}
}
