using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpForce;

	bool grounded = true;

	public Camera frontCamera;
	public Camera aboveCamera;

	bool changeLane;
	bool lane1Visible = true;
	bool lane2Visible = false;
	bool lane3Visibile = false;

	// Use this for initialization
	void Start () {
		frontCamera.enabled = true;
		aboveCamera.enabled = false;
		frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("1stLane") | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Player1")));
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
		if (other.gameObject.tag == "intersection" && aboveCamera.enabled == true) {
			changeLane = !changeLane;
			grounded=true;

		}
	}

	void OnCollisionEnter(Collision colide)
	{
		if (colide.gameObject.tag == "floor" || colide.gameObject.tag == "intersection") {
			grounded = true;
		}

	}

	void OnCollisionExit(Collision colide)
	{
		if (colide.gameObject.tag == "floor") {
			grounded = false;
		}
	}
}
