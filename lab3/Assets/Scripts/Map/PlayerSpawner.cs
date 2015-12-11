using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject Miner;
	public float spawnTime = 3f;
	public int maxPlayer = 1;
	public int minPlayer = 0;

	void Start(){
		Instantiate(Miner,this.transform.position, this.transform.rotation);
	}
}
