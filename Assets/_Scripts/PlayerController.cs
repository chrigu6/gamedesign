﻿using UnityEngine;
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

	bool facingRight = true;
	bool facingCamera = true;
	bool grounded = true;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	public bool changeLane;

	bool isActive = false;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float bulletSpeed;
	private float nextFire;

	private Animator anim;
	private HashIDs hash;


	// Use this for initialization
	void Start () {
		spawn = transform.position;
		playerRigidbody = GetComponent<Rigidbody> ();
		playerRigidbody.transform.Rotate (0, 270, 0);

	}

	void Awake() {
		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag ("GameController").GetComponent<HashIDs> ();
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		Debug.Log (this.name + " : " + this.isActive);
		float h = Input.GetAxisRaw ("Horizontal");
	
		float v = Input.GetAxisRaw("Vertical");
		if (isActive) {
			Move (h, v);
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
		Collider[] ground = Physics.OverlapSphere (groundCheck.position, groundRadius, whatIsGround);
		if (ground.Length > 0) {
			this.grounded = true;
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

			/*if (frontCamera.enabled && Input.GetButton ("Fire2") && Time.time > nextFire) {
				Vector3 shootDirection = Input.mousePosition;
				shootDirection.y = 0.0f;
				shootDirection = Camera.current.ScreenToWorldPoint (shootDirection) - transform.position;
				
				nextFire = Time.time + fireRate;
				Rigidbody2D bullet = Instantiate (shot, shotSpawn.position, shotSpawn.rotation) as Rigidbody2D;
				bullet.velocity = new Vector2 (shootDirection.x * bulletSpeed, shootDirection.y * bulletSpeed);
			}*/
		}
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
		Debug.Log ("Activated :" + this.name);
	}

	public void FlipHorizontaly()
	{

	}
}
