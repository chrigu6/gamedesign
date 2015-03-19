using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float jumpForce;
	public GameObject deathParticles;

	private Vector3 spawn;

	public Camera frontCamera;
	public Camera aboveCamera;

	bool grounded = true;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	public bool changeLane;

	bool isActive;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float bulletSpeed;
	private float nextFire;


	// Use this for initialization
	void Start () {
		spawn = transform.position;
		isActive = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Collider[] ground = Physics.OverlapSphere (groundCheck.position, groundRadius, whatIsGround);
		if (ground.Length > 0) {
			grounded = true;
		} else {
			grounded = false;
		}
		

		if (isActive) {
			float moveVertical = Input.GetAxis ("Vertical");
			float moveHorizontal = Input.GetAxis ("Horizontal");
			Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
			GetComponent<Rigidbody> ().AddForce (movement * speed * Time.deltaTime);
			if (frontCamera.enabled)
			{
				Vector3 mousePos = Input.mousePosition;
				Vector3 lookAt = Camera.current.WorldToScreenPoint (transform.position);
				mousePos.x -= lookAt.x;
				mousePos.y -= lookAt.y + 135;
				float rotationAngle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rotationAngle));
			}
		}
//		if (transform.position.y < -2) {
//			Die ();
//		}
	}
		

	void Update()
	{
		if (isActive) {
			if (Input.GetButtonDown("Jump") && grounded) {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (0, jumpForce, 0));
			}
		}
		if (Input.GetButton("Fire2") && Time.time > nextFire)
		{
			Vector3 shootDirection;
			shootDirection = Input.mousePosition;
			shootDirection.x = 0.0f;
			shootDirection = Camera.current.ScreenToWorldPoint(shootDirection) - transform.position;
			
			nextFire = Time.time + fireRate;
			Rigidbody2D bullet = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as Rigidbody2D;
			bullet.velocity = new Vector2(shootDirection.x * bulletSpeed, shootDirection.y * bulletSpeed );
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (isActive) {
			if (other.gameObject.tag == "intersection" ) {
				changeLane = !changeLane;
			}
		}
	}

//	void OnCollisionEnter(Collision colide)
//	{
//		if (isActive) {
//			if (colide.gameObject.tag == "enemy") {
//				Die ();
//			}
//		}
//	}
//
//	void Die()
//	{
//		transform.position = spawn;
//		Instantiate(deathParticles, transform.position, Quaternion.identity);
//	}
//
	public void changeState(){
		this.isActive = !this.isActive;
	}
}
