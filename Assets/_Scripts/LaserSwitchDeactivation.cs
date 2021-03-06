﻿using UnityEngine;
using System.Collections;

public class LaserSwitchDeactivation : MonoBehaviour {

	public GameObject laser;
	public Material unlockedMat;

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
			if(Input.GetButton("Action"))
			{
				LaserDeactivation(this.laser);
			}
		}
	}

	void LaserDeactivation(GameObject laser)
	{
		laser.SetActive (false);

		Renderer switchButton = GetComponent<Renderer> ();
		switchButton.material = unlockedMat;
		audio.Play ();
	} 
}
