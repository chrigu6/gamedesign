using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public abstract class abstractTerminalScript : MonoBehaviour {
	protected bool typing = false;
	protected bool isWriting = false;
	protected bool cursorVisible = false;
	protected bool busy = false;
	
	public float cursorSpeed;
	public float letterPause = 0;
	protected EventSystem system;
	public AudioClip key1;
	public AudioClip key2;
	public AudioClip key3;
	public AudioClip key4;
	public AudioClip key5;
	public AudioClip key6;
	protected AudioClip[] keys = new AudioClip[6];

	protected int cursorStarted = 0;
	public InputField inputField;
	public string fileName;
	protected char[] chars;
	protected int i = 0;
	protected int lines = 0;
	protected int charPerLines = 0;
	protected AudioSource terminalAudio;
	protected bool active = false;
	protected bool abort = false;
	protected string originalText;
	protected string[] instructions;
	protected string defaultText ="  **** COMMODORE 64 BASIC V2 ****\n        64K RAM SYSTEM\n	    38911 BASIC BYTES FREE\n\nREADY\n";
	protected static Regex instructionPattern = new Regex("[<][a-zA-Z]*[>]");

	protected void Start()
	{
		this.abort = false;
		this.cursorStarted = 0;
		system = EventSystem.current;
		this.inputField.enabled = false;
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent ();
		submitEvent.AddListener (endInput);
		inputField.onEndEdit = submitEvent;
		this.gameObject.GetComponent<Text> ().text = this.defaultText;

		//Important: change to Application.persistentdataPath before releasing!!!!!!!!!

		this.instructions = File.ReadAllLines(Application.dataPath+ "/" + fileName);
		//chars = message.ToCharArray();
		terminalAudio = gameObject.GetComponent<AudioSource> ();
		terminalAudio.mute = true;
		this.abstractStart ();
	}

	public void start()
	{
		this.Start ();
	}

	protected void OnGUI() {
		if (active) {;
			if(Input.GetButtonDown ("Abort"))
			{
				this.abortTerminal();
			}
		}
	}

	protected void abortTerminal ()
	{
		this.busy = false;
		this.typing = false;
		this.gameObject.GetComponent<Text> ().text = this.defaultText;
		this.terminalAudio.mute = true;
		this.abort = true;
		this.i = 0;
		this.lines = 0;
		this.StopAllCoroutines ();
		this.GetComponent<basicTerminalScript> ().enabled = false;
		this.cursorStarted = 0;
	}


	public IEnumerator run(){
		this.changeState ();
		this.gameObject.GetComponentInParent<Canvas> ().enabled = true;
		for(int instructionCounter = 0; instructionCounter < instructions.Length;)
		{
			if(this.abort)
			{
				StopAllCoroutines();
				break;
			}
			string instruction = instructions[instructionCounter];
			instructionCounter ++;
			string body = "";
			
			do
			{	while(this.busy)
				{
					yield return 0;
				}
				body = body + instructions[instructionCounter]+"\n";
				instructionCounter++;
				if(instructionCounter >= instructions.Length)
				{
					break;
				}
			}while(!instructionPattern.IsMatch(instructions[instructionCounter]));
			this.cleanCursor();
			StartCoroutine(callMethod(instruction,body));	
		}
		
		yield return 0;
		
	}

	public void changeState()
	{
		this.gameObject.GetComponentInParent<Canvas> ().enabled = true;
		this.active = !this.active;
		this.abort = false;
	}

	protected IEnumerator callMethod(string instruction, string body)
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
		case "<s>":
			StartCoroutine (play(body));
			yield return 0;
			break;
		case "<scriptInput>":
			StartCoroutine(scriptInput(body));
			yield return 0;
			break;
			
		default:
			yield return 0;
			break;
		}
	}

	
	protected IEnumerator write(string body){
		while (this.busy) {
			yield return 0;
		}
		this.busy = true;
		body = body.ToUpper ();
		chars = body.ToCharArray();
		StartCoroutine(TypeText()); 
		yield return 0;
	}

	protected IEnumerator TypeText ()
	{
		
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

	protected void wrapLines ()
	{
		string a = this.gameObject.GetComponent<Text> ().text;
		int i = a.IndexOf ("\n");
		this.gameObject.GetComponent<Text> ().text = a.Substring(i+1);
		
	}

	protected IEnumerator getInput()
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

	protected IEnumerator writeLine(string body)
	{
		
		while (busy) {
			yield return 0;
		}
		this.busy = true;
		string text = this.gameObject.GetComponent<Text> ().text;
		string[] lines = text.Split ('\n');
		int numberOfLines = lines.Length;
		lines = body.Split ('\n');
		foreach (string line in lines) {
			string line2 = line.ToUpper();
			this.writeLineMethod(line2);
			numberOfLines++;
			while(numberOfLines>20)
			{
				numberOfLines--;
				this.wrapLines();
			}
			yield return new WaitForSeconds(letterPause);
		}
		this.busy = false;
		
	}

	protected void writeLineMethod(string line)
	{
		
		this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text + line + "\n";
	}


	protected IEnumerator cursor(string body)
	{
		int count = System.Int32.Parse (body);
		StartCoroutine (ShowCursor(count));
		yield return 0;
	}

	protected IEnumerator ShowCursor(int count) {
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
	
	protected IEnumerator matrix(string body)
	{
		while (this.busy) {
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
			if(numberOfLines>20)
			{
				this.wrapLines();
			}
			yield return new WaitForSeconds(letterPause);
		}
		this.busy = false;
	}
	
	protected IEnumerator play(string body)
	{
		while (this.busy) {
			yield return 0;
		}
		body = body.Remove (body.Length - 1);
		this.busy = true;
		terminalAudio.clip = (AudioClip)Resources.Load(body);
		terminalAudio.mute = false;
		terminalAudio.Play ();
		
		while (terminalAudio.isPlaying) {
			yield return 0;
		}
		
		this.busy = false;
		yield return 0;
		
	}

	public char RandomLetter()
	{
		
		int num = Random.Range(0, 256); // Zero to 25
		char let = (char)('a' + num);
		return let;
	}

	
	public bool getAbort()
	{
		return this.abort;
	}
	
	public bool isActive()
	{
		return this.active;
	}

	protected void updateInput()
	{
		StopCoroutine (this.ShowCursorWriting ());
		this.cleanCursor ();
		i++;
		charPerLines = this.inputField.text.Length;
		//char currentLetter = 'z';
		string input = this.inputField.text.ToUpper();
		this.gameObject.GetComponent<Text> ().text = this.originalText + input;
		
		if (charPerLines % 35 == 0 && !(i == 0)) {
			this.inputField.text = this.inputField.text + "\n";
			this.inputField.MoveTextEnd(false);
			this.gameObject.GetComponent<Text> ().text = this.originalText + input;
			//currentLetter = '\n';
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

	protected IEnumerator ShowCursorWriting() {
		this.cursorStarted++;
		
		if (cursorStarted <= 1) {
			while (true && !this.abort) {
				StopCoroutine (ShowCursorWriting ());
				string a = this.gameObject.GetComponent<Text> ().text;
				char lastChar = a.ToCharArray () [a.Length - 1];
				
				if (lastChar == '▇') {
					this.gameObject.GetComponent<Text> ().text = a.Substring (0, a.Length - 1);
				} else {
					this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text + "▇";
				}
				yield return new WaitForSeconds (cursorSpeed);
			}
		}
		yield return 0;
	}

	void cleanCursor ()
	{
		this.gameObject.GetComponent<Text> ().text = this.gameObject.GetComponent<Text> ().text.Trim(new char[] {'▇'});
	}

	abstract protected void abstractStart();
	abstract protected void endInput(string name);
	abstract protected IEnumerator scriptInput(string body);
}

