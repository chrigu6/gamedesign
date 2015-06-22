using UnityEngine;
using System.Collections;

public class ComplexLaserSwitch : MonoBehaviour {

	public Material activatedMat;
	public Material deactivatedMat;
	public GameObject[] laser;
	private bool activated = false;
	private GameObject player;
	private AudioSource audio;
	
	void Awake() 
	{
		player = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		audio = GetComponent<AudioSource> ();
	}
	
	void OnTriggerStay(Collider other)
	{
		player = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		if (other.gameObject == player) 
		{
			if(Input.GetButtonDown("Action"))
			{
				this.activated = !this.activated;
				LaserDeactivation();
			}
		}
	}
	
	void LaserDeactivation()
	{
		Renderer switchButton = GetComponent<Renderer> ();
		if (this.activated) {
			switchButton.material = activatedMat;
		} else {
				switchButton.material = deactivatedMat;

		}
		foreach (GameObject o in this.laser) {
		o.SetActive (!o.activeSelf);
	}
		audio.Play ();
	} 
}