using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class basicTerminalScript : MonoBehaviour {

	private bool typing = false;
	private bool isWriting = false;
	private bool cursorVisible = false;
	private bool busy = false;
	
	public float cursorSpeed;
	public float letterPause = 0;
	private EventSystem system;
	public AudioClip key1;
	public AudioClip key2;
	public AudioClip key3;
	public AudioClip key4;
	public AudioClip key5;
	public AudioClip key6;
	private AudioClip[] keys = new AudioClip[6];


	public InputField inputField;
	public string fileName;
	private char[] chars;
	private int i = 0;
	private int lines = 0;
	private int charPerLines = 0;
	private AudioSource terminalAudio;
	private bool active = false;
	private bool abort = false;
	string originalText;
	string[] instructions;
	private string defaultText ="  **** COMMODORE 64 BASIC V2 ****\n        64K RAM SYSTEM\n	    38911 BASIC BYTES FREE\n\nREADY\n";
	private static Regex instructionPattern = new Regex("[<][a-zA-Z]*[>]");
	
	// Use this for initialization
	void Start () 
	{
		system = EventSystem.current;
		this.inputField.enabled = false;
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent ();
		submitEvent.AddListener (endInput);
		inputField.onEndEdit = submitEvent;
		this.gameObject.GetComponent<Text> ().text = this.defaultText;
		this.instructions = File.ReadAllLines(Application.dataPath+ "/" + fileName);
		//chars = message.ToCharArray();
		terminalAudio = gameObject.GetComponent<AudioSource> ();
		terminalAudio.mute = true;
		this.gameObject.GetComponentInParent<Canvas> ().enabled = false;
		keys [0] = key1;
		keys [1] = key2;
		keys [2] = key3;
		keys [3] = key4;
		keys [4] = key5;
		keys [5] = key6;


	}

	public void start()
	{
		this.Start ();
	}
	
	void OnGUI() {
		if (active) {;
			if(Input.GetButtonDown ("SwitchPlayer"))
			{
				this.abortTerminal();
			}
		}
	}

	public IEnumerator run(){
		this.changeState ();
		this.gameObject.GetComponentInParent<Canvas> ().enabled = true;
		for(int instructionCounter = 0; instructionCounter < instructions.Length; instructionCounter = instructionCounter)
		{
			string instruction = instructions[instructionCounter];
			instructionCounter ++;
			string body = "";

			do
			{	while(this.busy)
				{
					yield return 0;
				}
				Debug.Log ("body = " + body);
				body = body + instructions[instructionCounter]+"\n";
				instructionCounter++;
				if(instructionCounter >= instructions.Length)
				{
					break;
				}
			}while(!instructionPattern.IsMatch(instructions[instructionCounter]));

			StartCoroutine(callMethod(instruction,body));	
		}

		yield return 0;

	}

	IEnumerator getInput()
	{
		while (busy) {
			yield return 0;
		}
		this.busy = true;
		this.inputField.enabled = true;
		this.typing = true;
		Selectable next = this.inputField.GetComponentInChildren<Selectable>();
		system.SetSelectedGameObject(next.gameObject);
		this.originalText = this.gameObject.GetComponent<Text> ().text;
		while (typing) {

			yield return 0;
		}
		this.busy = false;
		yield return 0;
	}

	void updateInput()
	{
		StopCoroutine (this.ShowCursorWriting ());
		i++;
		charPerLines = this.inputField.text.Length;
		char currentLetter = 'z';
			string input = this.inputField.text.ToUpper();
			this.gameObject.GetComponent<Text> ().text = this.originalText + input;
			if (charPerLines % 35 == 0 && !(i == 0)) {
			this.inputField.text = this.inputField.text + "\n";
			this.inputField.MoveTextEnd(false);
				this.gameObject.GetComponent<Text> ().text = this.originalText + input;
				currentLetter = '\n';
			}
		StartCoroutine (this.ShowCursorWriting ());
			//Count the number of lines if the number of lines reach the bottom of the textfield, start to wrap the lines
			/*if (currentLetter == '\n') {
				charPerLines = 0;
				lines++;
				if (lines > 5) {
				string a = this.gameObject.GetComponent<Text> ().text;
				int f = a.IndexOf ("\n");
				this.inputField.text = a.Substring(f+1);
				this.gameObject.GetComponent<Text> ().text = this.originalText + input;
				}
			}*/
	}

	void endInput(string name)
	{

	}

	
	IEnumerator callMethod(string instruction, string body)
	{
		switch (instruction)
		{
		case "<w>":
			StartCoroutine(write (body));
			yield return 0;
			break;
		case "<i>":
			StartCoroutine(this.getInput());
			yield return 0;
			break;
		case "<wL>":
			StartCoroutine(this.writeLine(body));
			yield return 0;
			break;
		case "<fC>":
			StartCoroutine(cursor(body));
			yield return 0;
			break;
		case "<m>":
			StartCoroutine(matrix(body));
			yield return 0;
			break;
		default:
			yield return 0;
			break;
		}
	}

	IEnumerator matrix(string body)
	{
		while (this.busy) {;
			yield return 0;
		}
		this.busy = true;
		int numberOfLines = this.gameObject.GetComponent<Text> ().text.Split('n').Length;
		int count = System.Int32.Parse (body);
		for (int i = 0; i < count; i++) {
			string line = "";
			for (int j=0; j < 6; j++)
				{
					string word = "";
					for(int c = 0; c < Random.Range(4,6); c++ )
					{
						word = word + System.Convert.ToString(this.RandomLetter());
					}
					line = line + " " + word;
				}
			this.writeLineMethod(line);
			numberOfLines++;
			if(numberOfLines>18)
			{
				this.wrapLines();
			}
			yield return new WaitForSeconds(letterPause);
		}
		this.busy = false;
	}

	void writeLineMethod(string line)
	{
		this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text + line + "\n";
	}

	IEnumerator write(string body){
		body = body.ToUpper ();
		chars = body.ToCharArray();
		//If the end of the message is not reached and not allready writing startwriting
		if (!isWriting && (i < chars.Length - 1)) {
			isWriting = true;
			cursorVisible = false;
			StartCoroutine(TypeText());
			//StopCoroutine (ShowCursor ());
		} 
		yield return 0;
	}

	
	IEnumerator TypeText ()
	{
		while (this.busy) {;
			yield return 0;
		}
		this.busy = true;
		foreach (char letter in chars) {
			
			i++;
			this.gameObject.GetComponent<Text> ().text += letter;
			charPerLines++;
			char currentLetter = letter;

			if (!terminalAudio.isPlaying)
			{
				terminalAudio.clip = this.keys[Random.Range(0,5)];
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
				if (lines > 18)
				{
					wrapLines();
				}
			}

			yield return new WaitForSeconds(letterPause);
		}
		this.busy = false;
		yield return 0;
	}
	
	void wrapLines ()
	{
		Debug.Log ("wrap lines");
		string a = this.gameObject.GetComponent<Text> ().text;
		int i = a.IndexOf ("\n");
		this.gameObject.GetComponent<Text> ().text = a.Substring(i+1);
		
	}

	IEnumerator cursor(string body)
	{
		int count = System.Int32.Parse (body);
		StartCoroutine (ShowCursor(count));
		yield return 0;
	}
	
	IEnumerator ShowCursor(int count) {
		while (busy) {
			yield return 0;
		}
		this.busy = true;
		int j = 0;
		while (j<count) {
			string a = this.gameObject.GetComponent<Text> ().text;
			char lastChar = a.ToCharArray () [a.Length-1];
			
			if (lastChar == '▇') {
				this.gameObject.GetComponent<Text> ().text = a.Substring (0, a.Length - 1);
			} else {
				this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text + "▇";
			}
			j++;
			yield return new WaitForSeconds (cursorSpeed);
		}
		this.busy = false;
		yield return 0;
	}

	IEnumerator ShowCursorWriting() {
		while (true) {
			StopCoroutine(ShowCursorWriting());
			string a = this.gameObject.GetComponent<Text> ().text;
			char lastChar = a.ToCharArray () [a.Length-1];
			
			if (lastChar == '▇') {
				this.gameObject.GetComponent<Text> ().text = a.Substring (0, a.Length - 1);
			} else {
				this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text + "▇";
			}
			yield return new WaitForSeconds (cursorSpeed);
		}
		yield return 0;
	}

	IEnumerator writeLine(string body)
	{

		while (busy) {
			yield return 0;
		}
		this.busy = true;
		int numberOfLines = this.gameObject.GetComponent<Text> ().text.Split('n').Length;
		string[] lines = body.Split ('\n');
		foreach (string line in lines) {

			this.writeLineMethod(line);
			numberOfLines++;
			Debug.Log ("lines :" + numberOfLines);
			while(numberOfLines>18)
			{
				numberOfLines--;
				this.wrapLines();
			}
			yield return new WaitForSeconds(letterPause);
		}
		Debug.Log ("Busy false");
		this.busy = false;

	}

	public void changeState()
	{
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
		this.busy = false;
		this.typing = false;
		this.gameObject.GetComponent<Text> ().text = this.defaultText;
		this.terminalAudio.mute = true;
		this.abort = true;
		this.i = 0;
	    this.lines = 0;
		StopAllCoroutines ();
		this.isWriting = false;
		this.GetComponent<basicTerminalScript> ().enabled = false;
	}

	public char RandomLetter()
	{

		int num = Random.Range(0, 256); // Zero to 25
		char let = (char)('a' + num);
		return let;
	}
}