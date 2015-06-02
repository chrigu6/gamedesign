using UnityEngine;
using System.Collections;

public class MountedGunHealth : MonoBehaviour {

	public float health = 100;

	private float currentHealth;
	private Mover bullet;

	private PlayerController player;

	void Awake() {
		currentHealth = health;
		player = GameObject.Find ("Cameras").GetComponent<CameraController>().activePlayer.GetComponent<PlayerController> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "FriendlyBullet") {
			bullet = other.GetComponent<Mover>();
			TakeDamage(player.damagePerShot);
			Destroy(bullet.gameObject);
		}
	}

	void TakeDamage(float amount) {
		currentHealth -= amount;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	void Die() {
		Destroy (this.gameObject);
	}
}
