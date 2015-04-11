using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public GameObject activePlayer;
	public GameObject startArea;

	public Vector3	offset;

	public Camera frontCamera;
	public Camera aboveCamera;

	bool lane1Visible;
	bool lane2Visible;
	bool lane3Visible;
	
	bool playerInStartArea;

	// Use this for initialization
	void Start () {
		Debug.Log ("Start!");
		player1.GetComponent<PlayerController> ().changeState();
		offset = transform.position;
		lane1Visible = true;
		lane2Visible = false;
		lane3Visible = false;
		playerInStartArea = true;
		frontCamera.enabled = true;
		aboveCamera.enabled = false;
		frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("1stLane") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
		(frontCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = true;
		(aboveCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = false;
	}


	void FixedUpdate(){
		if (activePlayer.transform.position.z < 17){
			frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("1stLane") | 0 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer(activePlayer.tag) | 1 << LayerMask.NameToLayer("Shootable")));
			lane1Visible = true;
			lane2Visible = false;
			lane3Visible = false;
		}
		if (activePlayer.transform.position.z > 17){
			frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("Default") | 0 << LayerMask.NameToLayer("1stLane") | 1 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
			lane1Visible = false;
			lane2Visible = true;
			lane3Visible = false;
		}
		if (activePlayer.transform.position.z > 24){
			frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("Default") | 0 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer("3rdLane") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
			lane1Visible = false;
			lane2Visible = false;
			lane3Visible = true;
		}

//		if (activePlayer.transform.position 10) {
//			transform.position = Vector3(-3.32, 1, -32);
//		}

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
		if (Input.GetButtonDown ("ChangeCamera") && activePlayer.GetComponent<PlayerController>().changeLane == false) {
			frontCamera.enabled = !frontCamera.enabled;
			aboveCamera.enabled = !frontCamera.enabled;
			(frontCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = frontCamera.enabled;
			(aboveCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = aboveCamera.enabled;
		}

		if (Input.GetButtonDown("SwitchPlayer")) {
			player1.GetComponent<PlayerController>().changeState();
			player2.GetComponent<PlayerController>().changeState();

			if (activePlayer == player1)
			{
				activePlayer = player2;
			}
			else
			{
				activePlayer = player1;
			}
			Debug.Log (activePlayer.name);
		}

	}

}
