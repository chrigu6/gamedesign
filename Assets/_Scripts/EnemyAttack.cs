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
//		player = GameObject.FindGameObjectWithTag("Player1");
		playerHealth = player.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent<EnemyHealth> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player) {
			playerInRange = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject == player) {
			playerInRange = false;
		}
	}

	void Update() {
		timer += Time.deltaTime;

		if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0) {
			Attack ();
		}
		Debug.Log(playerInRange);

	}

	void Attack () {
		timer = 0f;

		if (playerHealth.currentHealth > 0) {
			playerHealth.TakeDamage (attackDamage);
		}
	}
}
