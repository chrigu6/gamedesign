using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class test : MonoBehaviour {
	
	
	private bool displayLabel = false;
	private bool displayCursor = false;
	public float letterPause = 0.2f;
	public float cursorPause = 0.5f;
	public AudioClip sound;
	public InputField inputText;
	
	public string fileName;
	private char[] chars;
	private int i = 0;
	private int lines = 0;
	private AudioSource terminalAudio;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(FlashLabel());
		StartCoroutine(FlashCursor());
		string message = File.ReadAllText (Application.dataPath+ "/" + fileName);
		message = message.ToUpper ();
		chars = message.ToCharArray ();
		terminalAudio = gameObject.GetComponent<AudioSource> ();
	}
	
	
	
	IEnumerator FlashLabel() {
		while (true) {
			displayLabel = true;
			yield return new WaitForSeconds(letterPause);
			displayLabel = false;
			yield return new WaitForSeconds(letterPause);
		}
	}
	
	IEnumerator FlashCursor() {
		while (true) {
			displayCursor = true;
			yield return new WaitForSeconds(cursorPause);
			displayCursor = false;
			yield return new WaitForSeconds(cursorPause);
		}
	}
	
	
	public void update(){
		Debug.Log (this.inputText.textComponent.text);
		char[] a = this.inputText.textComponent.text.ToCharArray ();
		this.gameObject.GetComponent<Text> ().text += chars[a.Length-1];
		
	}
	
	void OnGUI(){
		if (displayLabel == true) {
			if (i < chars.Length) {
				string a = this.gameObject.GetComponent<Text> ().text;
				char[] test = a.ToCharArray ();
				if (test.Length == 0) {
					this.gameObject.GetComponent<Text> ().text += chars [i];
					i++;
					if (sound) {
						terminalAudio.clip = sound;
						terminalAudio.Play ();
					}
					
				} else {
					if (!(test [test.Length - 1] == '▇')) {
						if (lines > 17 && (chars [i] == '\n' || i % 35 == 0)) {
							a = this.gameObject.GetComponent<Text> ().text;
							string result = "";
							char[] split = {'\n'};
							string[] s = a.Split (split);
							for (int j = 1; j<s.Length; j++) {
								result += s [j];
								
								if (!(result [result.Length - 1] == '\n')) {
									result += '\n';
								}
								
								
							}
							
							this.gameObject.GetComponent<Text> ().text = result;
							
							
							
						}
						if (i % 35 == 0 && !(test [test.Length - 1] == '\n')) {
							Debug.Log (test [test.Length - 1]);
							lines ++;
							this.gameObject.GetComponent<Text> ().text += "\n";
						}
						
						if (chars [i] == '\n') {
							if (lines > 17) {
								i++;
							}
							lines++;
						}
						this.gameObject.GetComponent<Text> ().text += chars [i];
						i++;
						if (sound && !terminalAudio.isPlaying) {
							terminalAudio.clip = sound;
							terminalAudio.Play ();
						}
					}
				}
			}
		}
		
		
		if (displayCursor == true) {
			string a = this.gameObject.GetComponent<Text> ().text;
			char[] test = a.ToCharArray();
			if (test[test.Length-1] == '▇'){
				this.gameObject.GetComponent<Text> ().text = a.Substring(0,a.Length-1);
			}
		} else {
			string a = this.gameObject.GetComponent<Text> ().text;
			char[] test = a.ToCharArray();
			if (!(test[test.Length-1] == '▇')){
				this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text + "▇";
			}
		}
	}
}