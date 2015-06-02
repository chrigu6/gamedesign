using UnityEngine;
using System.Collections;

public class LaserPlayerDetection : MonoBehaviour 
{
	GameObject player;
	Vector3 lastPlayerSighting;
	Renderer renderer;

	void Avake()
	{
		player = GameObject.Find ("Cameras").GetComponent<CameraController> ().activePlayer;
		renderer = GetComponent<Renderer> ();
	}

	void OnTriggerStay(Collider other)
	{
		if (renderer.enabled) 
		{
			if(other.gameObject == player)
			{
				lastPlayerSighting = other.transform.position;
			}
		}
	}
}
