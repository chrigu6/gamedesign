using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour 
{
	public float speed;
	private Vector3 origin;
	private float range = 0;


	void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.right * speed;
	}

	void Update()
	{
		if (this.origin != null) {
				if (Vector3.Distance (this.origin, this.transform.position) > this.range) {
					Destroy (this.gameObject);
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

	public void setOrigin(Vector3 origin)
	{
		this.origin = origin;
	}

	public void setRange(float range)
	{
		this.range = range;
	}



}
