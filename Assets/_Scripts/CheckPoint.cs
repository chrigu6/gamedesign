using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public static Vector3 ReachedPoint;
	public AudioClip checkPointClip;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per fram
	void Update () {
	}


	void OnTriggerEnter(Collider other){
		// check if collider's tag contains "Player"
		if (other.tag.Contains("Player")) {
			if (other.transform.position.x > ReachedPoint.x){
				AudioSource.PlayClipAtPoint(this.checkPointClip,transform.position);
				// checkpoint's position = player's position
				ReachedPoint = transform.position;
			}
		}


	}
}
