using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	//General state
	bool isActive = false;
	private int currentLayer;
	public bool changeLane;
	private Animator anim;
	private HashIDs hash;

	//Movement
	Rigidbody playerRigidbody;
	public float speed;
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;

	//Jumping
	public AudioClip jumpClip;
	public AudioClip landedClip;
	public float jumpForce;
	bool grounded = true;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	//Shooting
	public bool canShoot = true;
	public GameObject shootingQuad;
	public float shootingQuadoffset;
	public AudioClip shotClip;
	public float flashIntensity = 3f;
	public float fadeSpeed = 10f;
	private bool shooting;
	private Vector3 shotPosition;
	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float range =  100f;
	float timer;
	Ray shootRay;
	Ray enemyRay;
	RaycastHit shootHit;
	int shootableMask;
	int shootingMask;
	LineRenderer gunLine;
	AudioSource gunAudio;
	Light gunLight;
	float effectsDisplayTime = 0.2f;
	public Transform rightHand;
	public Transform shotSpawn;
	public float fireRate;
	public float bulletSpeed;
	private float nextFire;

	public GameObject shot;

	//Cameras
	public Camera frontCamera;
	public Camera aboveCamera;

	//Die
	public GameObject deathParticles;
	private PlayerHealth health;
	public AudioClip deathClip;
	private Vector3 spawn;


	//Terminal
	private bool atTerminal = false;
	private bool activateTerminal = false;
	private string terminalName = "";

	//Health



	// Use this for initialization
	void Start () {
		spawn = transform.position;
		health = GetComponent<PlayerHealth> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		playerRigidbody.transform.Rotate (0, 270, 0);
	}

	void Awake() {

		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag ("GameController").GetComponent<HashIDs> ();

		shootableMask = LayerMask.GetMask ("Shootable");
		shootingMask = LayerMask.GetMask ("ShootingQuad");

		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
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
			playerRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
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
		timer += Time.deltaTime;
		//float shot = anim.GetFloat (hash.shotFloat);

		if (this.isActive) {
			this.shootingQuad.transform.position = this.transform.position +  new Vector3(0,0,shootingQuadoffset);
		}

		/*if (shot < 0.5f) {
			laserShotLine.enabled = false;
		}*/

		Collider[] ground = Physics.OverlapSphere (groundCheck.position, groundRadius, whatIsGround);
		if (ground.Length > 0) {
			if(!this.grounded)
			{
				AudioSource.PlayClipAtPoint(this.landedClip, transform.position, 0.1f);
			}
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
				AudioSource.PlayClipAtPoint(this.jumpClip, transform.position, 0.3f);
			}

			if (frontCamera.enabled && Input.GetButton ("Fire2") && Time.time > nextFire && this.canShoot) {
				//anim.SetBool(hash.shotClicked, true);
				Vector3 shotPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 playerToMouse = shotPosition - rightHand.position;

				if((transform.forward.x > 0 && playerToMouse.x > 0) || (transform.forward.x < 0 && playerToMouse.x < 0))
				{
					nextFire = Time.time + fireRate;
					this.ShotEffects();
				}
				//anim.SetBool(hash.shotClicked, false);
			}

			if(this.atTerminal && Input.GetButtonDown("Action"))
			{
				this.activateTerminal = true;
			}

		}

		if (timer >= timeBetweenBullets * effectsDisplayTime) {
			DisableEffects();
		}
		//laserShotLight.intensity = Mathf.Lerp (laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
	}

	/*void OnAnimatorIK (int layerIndex)
	{
		// Cache the current value of the AimWeight curve.
		float aimWeight = anim.GetFloat(hash.aimWeightFloat);
		
		// Set the IK position of the right hand to the player's centre.
		anim.SetIKPosition(AvatarIKGoal.RightHand, this.shotPosition);
		
		// Set the weight of the IK compared to animation to that of the curve.
		anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
	}*/

	void ShotEffects(){
		timer = 0f;
		gunLight.enabled = true;
		//gunLine.enabled = true;
		//gunLine.SetPosition (0, rightHand.position);
		shootRay.origin = rightHand.position;

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		shotPosition = Input.mousePosition;
		shotPosition = Camera.main.ScreenToWorldPoint (shotPosition);
		shotPosition.z = transform.position.z;

		AudioSource.PlayClipAtPoint (shotClip, rightHand.position, 0.3f);

		enemyRay.origin = rightHand.position;
		enemyRay.direction = shotPosition;

		if(Physics.Raycast(enemyRay, out shootHit, range, shootableMask)){
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth>();
			// if there is a enemyHealth script component, take damage and draw a line
			if(enemyHealth != null){
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
				// draw the line
			}
		}
		 

		if(Physics.Raycast(camRay, out floorHit, range, this.shootingMask))
		{
			Vector3 playerToMouse = floorHit.point - rightHand.position;
			playerToMouse.z = rightHand.position.z;
			float angle = Vector3.Angle(playerToMouse,new Vector3(1,0,0));
			if(playerToMouse.y<0)
			{
				angle = angle *-1;
			}
			//shootRay.direction = playerToMouse;
			//gunLine.SetPosition(1,shootRay.origin + shootRay.direction * range);
			//Quaternion rotation = 
			//rotation = rightHand.rotation * rotation;
			Instantiate (shot, rightHand.position, Quaternion.Euler (new Vector3(0,0,angle)));
		}


	}

	void DisableEffects()
	{
		//gunLine.enabled = false;
		gunLight.enabled = false;
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
			if (other.gameObject.tag.Contains ("terminal")) {
				this.atTerminal = true;
				this.terminalName = other.gameObject.tag;
			}

			if (other.gameObject.tag == "DialogCollider")
			{
				StartCoroutine(other.gameObject.GetComponent<dialogScript>().dialogCollision());
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (isActive) {
			if (other.gameObject.tag.Contains("terminal")) {
				this.atTerminal = false;
				this.terminalName = "";
			}
		}
	}



	void OnCollisionEnter(Collision colide)
	{
		if (isActive) {
			if (colide.gameObject.tag == "platform") {
				transform.parent = colide.transform;
			}
		}
	}

	void OnCollisionExit(Collision colide)
	{
		if (isActive) {
			if (colide.gameObject.tag == "platform") {
				transform.parent = null;
			}
		}
	}

	void Rotating(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3 (horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp (playerRigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		playerRigidbody.MoveRotation (newRotation); 
	}
//
	public void Die()
	{
//		StartCoroutine (waitOneSecond());
		// respawn at latest checkpoint
		AudioSource.PlayClipAtPoint (this.deathClip, this.transform.position, 0.5f);
		transform.position = CheckPoint.ReachedPoint + new Vector3(0,3,0);
		this.health.AddHealth (1024);
		this.health.alive();
//		Instantiate(deathParticles, transform.position, Quaternion.identity);
	}

	public void changeState(){
		this.isActive = !this.isActive;
		this.Move (0f,0f);
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

	public void abort(){
		this.activateTerminal = false;
	}

	public bool getTerminalState()
	{
		return this.activateTerminal;
	}

	public string getTerminalName()
	{
		return this.terminalName;
	}

	public bool isPlayerActive()
	{
		return this.isActive;
	}

	public void enableShooting()
	{
		this.canShoot = true;
	}

	public void disableShooting()
	{
		this.canShoot = false;
	}

	IEnumerator waitOneSecond() {
		yield return new WaitForSeconds(1);
	}


}
