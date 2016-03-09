using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	private int level = 3;
	GameObject Elsa;
	GameObject Miner;
	GameObject playerSpawner;
	Grid gridGen;
	//Astar Astar;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		gridGen = GetComponent<Grid>();

		boardScript = GetComponent<BoardManager> ();
//		Elsa = GameObject.Find("ElsatheFinder");
		InitGame ();
	}

	void InitGame ()
	{
		boardScript.SetupScene (level);
		gridGen.CreateGrid();
		playerSpawner = GameObject.Find("PlayerSpawner");
	}
		

	void Update ()
	{
		
	}
}
