using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;
	public AudioClip deathClip;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

	AudioSource playerAudio;
	PlayerController playerController;
	bool isDead;
	bool damaged;

	void Awake () {
		playerAudio = GetComponent <AudioSource> ();
		playerController = GetComponent <PlayerController> ();
		currentHealth = startingHealth;
	}
	
	void Update () {
		// flash screen when damage is taken by player
		if (damaged) {
			damageImage.color = flashColour;
		}
		else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
	}

	public void TakeDamage (int amount){
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;
		playerAudio.Play ();

		if (currentHealth <= 0 && !isDead) {
			Death();
		}
	
	}

	void Death(){
		isDead = true;

		playerAudio.clip = deathClip;
		playerAudio.Play ();

	}
}