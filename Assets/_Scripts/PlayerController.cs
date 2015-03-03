using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpForce;

	bool grounded = true;

	public Camera frontCamera;
	public Camera aboveCamera;

	// Use this for initialization
	void Start () {
		frontCamera.enabled = true;
		aboveCamera.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKeyDown (KeyCode.Tab)) {
			frontCamera.enabled = !frontCamera.enabled;
			aboveCamera.enabled = !frontCamera.enabled;
		}

		float moveHorizontal = Input.GetAxis ("Horizontal");


		Vector3 movement = new Vector3 (moveHorizontal, 0f, 0f);
		rigidbody.AddForce (movement * speed * Time.deltaTime);


	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			rigidbody.AddForce(new Vector3(0, jumpForce, 0));
		}
	}

	void OnCollisionEnter(Collision colide)
	{
		if (colide.gameObject.tag == "floor") {
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
