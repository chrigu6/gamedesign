using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	Transform player1;
	GameObject enemy;
	NavMeshAgent nav;
	bool isDead;

	
	void Awake (){
		player1 = GameObject.FindGameObjectWithTag ("Player1").transform;
		nav = GetComponent <NavMeshAgent> ();
	}

	void Update () {
		EnemyHealth enemyHealth = GetComponent<EnemyHealth> ();
		isDead = enemyHealth.isDead;

		if (isDead != true) {
			nav.SetDestination (player1.position);
		}
	}
}
