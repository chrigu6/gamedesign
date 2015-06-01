using UnityEngine;
using System.Collections;

public class GetPivot : MonoBehaviour {

	private Vector3 pivot;
	// Use this for initialization
	void Start () {
		this.pivot = GetComponent<MeshRenderer> ().bounds.center;
	}
	
	public Vector3 getPivot()
	{
		return this.pivot;
	}
}
