using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	public float timeBetweenAttacks = 0.5f;
	public int attackDamage = 10;
	
	public GameObject player;
	PlayerHealth playerHealth;
	EnemyHealth enemyHealth;

	bool playerInRange;
	float timer;
	
	void Awake () {
		player = GameObject.Find("Cameras").GetComponent<CameraController>().activePlayer;
		playerHealth = player.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent<EnemyHealth> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player1"){
			playerInRange = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Player1"){
			playerInRange = false;
		}
	}

	void Update() {
		timer += Time.deltaTime;

		if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0 && player.GetComponent<PlayerController>().isPlayerActive()) {
			Attack ();
		}
	}

	void Attack () {
		timer = 0f;

		if (playerHealth.currentHealth > 0) {
			playerHealth.TakeDamage (attackDamage);
		}
	}
}
