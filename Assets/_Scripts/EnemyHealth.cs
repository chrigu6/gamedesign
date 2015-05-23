﻿using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public int startingHealth;
	public float currentHealth;
	public float sinkSpeed = 5.5f;
	public int scoreValue = 10;
	public AudioClip deathClip;
	
	AudioSource enemyAudio;
	ParticleSystem hitParticles;
	SphereCollider sphereCollider;
	PlayerScore playerScore;
	
	public bool isDead;
	bool isSinking;
	
	void Awake (){
		enemyAudio = GetComponent <AudioSource> ();
		hitParticles = GetComponent <ParticleSystem> ();
		sphereCollider = GetComponent <SphereCollider> ();

		playerScore = GameObject.Find("Player1").GetComponent<PlayerScore> ();
		
		currentHealth = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (isSinking) {
			transform.Translate (Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}
	
	// Enemy takes damage
	public void TakeDamage (int amount, Vector3 hitPoint){
		if (isDead)
			return;
		enemyAudio.Play ();
		
		currentHealth -= amount;
		
		// Move the particles to the hitposition (where enemy was hit)
		hitParticles.transform.position = hitPoint;
		hitParticles.Play ();
		
		// Player is dead
		if (currentHealth <= 0 && !isDead) {
			Death ();
		}
		
	}
	
	void Death(){
		isDead = true;
		
		sphereCollider.isTrigger = true;
		
		//playerScore.AddScore (scoreValue);
		
		enemyAudio.clip = deathClip;
		enemyAudio.Play ();
		
		StartSinking ();
	}
	
	public void StartSinking (){
		// turn off navmesh component only
		GetComponent <NavMeshAgent> ().enabled = false;
		GetComponent <Rigidbody> ().isKinematic = true;
		
		isSinking = true;
		// Destroy the object after 2 seconds
		Destroy (gameObject, 10f);
	}
}
