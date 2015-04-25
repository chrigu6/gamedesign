using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class basicTerminalScript : MonoBehaviour {
	
		
		private bool displayLabel = false;
		public float letterPause = 0.2f;
		public AudioClip sound;
		public InputField inputText;
		
		public string message;
		private char[] chars;
		private int i = 0;
		private AudioSource terminalAudio;
		
		// Use this for initialization
		void Start () {
			StartCoroutine(FlashLabel());
		message = message.ToUpper ();
			chars = message.ToCharArray ();
		terminalAudio = gameObject.GetComponent<AudioSource> ();
		}
		
		
		
		IEnumerator FlashLabel() {
			while (true) {
				displayLabel = true;
				yield return new WaitForSeconds(.5f);
				displayLabel = false;
				yield return new WaitForSeconds(.5f);
			}
		}
		
		public void update(){
			Debug.Log (this.inputText.textComponent.text);
			char[] a = this.inputText.textComponent.text.ToCharArray ();
			this.gameObject.GetComponent<Text> ().text += chars[a.Length-1];
			
		}
		
		void OnGUI(){
			
			if (i < chars.Length) {
				string a = this.gameObject.GetComponent<Text> ().text;
				char[] test = a.ToCharArray();
				if(test.Length == 0)
				{
					this.gameObject.GetComponent<Text> ().text += chars[i];
					i++;
					if (sound)
					{
					terminalAudio.clip = sound;
					terminalAudio.Play();
					}
					
				}
				else{
					if (!(test[test.Length-1] == '▇')){
						this.gameObject.GetComponent<Text> ().text += chars[i];
						i++;
					if (sound && !terminalAudio.isPlaying)
					{
						terminalAudio.clip = sound;
						terminalAudio.Play();
					}
					}
				}
			}
			
			if (displayLabel == true) {
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
