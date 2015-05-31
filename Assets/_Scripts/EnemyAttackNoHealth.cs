using UnityEngine;
using System.Collections;

public class EnemyAttackNoHealth : MonoBehaviour {

	public float timeBetweenAttacks = 0.5f;
	public int attackDamage = 10;
	
	public GameObject player;
	PlayerHealth playerHealth;

	bool playerInRange;
	float timer;
	
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player1");
		playerHealth = player.GetComponent <PlayerHealth> ();
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

		if (timer >= timeBetweenAttacks && playerInRange && player.GetComponent<PlayerController>().isPlayerActive()) {
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
