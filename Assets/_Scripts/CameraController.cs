using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public GameObject activePlayer;
	public GameObject startArea;
	public GameObject hud;

	public Vector3	offset;

	public Camera frontCamera;
	public Camera aboveCamera;
	public Camera terminalCamera;

	public bool switchingCamAllowed;


	private int layerNumber;
	
	bool playerInStartArea;

	// Use this for initialization
	void Start () {
		player1.GetComponent<PlayerController> ().changeState();
		offset = transform.position;
		playerInStartArea = true;
		frontCamera.enabled = true;
		aboveCamera.enabled = false;
		terminalCamera.enabled = false;
		frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("1stLane") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
		(frontCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = true;
		(aboveCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = false;
		this.layerNumber = this.getLayerFromPlayer (this.activePlayer);

		// If first level, do not allow the player to switch to 3d view camera
		if (Application.loadedLevelName == "level1") {
			switchingCamAllowed = false;
		} else {
			switchingCamAllowed = true;
		}

	}


	void FixedUpdate(){
		transform.position = activePlayer.transform.position + offset;
	}

		void OnTriggerEnter (Collider other) {
		if (other.gameObject == startArea) {
			playerInStartArea = true;
		}
	}
	void OnTriggerExit (Collider other) {
		if (other.gameObject == startArea) {
			playerInStartArea = false;
		}
	}
	// Update is called once per frame
	void Update () {
		if (this.getLayerFromPlayer (this.activePlayer) == 1) {
			//Debug.Log ("1st Mask");
			frontCamera.cullingMask = frontCamera.cullingMask = ((1 << LayerMask.NameToLayer ("Default") | 1 << LayerMask.NameToLayer ("1stLane") | 0 << LayerMask.NameToLayer ("2ndLane") | 0 << LayerMask.NameToLayer ("3dLane") | 1 << LayerMask.NameToLayer (activePlayer.tag) | 1 << LayerMask.NameToLayer ("Shootable")));
		}
		if (this.getLayerFromPlayer (this.activePlayer) == 2) {
			//Debug.Log ("2nd Mask");
			frontCamera.cullingMask = frontCamera.cullingMask = ((1 << LayerMask.NameToLayer ("Default") | 0 << LayerMask.NameToLayer ("1stLane") | 1 << LayerMask.NameToLayer ("2ndLane") | 0 << LayerMask.NameToLayer ("3dLane") | 1 << LayerMask.NameToLayer (activePlayer.tag) | 1 << LayerMask.NameToLayer ("Shootable")));
		}
		if (this.getLayerFromPlayer (this.activePlayer) == 3) {
			//Debug.Log ("3d Mask");
			frontCamera.cullingMask = frontCamera.cullingMask = ((1 << LayerMask.NameToLayer ("Default") | 0 << LayerMask.NameToLayer ("1stLane") | 0 << LayerMask.NameToLayer ("2ndLane") | 1 << LayerMask.NameToLayer ("3dLane") | 1 << LayerMask.NameToLayer (activePlayer.tag) | 1 << LayerMask.NameToLayer ("Shootable")));
		}



		if (Input.GetButtonDown ("ChangeCamera") && activePlayer.GetComponent<PlayerController>().changeLane == false && switchingCamAllowed == true) {
			frontCamera.enabled = !frontCamera.enabled;
			aboveCamera.enabled = !frontCamera.enabled;
			(frontCamera.GetComponent (typeof(AudioListener)) as AudioListener).enabled = frontCamera.enabled;
			(aboveCamera.GetComponent (typeof(AudioListener)) as AudioListener).enabled = aboveCamera.enabled;
		}

		if (Input.GetButtonDown ("SwitchPlayer")) {
			player1.GetComponent<PlayerController> ().changeState ();
			player2.GetComponent<PlayerController> ().changeState ();

			if (activePlayer == player1) {
				activePlayer = player2;
			} else {
				activePlayer = player1;
			}
			//Debug.Log (activePlayer.name);
		}

		if (activePlayer.GetComponent<PlayerController> ().getTerminalActivated ()) {

			this.frontCamera.enabled = false;
			this.aboveCamera.enabled = false;
			this.terminalCamera.enabled = true;
			this.hud.GetComponent<Canvas>().enabled = false;
			this.activePlayer.GetComponent<PlayerController> ().changeState ();
		}

	}

	int getLayerFromPlayer(GameObject player)
	{
		return player.GetComponent<PlayerController> ().getLayerNumber();
	}

}
