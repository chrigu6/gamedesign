using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerAmmo : MonoBehaviour {

	private PlayerController player1;
	private PlayerController player2;
	private PlayerController activePlayer;	

	private float startingAmmo;
	public float currentAmmo;
	bool recharged;

	float maxAmmo = 100;

	private bool isBlinking = false;
	private Coroutine blink;

	Text guiAmmo;
	
	void Start() {
		player1 = GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerController>();

		if (GameObject.Find("Player2") != null)
		{
			player2 = GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerController>();
		}

		startingAmmo = player1.ammo;
		float startingScore = player1.ammo;
		currentAmmo = startingAmmo;
		guiAmmo = GameObject.Find ("CurrentAmmo").GetComponent<Text> ();
		guiAmmo.text = startingAmmo + "";
	}

	void Update() {
		if (player1.isPlayerActive ()) {
			activePlayer = player1;
		} else {
			activePlayer = player2;
		}

		// IMPORTANT !!! parameter should be "activePlayer", but there is a bug when the "dialogScript" appears:
		// active player becomes "null"
		updateAmmo (currentAmmo);
//		guiAmmo.text = currentAmmo + "";


		if (player1.ammo <= 0) {
			guiAmmo.color = new Color(1, 0, 0);
		}

	}

	public void updateAmmo(float currentAmmo) {
		guiAmmo.text = currentAmmo + "";
	}


	public void AddAmmo(int ammoAmount)
	{
		if (currentAmmo < maxAmmo)
		{
			recharged = true;
			currentAmmo += ammoAmount;

			Debug.Log(currentAmmo);
//			ammoSlider.fillAmount = ammoAmount;
//			playerAudio.Play();
		}
	}
}
