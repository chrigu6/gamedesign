﻿using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {

	public GameObject player;
	public GameObject door;
	public GameObject lamp;


	AudioSource playerAudio;


	bool playerInRange;
	float timer;


	void Awake () {
		playerAudio = GetComponent <AudioSource> ();
		playerInRange = false;
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player) {
			playerInRange = true;
			// Rotate the lamp, wait 2 secs, then open the door
			lamp.GetComponent<Animation>().Play("RotateLamp");

			if (other.gameObject.tag.Contains("Player2")){
			StartCoroutine(Wait(2.5f));
			}

			if (other.gameObject.tag.Contains("Player1")){
			StartCoroutine(Wait1(2.5f));
			}

			playerAudio.Play ();

		}
	}

	public IEnumerator Wait(float delayInSecs)
	{
		yield return new WaitForSeconds(delayInSecs);
		door.GetComponent<Animation>().Play("DoorOpen");

	}

	public IEnumerator Wait1(float delayInSecs)
	{
		yield return new WaitForSeconds(delayInSecs);
		door.GetComponent<Animation>().Play("DoorOpen1");
		
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
