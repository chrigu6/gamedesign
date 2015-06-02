using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScore : MonoBehaviour {
	public int startingScore;
	public int currentScore;
	
	int amount;
	bool scoreAdded = false;
	string scoreText = "Score: 0";
	
	Text guiScore;
	
	public AudioClip scoreClip;
	AudioSource scoreAudio;
	
	public AudioClip healthClip;
	AudioSource healthAudio;
	
	public AudioClip ammoClip;
	AudioSource ammoAudio;
	
	
	
	
	// Use this for initialization
	void Start () {
		currentScore = startingScore;
		guiScore = GameObject.Find("Score").GetComponent<Text> ();
		guiScore.text = scoreText;
		scoreAudio = GetComponent <AudioSource> ();
		healthAudio = GetComponent <AudioSource> ();
		ammoAudio = GetComponent <AudioSource> ();
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	
	public void AddScore(int amount)
	{
		scoreAdded = true;
		currentScore += amount;
		scoreText = "Score: " + currentScore.ToString ();
		guiScore.text = scoreText;
		
		//			playerAudio.Play();
	}
	
	public void PlayScoreSound(){
		
		scoreAudio.clip = scoreClip;
		scoreAudio.Play ();
		
	}
	
	public void PlayHealthSound(){
		
		healthAudio.clip = healthClip;
		healthAudio.Play ();
		
	}
	
	public void PlayAmmoSound(){
		
		ammoAudio.clip = ammoClip;
		ammoAudio.Play ();
		
	}
	
	
}
