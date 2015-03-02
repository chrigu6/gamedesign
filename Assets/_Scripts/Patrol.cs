using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
	public Transform[] patrolPoints;
	public float moveSpeed;

	private int currentPoint;


	// Use this for initialization
	void Start () {
		transform.position = patrolPoints[0].position;
		currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {

		// Set the enemy's next patrol point to 1, and moves the enemy to it (resets to 0 when currentPoint == 1)
		if (transform.position == patrolPoints[currentPoint].position)
		{
			currentPoint++;
		}
		
		if (currentPoint >= patrolPoints.Length) {
			currentPoint = 0;
		}


		transform.position = Vector3.MoveTowards (transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);
	}
}
