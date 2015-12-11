using UnityEngine;
using System.Collections;

public class Unit:MonoBehaviour{
	
	
	//public Transform target;
	float speed = 2;
	Vector3[] path;
	int targetIndex;
	
	public void findingPath(Vector3 targetPos) {
		//target.position = Vector3(0f,0f,0f);
		PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
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
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			Debug.Log("MOOOOOOOOOOOOOVVVVVVVVVVVVVVIIIIIIIIIIIIIIINNNNNNNNNGGGGGG");

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
