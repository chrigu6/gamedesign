using UnityEngine;
using System.Collections;

public class RotateSegment : MonoBehaviour {
	public float angle = 0.1f;
	private Vector3 pivot;
	public bool pivotInChild = true;
	// Use this for initialization
	void Start () {
		if (pivotInChild) {
			this.pivot = GetComponentInChildren<GetPivot> ().getPivot ();
		}
		else
		{
			this.pivot = this.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround (this.pivot, Vector3.up, this.angle);
	}
}
