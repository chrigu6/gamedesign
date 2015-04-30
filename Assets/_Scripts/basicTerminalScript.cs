using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class basicTerminalScript : MonoBehaviour {
	
	private bool isWriting = false;
	private bool cursorVisible = false;
	
	public float cursorSpeed;
	public float letterPause;
	
	public AudioClip sound;
	
	public string fileName;
	private char[] chars;
	private int i = 0;
	private int lines = 0;
	private int charPerLines = 0;
	private AudioSource terminalAudio;
	private bool active = false;
	private bool abort = false;
	private string defaultText ="  **** COMMODORE 64 BASIC V2 ****\n        64K RAM SYSTEM\n	    38911 BASIC BYTES FREE\n\nREADY\n";
	
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Text> ().text = this.defaultText;
		string message = File.ReadAllText (Application.dataPath+ "/" + fileName).ToUpper();
		chars = message.ToCharArray();
		terminalAudio = gameObject.GetComponent<AudioSource> ();
		terminalAudio.mute = true;
		this.gameObject.GetComponentInParent<Canvas> ().enabled = false;
	}
	
	void OnGUI() {
		//Debug.Log ("Hans: " + active);
		if (active) {;
			if(Input.GetButtonDown ("SwitchPlayer"))
			{
				this.abortTerminal();
			}
			//If the end of the message is not reached and not allready writing startwriting
			if (!isWriting && (i < chars.Length - 1)) {
				isWriting = true;
				cursorVisible = false;
				StartCoroutine (TypeText ());
				StopCoroutine (ShowCursor ());
			} else {
				if (!cursorVisible && (i > chars.Length - 1)) {
					cursorVisible = true;
					StopCoroutine (TypeText ());
					StartCoroutine (ShowCursor ());
				}
			}
		}
	}
	
	IEnumerator TypeText ()
	{
		foreach (char letter in chars) {
			
			i++;
			this.gameObject.GetComponent<Text> ().text += letter;
			charPerLines++;
			char currentLetter = letter;

			if (sound && !terminalAudio.isPlaying)
			{
				terminalAudio.clip = sound;
				terminalAudio.mute = false;
				terminalAudio.Play ();
			}


			//If the textline is to long, start a new one
			if(charPerLines%35 == 0 && !(i ==0))
			{
				this.gameObject.GetComponent<Text> ().text += '\n';
				currentLetter = '\n';
			}
			
			//Count the number of lines if the number of lines reach the bottom of the textfield, start to wrap the lines
			if (currentLetter == '\n')
			{
				charPerLines = 0;
				lines++;
				if (lines > 5)
				{
					wrapLines();
				}
			}
			
			
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}
	}
	
	void wrapLines ()
	{
		string a = this.gameObject.GetComponent<Text> ().text;
		int i = a.IndexOf ("\n");
		this.gameObject.GetComponent<Text> ().text = a.Substring(i+1);
		
	}
	
	IEnumerator ShowCursor() {
		while (true) {
			string a = this.gameObject.GetComponent<Text> ().text;
			char lastChar = a.ToCharArray () [a.Length-1];
			
			if (lastChar == '▇') {
				this.gameObject.GetComponent<Text> ().text = a.Substring (0, a.Length - 1);
			} else {
				this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text + "▇";
			}
			
			yield return 0;
			yield return new WaitForSeconds (cursorSpeed);
		}
	}

	public void changeState()
	{
		//Debug.Log ("change");
		this.gameObject.GetComponentInParent<Canvas> ().enabled = true;
		this.active = !this.active;
		this.abort = false;
	}

	public bool getAbort()
	{
		return this.abort;
	}

	public bool isActive()
	{
		return this.active;
	}

	void abortTerminal ()
	{
		this.Start ();
		this.terminalAudio.mute = true;
		this.abort = true;
		this.i = 0;
	    this.lines = 0;;
		StopAllCoroutines ();
		this.isWriting = false;
	}
}