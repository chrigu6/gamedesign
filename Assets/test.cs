using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	public float speed = 6f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;

	void Awake()
	{
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v);
	}

	void Move(float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}
}
