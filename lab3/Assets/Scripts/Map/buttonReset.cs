using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class buttonReset: MonoBehaviour{

	public Texture2D buttonImage = null;

	public void StartNewGame(){
		EditorSceneManager.LoadScene ("lab3_v1");
	}
}
