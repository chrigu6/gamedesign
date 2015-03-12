using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	Transform player1;
	NavMeshAgent nav;
	
	void Awake (){
		player1 = GameObject.FindGameObjectWithTag ("Player1").transform;
		nav = GetComponent <NavMeshAgent> ();
	}

	void Update () {
		nav.SetDestination (player1.position);
	}
}
