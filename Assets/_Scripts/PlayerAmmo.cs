using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerAmmo : MonoBehaviour {

	public float startingAmmo = 10;
	public float currentAmmo;

	bool recharged;

	float maxAmmo = 100;

	private bool isBlinking = false;
	private Coroutine blink;

	Text guiAmmo;
	
	void Start() {
		currentAmmo = startingAmmo;
		guiAmmo = GameObject.Find ("CurrentAmmo").GetComponent<Text> ();
		guiAmmo.text = "";
	}

	void Update() {
		updateAmmo (currentAmmo);

		if (currentAmmo <= 0) {
			guiAmmo.color = new Color (1, 0, 0);
		} else {
			guiAmmo.color = new Color (1, 1, 1);
		}

	}

	public void reduceAmmo(float amount) {
		currentAmmo -= amount;
	}

	public void updateAmmo(float currentAmmo) {
		guiAmmo.text = "Ammo: " + currentAmmo;
	}


	public void AddAmmo(int ammoAmount)
	{
		if (currentAmmo < maxAmmo)
		{
			recharged = true;
			currentAmmo += ammoAmount;
//			ammoSlider.fillAmount = ammoAmount;
//			playerAudio.Play();
		}
	}
}
