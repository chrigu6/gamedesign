using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpForce;

	bool grounded = true;

	public Camera frontCamera;
	public Camera aboveCamera;

	bool changeLane;

	// Use this for initialization
	void Start () {
		frontCamera.enabled = true;
		aboveCamera.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKeyDown (KeyCode.Tab) && changeLane == false) {
			frontCamera.enabled = !frontCamera.enabled;
			aboveCamera.enabled = !frontCamera.enabled;
		}

		if (changeLane) {
			rigidbody.constraints = RigidbodyConstraints.FreezePositionX|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY ;
			float moveVertical = Input.GetAxis ("Vertical");
			Vector3 movement = new Vector3 (0, 0, moveVertical);
			
			rigidbody.AddForce (movement * speed * Time.deltaTime);
		} else {
			rigidbody.constraints = RigidbodyConstraints.FreezePositionZ|RigidbodyConstraints.FreezeRotationZ;
			float moveHorizontal = Input.GetAxis ("Horizontal");
			
			
			Vector3 movement = new Vector3 (moveHorizontal, 0f, 0f);
			rigidbody.AddForce (movement * speed * Time.deltaTime);
			
		}
		





	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			rigidbody.AddForce(new Vector3(0, jumpForce, 0));
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "enemy") {
			Destroy (other.gameObject);
		}
	}

	void OnCollisionEnter(Collision colide)
	{
		if (colide.gameObject.tag == "floor" || colide.gameObject.tag == "intersection") {
			grounded = true;
		}

		if (colide.gameObject.tag == "intersection" && aboveCamera.enabled == true) {
			changeLane = !changeLane;
		}
	}

	void OnCollisionExit(Collision colide)
	{
		if (colide.gameObject.tag == "floor" || colide.gameObject.tag == "intersection") {
			grounded = false;
		}
	}
}
