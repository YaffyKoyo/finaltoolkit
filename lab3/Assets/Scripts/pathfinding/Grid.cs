using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
	public Node[,]grid;
	public LayerMask BlockingLayer;
	public List<Node> path;

	void Awake ()
	{
		CreateGrid ();
	}

	public void CreateGrid ()
	{
		grid = new Node[8, 8];
		for(int x=0;x<8;x++){
			for(int y=0;y<8;y++){
				bool walkable = true;

				Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(x-0.2f,y-0.2f),new Vector2(x+0.2f,y+0.2f),BlockingLayer);
				if(colliders.Length>0)
					walkable = false;
				if(!walkable){
					//Debug.Log(x+","+y);
				}
				grid[x,y] = new Node(walkable,new Vector3(x,y,0f),x,y);
			}
		}
	}

	public List<Node> GetNeighbours (Node node)
	{
		List<Node> neighbours = new List<Node> ();
		
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;
				
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				
				if (checkX >= 0 && checkX < 8 && checkY >= 0 && checkY < 8) {
					neighbours.Add (grid [checkX, checkY]);
				}
			}
		}
		return neighbours;
	}

	public Node NodeFromWorldPoint (Vector3 worldPosition)
	{
		int x = Mathf.RoundToInt (worldPosition.x);
		int y = Mathf.RoundToInt (worldPosition.y);
		return grid [x, y];
	}
}
