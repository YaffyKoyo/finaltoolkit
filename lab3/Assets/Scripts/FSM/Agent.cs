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

	float speed = 2;
	Vector3[] path;
	int targetIndex;
	
	public void findingPath(Vector3 targetPos) {
		if(Vector3.Distance(transform.position,targetPos)!=0){
			//Debug.Log("hehe");
			PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
		}
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];
		
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					targetIndex = 0;
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			Debug.Log("MOVINGGGGGGGGGGG");
			
			yield return null;	
		}
	}
	
	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color= new Color(0,140,0,0.2f);
				Gizmos.DrawCube(path[i], Vector3.one);
				
				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
