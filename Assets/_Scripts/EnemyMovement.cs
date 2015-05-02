using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public GameObject player1;
	Transform player1Transform;
	NavMeshAgent nav;
	bool frontCameraEnabled;
	bool isDead;
	

	void Awake (){
//		player1 = GameObject.FindGameObjectWithTag ("Player1");
		PlayerController playerController = player1.GetComponent <PlayerController> ();
		player1Transform = player1.transform;

		nav = GetComponent <NavMeshAgent> ();
	}

	void Update () {
		PlayerController playerController = player1.GetComponent <PlayerController> ();

		frontCameraEnabled = playerController.frontCamera.enabled;

		// if front camera is enabled, put the enemy on the same Z position as the player
		if (frontCameraEnabled == true){
		
			Vector3 newPosition = transform.position;
			newPosition.z = player1Transform.position.z;
			transform.position = newPosition;

		}

		EnemyHealth enemyHealth = GetComponent<EnemyHealth> ();
		isDead = enemyHealth.isDead;

		// If the enemy is not dead, set the destination to "player1.position"
		if (isDead != true) {
			nav.SetDestination (player1Transform.position);
		}
	}
}
