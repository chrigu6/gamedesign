using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public static Vector3 ReachedPointP1;
	public static Vector3 ReachedPointP2;
	
	public AudioClip checkPointClip;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per fram
	void Update () {
	}
	
	
	void OnTriggerEnter(Collider other){
		// PLAYER 1 TAG 
		if (other.tag.Contains("Player1")) {
			if (other.transform.position.x > ReachedPointP1.x){
				ReachedPointP1 = transform.position;
				
				if (gameObject.name.Contains ("CheckPointP1 0")){
				}
				else{
					AudioSource.PlayClipAtPoint(this.checkPointClip,transform.position, 0.5f);
					// checkpoint's position = player's position
				}
			}
		}
		// PLAYER 2 TAG
		if (other.tag.Contains("Player2")) {
			if (other.transform.position.x > ReachedPointP2.x){
				ReachedPointP2 = transform.position;

				if (gameObject.name.Contains ("CheckPointP2 0")){
				}
				else{
				AudioSource.PlayClipAtPoint(this.checkPointClip,transform.position, 0.5f);
				// checkpoint's position = player's position
				}
			}
		}
		
		
	}
}
