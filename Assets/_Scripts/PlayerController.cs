using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	Rigidbody playerRigidbody;


	public float speed;
	public float jumpForce;
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;
	public GameObject deathParticles;

	private Vector3 spawn;
	Vector3 movement;


	public Camera frontCamera;
	public Camera aboveCamera;

	bool grounded = true;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	private int currentLayer;

	public bool changeLane;

	bool isActive = false;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float bulletSpeed;
	private float nextFire;

	private Animator anim;
	private HashIDs hash;

	public AudioClip shotClip;
	public float flashIntensity = 3f;
	public float fadeSpeed = 10f;
	private LineRenderer laserShotLine;
	private Light laserShotLight;
	private bool shooting;
	private Vector3 shotPosition;


	// Use this for initialization
	void Start () {
		spawn = transform.position;
		playerRigidbody = GetComponent<Rigidbody> ();
		playerRigidbody.transform.Rotate (0, 270, 0);

	}

	void Awake() {
		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag ("GameController").GetComponent<HashIDs> ();
		laserShotLine = GetComponentInChildren<LineRenderer> ();
		laserShotLight = GetComponentInChildren<Light> ();

		laserShotLine.enabled = false;
		laserShotLight.intensity = 0f;
		this.currentLayer = 1;
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		float h = Input.GetAxisRaw ("Horizontal");
	
		float v = Input.GetAxisRaw ("Vertical");
		if (isActive) {
			Move (h, v);
		}


		if (frontCamera.enabled) {
			playerRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		} else {
			playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
		/*
		if (frontCamera.enabled)
		{
			Vector3 mousePos = Input.mousePosition;
			Vector3 lookAt = frontCamera.WorldToScreenPoint (transform.position);
			mousePos.x -= lookAt.x;
			mousePos.y -= lookAt.y;
			float rotationAngle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, rotationAngle));
		}
		*/
	}
		

	void Update()
	{
		float shot = anim.GetFloat (hash.shotFloat);

		if (shot < 0.5f) {
			laserShotLine.enabled = false;
		}

		Collider[] ground = Physics.OverlapSphere (groundCheck.position, groundRadius, whatIsGround);
		if (ground.Length > 0) {
			this.grounded = true;
			this.currentLayer = this.getLayer(ground[0].tag);
		} else {
			this.grounded = false;
		}
		if (transform.position.y < -9) {
			Die ();
		}

		if (isActive) {

			if (Input.GetButtonDown ("Jump") && this.grounded) {
				GetComponent<Rigidbody> ().AddForce (new Vector3 (0, jumpForce, 0));
			}

			if (frontCamera.enabled && Input.GetButton ("Fire2") && Time.time > nextFire) {
				anim.SetBool(hash.shotClicked, true);
				nextFire = Time.time + fireRate;
				this.ShotEffects();
				anim.SetBool(hash.shotClicked, false);
			}
		}

		laserShotLight.intensity = Mathf.Lerp (laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
	}

	void OnAnimatorIK (int layerIndex)
	{
		// Cache the current value of the AimWeight curve.
		float aimWeight = anim.GetFloat(hash.aimWeightFloat);
		
		// Set the IK position of the right hand to the player's centre.
		anim.SetIKPosition(AvatarIKGoal.RightHand, this.shotPosition);
		
		// Set the weight of the IK compared to animation to that of the curve.
		anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
	}

	void ShotEffects(){
		shotPosition = Input.mousePosition;
		shotPosition = Camera.main.ScreenToWorldPoint (shotPosition);
		shotPosition.z = transform.position.z;
		laserShotLine.SetPosition (0, laserShotLine.transform.position);
		laserShotLine.SetPosition (1, shotPosition);
		laserShotLine.enabled = true;
		laserShotLight.intensity = flashIntensity;
		AudioSource.PlayClipAtPoint (shotClip, laserShotLight.transform.position);
	}

	void Move (float h, float v)
	{
		if (h != 0f || v != 0f) {
			anim.SetFloat (hash.speedFloat, 5.5f, speedDampTime, Time.deltaTime);
			if (frontCamera.enabled) {
				Rotating (h, v);

			} else {
				Rotating (v, -h);
			}


		} else {
			anim.SetFloat(hash.speedFloat, 0f);
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

	void Rotating(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3 (horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp (playerRigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		playerRigidbody.MoveRotation (newRotation); 
	}
//
	void Die()
	{
		transform.position = spawn;
		Instantiate(deathParticles, transform.position, Quaternion.identity);
	}

	public void changeState(){
		this.isActive = !this.isActive;
	}

	private int getLayer(string LayerName){
		if (LayerName.Equals ("1stLane"))
			return 1;
		if (LayerName.Equals ("2ndLane"))
			return 2;
		if (LayerName.Equals ("3dLane"))
			return 3;

		return this.currentLayer;
	}

	public int getLayerNumber()
	{
		return this.currentLayer;
	}

}
