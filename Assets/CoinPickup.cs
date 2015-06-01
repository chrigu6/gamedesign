using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {
	public int scoreValue = 10;
	public PlayerScore playerScore;

	public ParticleSystem particleEffect;

	
	
	
	// Use this for initialization
	void Start () {
		this.GetComponent<MeshRenderer>().enabled = true;
		this.GetComponent<BoxCollider>().enabled = true;
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other){
		// check if collider's tag contains "Player"
		if (other.tag.Contains("Player")) {
			playerScore.AddScore (scoreValue);

			playerScore.PlayCoinSound();

			particleEffect.Play();

			this.GetComponent<MeshRenderer>().enabled = false;
			this.GetComponent<BoxCollider>().enabled = false;
			Destroy (gameObject, 3f);


		}
	}
}