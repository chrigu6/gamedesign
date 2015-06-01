using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Ammo : MonoBehaviour {

	private PlayerController player1;
	private PlayerController player2;
	private PlayerController activePlayer;	

	private float startingAmmo;
	private bool isBlinking = false;
	private Coroutine blink;

	Text guiAmmo;
	
	void Start() {
		player1 = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController>();
		player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController>();
		startingAmmo = player1.ammo;
		float startingScore = player1.ammo;
		guiAmmo = GameObject.Find ("Ammo").GetComponent<Text> ();
		guiAmmo.text = "Ammo: " + startingAmmo;
	}

	void Update() {
		if (player1.isPlayerActive ()) {
			activePlayer = player1;
		} else {
			activePlayer = player2;
		}
		updateAmmo (activePlayer.ammo);

		if (activePlayer.ammo <= 0) {
			guiAmmo.color = new Color(1, 0, 0);
		}
	}

	void updateAmmo(float currentAmmo) {
		guiAmmo.text = "Ammo: " + currentAmmo;
	}
}
