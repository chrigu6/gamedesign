using UnityEngine;
using System.Collections;

public class MountedGunHealth : MonoBehaviour {

	public float health = 100;

	private float currentHealth;
	private Mover bullet;

	void Awake() {
		currentHealth = health;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "FriendlyBullet") {
			bullet = other.GetComponent<Mover>();
			TakeDamage(bullet.damage);
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
