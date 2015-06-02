using UnityEngine;
using System.Collections;

public class RandomShooting : MonoBehaviour {
	public float timeBetweenShots = 1f;
	public GameObject shot;
		
	public float minShot = -45;
	public float maxShot = 45;
	public float angle = 5;
	public float direction;
	public bool fixedDirection = false;

	private float difference;
	private Transform shotSpawn;
	private float timer;
	
	void Awake() {
		shotSpawn = transform.Find ("ShotSpawn");
		difference = maxShot - minShot;
		if (!fixedDirection) {
			direction = minShot;
		}
	}


	void Update () {
		timer += Time.deltaTime;

		if (timer >= timeBetweenShots) {
			Shoot ();
		}
	}

	void Shoot() {
		timer = 0f;
		if (!fixedDirection) {
			direction = (direction + angle);
			if (direction >= maxShot) {
				direction = minShot;
			}
		}
		shotSpawn.rotation = Quaternion.Euler (0, 0, direction);
		GameObject o = Instantiate (shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
		o.GetComponent<Move3DBullet> ().setRange (100);
		o.GetComponent<Move3DBullet> ().setOrigin (this.transform.position);
	}
}
