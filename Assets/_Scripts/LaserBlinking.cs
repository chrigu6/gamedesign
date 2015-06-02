using UnityEngine;
using System.Collections;

public class LaserBlinking : MonoBehaviour {
	public float onTime;
	public float offTime;
	public int attackDamage = 50;
	public float timeBetweenAttacks = 0.5f;

	private float damageTimer;
	private float timer;
	private bool playerInRange;

	private Renderer renderer;
	private GameObject player;
	private PlayerHealth playerHealth;
	private Light light;


	void Awake()
	{
		renderer = GetComponent<Renderer> ();
		player = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		light = GetComponent<Light> ();
	}

	void Update()
	{
		player = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		timer += Time.deltaTime;
		damageTimer += Time.deltaTime;

		if(renderer.enabled && timer >= onTime)
		{
			SwitchBeam();
		}

		if (!renderer.enabled && timer >= offTime) 
		{
			SwitchBeam();
		}

		if(renderer.enabled && playerInRange && damageTimer >= timeBetweenAttacks)
		{
			Damage ();
		}
	}

	void SwitchBeam()
	{
		timer = 0f;
		renderer.enabled = !renderer.enabled;
		light.enabled = !light.enabled;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
		{
			playerHealth = player.GetComponent<PlayerHealth>();
			playerInRange = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player) 
		{
			playerInRange = false;	
		}
	}
	
	void Damage()
	{
		damageTimer = 0f;

		if (playerHealth.currentHealth > 0) 
		{
			playerHealth.TakeDamage(attackDamage);
		}
	}
}
