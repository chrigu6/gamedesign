using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScore : MonoBehaviour {
	public int startingScore;
	int currentScore;
	
	int amount;
	bool scoreAdded = false;
	string scoreText = "Score: 0";
	
	Text guiScore;
	
	
	
	// Use this for initialization
	void Start () {
		currentScore = startingScore;
		guiScore = GameObject.Find("Score").GetComponent<Text> ();
		guiScore.text = scoreText;
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
	
	
}
