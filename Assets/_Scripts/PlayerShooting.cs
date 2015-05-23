using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	public float range = 100f;
	public float timeBetweenBullets = 0.15f;
	public Transform rightHand;

	int shootingMask;
	int shootableMask;
	float timer;
	Ray shootRay;
	Ray camRay;
	RaycastHit shootHit;
	LineRenderer gunLine;
	Light gunLight;
	AudioSource gunAudio;
	float effectsDisplayTime = 0.2f;

	void Awake()
	{
		gunLine = GetComponent<LineRenderer> ();
		gunLight = GetComponent<Light> ();
		gunAudio = GetComponent<AudioSource> ();
		shootingMask = LayerMask.GetMask ("ShootingMask");
		shootableMask = LayerMask.GetMask ("Shootable");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<PlayerController> ().isPlayerActive ()) {
			timer += Time.deltaTime;
			if(Input.GetButton ("Fire1"))
			{
				Fire();
			}
		}

		if (timer >= timeBetweenBullets * effectsDisplayTime) {
			DisableEffects();
		}

	}

	public void DisableEffects()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;
	}

	void Fire()
	{
		timer = 0f;
		gunLight.enabled = true;
		gunLine.enabled = true;
		gunLine.SetPosition (0, rightHand.position);
		shootRay.origin = rightHand.position;

		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;
		
		if(Physics.Raycast(camRay, out floorHit, range, shootableMask))
		{
			Vector3 playerToMouse = floorHit.point - rightHand.position;
			shootRay.direction = playerToMouse;
			gunLine.SetPosition(1,shootRay.origin + shootRay.direction * range);
		}


	}
}
