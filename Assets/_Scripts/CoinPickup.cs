using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {
	public int scoreValue = 10;
	public int healthValue = 10;
	public int ammoValue = 10;
	public PlayerScore playerScore;
	public PlayerHealth playerHealth;
	public PlayerAmmo playerAmmo;

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

			if(this.tag.Equals("LifeCoin")){
				Debug.Log("Hell yea lifecoin");
				playerHealth.AddHealth (healthValue);
				
				playerScore.PlayHealthSound();

			}

			if(this.tag.Equals("ScoreCoin")){
				Debug.Log("Hell yea ScoreCoin");

				playerScore.AddScore (scoreValue);
				
				playerScore.PlayScoreSound();
			}

			if(this.tag.Equals("AmmoCoin")){
				Debug.Log("Hell yea AmmoCoin");

				playerAmmo.AddAmmo (ammoValue);
				
				playerScore.PlayAmmoSound();
			}


				particleEffect.Play();

				this.GetComponent<MeshRenderer>().enabled = false;
				this.GetComponent<BoxCollider>().enabled = false;
				Destroy (gameObject, 3f);


		}
	}
}