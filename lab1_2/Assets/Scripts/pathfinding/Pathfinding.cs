using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{

	//public Transform seeker, target;
	Grid grid;
	PathRequestManager requestManager;


	void Awake ()
	{
		grid = GetComponent<Grid> ();
		requestManager = GetComponent<PathRequestManager>();
	}

//	void Update ()
//	{
//		FindPath (seeker.position, target.position);
//	}

	public void StartFindPath (Vector3 startPos, Vector3 targetPos)
	{
		StartCoroutine (FindPath (startPos, targetPos));
	}

	IEnumerator FindPath (Vector3 startPos, Vector3 targetPos)
	{		

		bool pathSuccess = false;
		Vector3[] path = new Vector3[0];

		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);
		if (startNode.walkable && targetNode.walkable) {
			List<Node> openSet = new List<Node> ();
			HashSet<Node> closedSet = new HashSet<Node> ();
			openSet.Add (startNode);

			while (openSet.Count>0) {
				Node currentNode = openSet [0];
				for (int i = 1; i < openSet.Count; i ++) {
					if (openSet [i].fCost < currentNode.fCost || openSet [i].fCost == currentNode.fCost && openSet [i].hCost < currentNode.hCost) {
						currentNode = openSet [i];
					}
				}

				openSet.Remove (currentNode);
				closedSet.Add (currentNode);

				if (currentNode == targetNode) {
				//	RetracePath (startNode, targetNode);
				//	return;
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.walkable || closedSet.Contains (neighbour)) {
						continue;
					}
				
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance (neighbour, targetNode);
						neighbour.parent = currentNode;
					
						if (!openSet.Contains (neighbour))
							openSet.Add (neighbour);
					}
				}
			}
		}
		yield return null;
		if(pathSuccess){
			path = RetracePath(startNode,targetNode);
		}
		requestManager.FinishedProcessingPath(path,pathSuccess);
	}

	Vector3[] RetracePath (Node startNode, Node endNode)
	{
		List<Node> path = new List<Node> ();
		List<Vector3> pathPoints = new List<Vector3>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse ();

		for(int i=0;i<path.Count;i++){
			pathPoints.Add(path[i].worldPosition);
		}
		return pathPoints.ToArray();
		//grid.path = path;
	}

	int GetDistance (Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}

}
