using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public static Vector3 ReachedPoint;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other){
		// check if collider's tag contains "Player"
		if (other.tag.Contains("Player")) {
			if (other.transform.position.x > ReachedPoint.x){
				// checkpoint's position = player's position
				ReachedPoint = other.transform.position;
			}
		}


	}
}
