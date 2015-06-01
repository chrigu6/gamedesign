using UnityEngine;
using System.Collections;

public class LaserSwitchDeactivation : MonoBehaviour {

	public GameObject laser;
	public Material unlockedMat;

	private GameObject player;
	private AudioSource audio;

	void Awake() 
	{
		player = GameObject.FindGameObjectWithTag ("Player1");
		audio = GetComponent<AudioSource> ();
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == player) 
		{
			if(Input.GetButton("Action"))
			{
				LaserDeactivation();
			}
		}
	}

	void LaserDeactivation()
	{
		laser.SetActive (false);

		Renderer switchButton = GetComponent<Renderer> ();
		switchButton.material = unlockedMat;
		audio.Play ();
	}
}
