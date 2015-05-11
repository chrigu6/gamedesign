using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth;
    public int maxHealth = 1024;
    public float currentHealth;
    public Image healthSlider;
	Text guiHealth;
	Light frontSpotlight;

    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    AudioSource playerAudio;
    PlayerController playerController;
    bool healed;
    bool damaged;
    bool isDead;


    private float startFlash;

    void Awake()
    {
		playerAudio = GetComponent<AudioSource> ();
		frontSpotlight = GameObject.Find ("2dCamera").GetComponentInChildren<Light> ();
		guiHealth = GameObject.Find("CurrentHealth").GetComponent<Text> ();
        playerController = GetComponent<PlayerController>();
        currentHealth = startingHealth;
    }

    void Update()
    {
		guiHealth.text = currentHealth + " MB";
		frontSpotlight.spotAngle = currentHealth / 18;
        // flash screen when damage is taken by player
        if (damaged)
        {
            //damageImage.color = flashColour;
        }
        else
        {
           // damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    // Player regains health
    public void AddHealth(int amount)
    {
        if (currentHealth < maxHealth)
        {
            healed = true;
            currentHealth += amount;
            healthSlider.fillAmount = currentHealth / maxHealth;
            playerAudio.Play();
        }
    }

    // Player takes damage
    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.fillAmount = currentHealth / maxHealth;

        playerAudio.Play();

        // Player is dead
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }

    }


    void Death()
    {
        isDead = true;

        playerAudio.clip = deathClip;
        playerAudio.Play();

    }
}