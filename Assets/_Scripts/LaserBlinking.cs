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
	private GameObject player1;
	private GameObject player2;
	private PlayerHealth playerHealth;
	private Light light;


	void Awake()
	{
		renderer = GetComponent<Renderer> ();
		player1 = GameObject.FindGameObjectWithTag ("Player1");
		player2 = GameObject.FindGameObjectWithTag ("Player2");
		light = GetComponent<Light> ();
	}

	void Update()
	{
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
		if(other.gameObject == player1 || other.gameObject == player2)
		{
			if(other.gameObject == player1){
				playerHealth = player1.GetComponent<PlayerHealth>();
			}
			else {
				playerHealth = player2.GetComponent<PlayerHealth>();
			}
			playerInRange = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player1 || other.gameObject == player2) 
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
