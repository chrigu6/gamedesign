using UnityEngine;
using System.Collections;

public class Move3DBullet : MonoBehaviour {

	public float speed;
	public float damage = 50;
	
	private Vector3 origin;
	private float range = 100;

	private GameObject activePlayer;
	
	
	void Start ()
	{
		activePlayer = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		GetComponent<Rigidbody>().velocity = -transform.up * speed;
	}
	
	void Update()
	{
		if (this.origin != null) {
			if (Vector3.Distance (this.origin, this.transform.position) > this.range) {
				Destroy (this.gameObject);
			}
		}
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == activePlayer) {
			activePlayer.GetComponent<PlayerHealth>().TakeDamage((int)damage);
			Destroy (this.gameObject);
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
	
	public void setOrigin(Vector3 origin)
	{
		this.origin = origin;
	}
	
	public void setRange(float range)
	{
		this.range = range;
	}

}
