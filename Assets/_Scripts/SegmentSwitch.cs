using UnityEngine;
using System.Collections;

public class SegmentSwitch : MonoBehaviour {

	public Material activatedMat;
	public Material deactivatedMat;
	public Transform objectToMove;
	private Vector3 startPoint;
	public Vector3 offSet;
	private Vector3 endPoint;
	public bool canMoveTwice;
	public float speed;

	private int running = 0;
	private bool activated = false;
	private GameObject player;
	private AudioSource audio;


	void Awake() 
	{
		this.startPoint = this.objectToMove.position;
		this.endPoint = this.startPoint + offSet;
		player = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		audio = GetComponent<AudioSource> ();
	}

	void OnTriggerStay(Collider other)
	{
		player = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		if (other.gameObject == player) 
		{
			if(Input.GetButton("Action") && (!this.activated || this.canMoveTwice) && running < 1)
			{
				this.running++;
				activated = !activated;
				StartCoroutine(moveSegment(speed));
				Renderer switchButton = GetComponent<Renderer> ();
				if(activated)
				{
					switchButton.material = activatedMat;
				}
				else{
					switchButton.material = deactivatedMat;
				}
				audio.Play ();
			}
		}
	}

	IEnumerator moveSegment(float time)
	{
		float i = 0.0f;
		float rate = 1.0f / time;

		if (!canMoveTwice) {
			while (i < 1.0f) {
				i += Time.deltaTime * rate;
				this.objectToMove.transform.localPosition = Vector3.Lerp(this.startPoint, this.endPoint, i);
				this.running--;
				yield return null;
			}
		} else {
			Vector3 currentPosition = this.objectToMove.position;
			if(activated){
				while (i < 1.0f) {
					i += Time.deltaTime * rate;
					this.objectToMove.transform.localPosition = Vector3.Lerp(this.startPoint, this.endPoint, i);
					this.running--;
					yield return null;
				}
			}
			else{
				while (i < 1.0f) {
					i += Time.deltaTime * rate;
					this.objectToMove.localPosition = Vector3.Lerp(this.endPoint, this.startPoint, i);
					this.running--;
					yield return null;
				}
			}
		}
	}
}
