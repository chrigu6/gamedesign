using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int startingHealth = 100;
	public float currentHealth;
	public Image healthSlider;
	public Image damageImage;
	public AudioClip deathClip;
	public float flashSpeed = .5f;
	public Color flashColour = new Color(1f, 0f, 0f, 255f);

	AudioSource playerAudio;
	PlayerController playerController;
	bool isDead;
	bool damaged;

	private float startFlash;

	void Awake () {
		playerAudio = GetComponent <AudioSource> ();
		playerController = GetComponent <PlayerController> ();
		currentHealth = startingHealth;
	}
	
	void Update () {
		// flash screen when damage is taken by player
		if (damaged) {
			this.startFlash = Time.time;
			damageImage.enabled = true;
			damageImage.color = flashColour;

		}

		if (Time.time - this.startFlash > flashSpeed)
		{
			damageImage.color = new Color(0f, 0f, 0f, 0f);
		}

	}

	public void TakeDamage (int amount){
		damaged = true;
		currentHealth -= amount;
		healthSlider.fillAmount = currentHealth/100;
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