using UnityEngine;
using System.Collections;


public class LightScripts : MonoBehaviour {

	public Light spotLight;
	public float speed = 0.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = spotLight.transform.position + new Vector3 (speed, 0, 0);
		spotLight.transform.position = position;

		if (spotLight.transform.position.x > 60)
			spotLight.transform.position = new Vector3(-10,spotLight.transform.position.y,spotLight.transform.position.z);
	}
}
