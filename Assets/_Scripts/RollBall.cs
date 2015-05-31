using UnityEngine;
using System.Collections;

public class RollBall : MonoBehaviour {

	public Vector3 start;
	public Vector3 end;
	private bool moveBack = false;
	public float speed = 2;
	private Rigidbody body;
	private Vector3 forward = new Vector3 (5, 0, 0);
	// Use this for initialization
	void Start () {
		this.body = this.gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*if (!moveBack) {
			while (this.transform.position.x < this.end.x) {
				body.AddForce (forward * speed);
			}

			this.moveBack = true;
		} else {
			while (false) {
				body.AddForce (-forward * speed);
			}
			this.transform.position = this.start;
			this.moveBack = false;

		}*/
		if (this.transform.position.x > this.end.x) {
			this.moveBack = true;
			body.velocity = Vector3.zero;
			body.angularVelocity = Vector3.zero; 
		}

		if(this.transform.position.x < this.start.x){
			this.moveBack = false;
			body.velocity = Vector3.zero;
			body.angularVelocity = Vector3.zero; 
		}

		if (this.moveBack) {
			body.AddForce (-forward * speed);
		} else {
			body.AddForce (forward * speed);
		}
	}
}
