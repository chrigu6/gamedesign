using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpSpeed;
	public Camera frontCamera;
	public Camera aboveCamera;

	// Use this for initialization
	void Start () {
		frontCamera.enabled = true;
		aboveCamera.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKeyDown(KeyCode.Tab)){
			frontCamera.enabled = !frontCamera.enabled;
			aboveCamera.enabled = !frontCamera.enabled;
		}

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical") * jumpSpeed;

		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0f);

		rigidbody.AddForce (movement * speed * Time.deltaTime);
	}
}
