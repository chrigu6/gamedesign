using UnityEngine;
using System.Collections;

public class RotateSegment : MonoBehaviour {
	public float angle = 0.1f;
	public Vector3 pivot;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround (this.pivot, Vector3.up, this.angle);
	}
}
