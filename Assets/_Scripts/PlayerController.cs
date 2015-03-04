using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpForce;
	public GameObject deathParticles;

	private Vector3 spawn;

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
		(frontCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = true;
		(aboveCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = false;
		spawn = transform.position;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKeyDown (KeyCode.Tab) && changeLane == false) {
			frontCamera.enabled = !frontCamera.enabled;
			aboveCamera.enabled = !frontCamera.enabled;
			(frontCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = frontCamera.enabled;
			(aboveCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = aboveCamera.enabled;
		}

		if (changeLane) {
			rigidbody.constraints = RigidbodyConstraints.FreezePositionX|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY ;
			float moveVertical = Input.GetAxis ("Vertical");
			Vector3 movement = new Vector3 (0, 0, moveVertical);
			
			rigidbody.AddForce (movement * speed * Time.deltaTime);

			if (transform.position.z < 9){
				frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("1stLane") | 0 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer("Player1")));
				bool lane1Visible = true;
				bool lane2Visible = false;
				bool lane3Visible = false;
			}
			if (transform.position.z > 9){
				frontCamera.cullingMask = ((0 << LayerMask.NameToLayer("1stLane") | 1 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer("Player1")));
				bool lane1Visible = false;
				bool lane2Visible = true;
				bool lane3Visible = false;
			}
			if (transform.position.z > 19){
				frontCamera.cullingMask = ((0 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer("3rdLane") | 1 << LayerMask.NameToLayer("Player1")));
				bool lane1Visible = false;
				bool lane2Visible = false;
				bool lane3Visible = true;
			}



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
		if (transform.position.y < -2) {
			Die();
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
		if (colide.gameObject.tag == "enemy") {
			Die();
		}
	}

	void OnCollisionExit(Collision colide)
	{
		if (colide.gameObject.tag == "floor") {
			grounded = false;
		}
	}

	void Die()
	{
		transform.position = spawn;
		Instantiate(deathParticles, transform.position, Quaternion.identity);
	}
}
