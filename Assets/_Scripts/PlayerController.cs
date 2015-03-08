using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpForce;
	public GameObject deathParticles;

	private Vector3 spawn;

	bool grounded = true;

	public bool changeLane;

	bool isActive;


	// Use this for initialization
	void Start () {
		spawn = transform.position;
		isActive = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (isActive) {

			if (changeLane) {
				GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
				float moveVertical = Input.GetAxis ("Vertical");
				Vector3 movement = new Vector3 (0, 0, moveVertical);
			
				GetComponent<Rigidbody> ().AddForce (movement * speed * Time.deltaTime);

			} else {
				GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
				float moveHorizontal = Input.GetAxis ("Horizontal");
			
			
				Vector3 movement = new Vector3 (moveHorizontal, 0f, 0f);
				GetComponent<Rigidbody> ().AddForce (movement * speed * Time.deltaTime);
			
			}
		}
		





	}

	void Update()
	{
		if (isActive) {
			if (Input.GetKeyDown (KeyCode.Space) && grounded) {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (0, jumpForce, 0));
			}
			if (transform.position.y < -2) {
				Die ();
			}
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (isActive) {
			if (other.gameObject.tag == "intersection" ) {
				changeLane = !changeLane;
				grounded = true;

			}
		}
	}

	void OnCollisionEnter(Collision colide)
	{
		if (isActive) {
			if (colide.gameObject.tag == "floor" || colide.gameObject.tag == "intersection") {
				grounded = true;
			}
			if (colide.gameObject.tag == "enemy") {
				Die ();
			}
		}
	}

	void OnCollisionExit(Collision colide)
	{
		if (isActive) {
			if (colide.gameObject.tag == "floor") {
				grounded = false;
			}
		}
	}

	void Die()
	{
		transform.position = spawn;
		Instantiate(deathParticles, transform.position, Quaternion.identity);
	}

	public void changeState(){
		this.isActive = !this.isActive;
	}
}
