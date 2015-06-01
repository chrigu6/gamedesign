using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour 
{
	public float speed;
	private GameObject player1;
	private GameObject player2;
	float range = 0;


	void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.right * speed;
		player1 = GameObject.FindGameObjectWithTag ("Player1");
		player2 = GameObject.FindGameObjectWithTag ("Player2");
		if (player1 != null) {
			range = player1.GetComponent<PlayerController>().range;
		}
		if (range == 0 && player2 != null) {
			range = player2.GetComponent<PlayerController>().range;
		}
	}

	void Update()
	{
		if (player1 != null) {
			if (player1.GetComponent<PlayerController> ().isPlayerActive ()) {
				if (Vector3.Distance (player1.transform.position, this.transform.position) > this.range) {
					Destroy (this.gameObject);
				}
			}
		}

		if (player2 != null) {
			if (player2.GetComponent<PlayerController> ().isPlayerActive ()) {
				if (Vector3.Distance (player2.transform.position, this.transform.position) > this.range) {
					Destroy (this.gameObject);
				}
			}
		}
	}


	void OnCollisionEnter(Collision collide)
	{
		if (!collide.gameObject.name.Contains ("Player")) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisioneExit(Collision collide)
	{
		if (!collide.gameObject.name.Contains ("Player")) {
			Destroy (this.gameObject);
		}
	}


}
