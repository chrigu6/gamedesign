using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public GameObject activePlayer;

	public Vector3	offset;

	public Camera frontCamera;
	public Camera aboveCamera;

	bool lane1Visible;
	bool lane2Visible;
	bool lane3Visible;

	// Use this for initialization
	void Start () {
		offset = transform.position;
		lane1Visible = true;
		lane2Visible = false;
		lane3Visible = false;
		frontCamera.enabled = true;
		aboveCamera.enabled = false;
		frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("1stLane") | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
		(frontCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = true;
		(aboveCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = false;
		activePlayer.GetComponent<PlayerController> ().changeState();
	}


	void FixedUpdate(){
		if (Input.GetKeyDown (KeyCode.Tab) && activePlayer.GetComponent<PlayerController>().changeLane == false) {
			frontCamera.enabled = !frontCamera.enabled;
			aboveCamera.enabled = !frontCamera.enabled;
			(frontCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = frontCamera.enabled;
			(aboveCamera.GetComponent(typeof(AudioListener)) as AudioListener).enabled = aboveCamera.enabled;
		}

		if (activePlayer.transform.position.z < 9){
			frontCamera.cullingMask = ((1 << LayerMask.NameToLayer("1stLane") | 0 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
			lane1Visible = true;
			lane2Visible = false;
			lane3Visible = false;
		}
		if (activePlayer.transform.position.z > 9){
			frontCamera.cullingMask = ((0 << LayerMask.NameToLayer("1stLane") | 1 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
			lane1Visible = false;
			lane2Visible = true;
			lane3Visible = false;
		}
		if (activePlayer.transform.position.z > 19){
			frontCamera.cullingMask = ((0 << LayerMask.NameToLayer("2ndLane") | 1 << LayerMask.NameToLayer("3rdLane") | 1 << LayerMask.NameToLayer(activePlayer.tag)));
			lane1Visible = false;
			lane2Visible = false;
			lane3Visible = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.C)) {
			if (activePlayer == player1)
			{
				activePlayer = player2;
				activePlayer.GetComponent<PlayerController>().changeState();
			}
			else
			{
				activePlayer = player1;
				activePlayer.GetComponent<PlayerController>().changeState();
			}
		}
		transform.position = activePlayer.transform.position + offset;
	}
}
