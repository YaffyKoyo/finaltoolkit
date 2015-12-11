using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;             //Minimum value for our Count class.
		public int maximum;             //Maximum value for our Count class.
		//Assignment constructor.
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
	public int columns = 8;                                         //Number of columns in our game board.
	public int rows = 8;                                            //Number of rows in our game board.
	public Count wallCount = new Count (5, 9);                      //Lower and upper limit for our random number of walls per level.
	public Count mineCount = new Count (1, 1);                      //Lower and upper limit for our random number of food items per level.
	public GameObject[] barTiles;
	public GameObject[] bankTiles;
	public GameObject[] floorTiles;                                 //Array of floor prefabs.
	public GameObject[] wallTiles;                                  //Array of wall prefabs.
	public GameObject[] homeTils;                                  //Array of food prefabs.
	public GameObject[] mineTiles;
	public GameObject[] outerWallTiles;  
	//public GameObject player;

	private Transform boardHolder;
	private List<Vector3>gridPostions = new List<Vector3>();

	void InitialiseList(){
		gridPostions.Clear();
		for(int x=1;x<columns-1;x++){
			for(int y=1;y<rows-1;y++){
				gridPostions.Add(new Vector3(x,y,0f));
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject("Board").transform;
		for(int x=-1;x<columns+1;x++){
			for(int y=-1;y<rows+1;y++){
				GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
				if(x==-1||x==columns||y==-1||y==rows)
					toInstantiate=outerWallTiles[Random.Range(0,outerWallTiles.Length)];
				GameObject instatnce=
					Instantiate(toInstantiate,new Vector3(x,y,0f),Quaternion.identity) as GameObject;
				instatnce.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition(){
		int randomIndex = Random.Range(0,gridPostions.Count);
		Vector3 randomPosition = gridPostions[randomIndex];
		gridPostions.RemoveAt(randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range(minimum, maximum+1);
		for(int i=0;i<objectCount;i++){
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene (int level)
	{
		//Creates the outer walls and floor.
		BoardSetup ();
		
		//Reset our list of gridpositions.
		InitialiseList ();

		LayoutObjectAtRandom(bankTiles,1,1);

		LayoutObjectAtRandom(barTiles,1,1);

		LayoutObjectAtRandom(mineTiles,1,1);

		LayoutObjectAtRandom (wallTiles, wallCount.minimum+3, wallCount.maximum);
		
		LayoutObjectAtRandom (homeTils, mineCount.minimum, mineCount.maximum);

		//Instantiate (player, new Vector3 (0f, 0f, 0f), Quaternion.identity);
	}
}
