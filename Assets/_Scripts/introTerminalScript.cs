using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class introTerminalScript : abstractTerminalScript {	
	public GameObject movieQuad;
	public GameObject terminalCanvas;
	private MovieTexture movTexture;

	new protected void OnGUI() {
		if (active) {;
			if(Input.GetButtonDown ("Abort"))
			{
				Application.LoadLevel ("level1");
			}
		}
	}

	override protected void abstractStart()
	{
		this.gameObject.GetComponentInParent<Canvas> ().enabled = true;
		this.instructions = File.ReadAllLines(Application.dataPath+ "/TerminalScripts/terminalintro.txt");
		this.movieQuad.SetActive (false);
		this.movTexture = this.movieQuad.GetComponent<Renderer>().material.mainTexture as MovieTexture;
		StartCoroutine (this.run ());
	}

	new protected IEnumerator callMethod(string instruction, string body)
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
		case "<cameraSwitch>":
			this.enableCameraSwitch();
			break;
		case "<characterSwitch>":
			this.enableCharacterSwitch();
			break;
		case "<enableShooting>":
			this.enableShooting();
			break;
		case "<end>":
			StartCoroutine (end(body));
			yield return 0;
			break;
		case "<sV>":
			StartCoroutine(showVideo(body));
			yield return 0;
			break;				
		default:
			yield return 0;
			break;
		}
	}


	public IEnumerator showVideo(string body)
	{
		while (this.busy) {
			yield return 0;
		}
		this.busy = true;
		this.movieQuad.SetActive (true);
		RectTransform terminal = terminalCanvas.GetComponent<RectTransform> ();
		Vector3[] canvasPosition = new Vector3[4];
		terminal.GetWorldCorners (canvasPosition);
		MeshCollider collider = movieQuad.GetComponent<MeshCollider> ();
		this.movieQuad.transform.position = (canvasPosition[2]) + ((canvasPosition[3] - canvasPosition[2])/2)  + new Vector3 (-collider.bounds.extents[0]*1.5f,0, -0.1f);
		movTexture.Play ();
		this.terminalAudio.mute = true;
		this.movieQuad.GetComponent<AudioSource> ().Play ();
		this.busy = false;
		yield return 0;
	}

	new public IEnumerator run(){
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
			
			StartCoroutine(callMethod(instruction,body));	
		}

		while (this.movTexture.isPlaying) {
			yield return 0;
		}
		StartCoroutine (nextLevel ());
		yield return 0;
		
	}

	IEnumerator nextLevel()
	{
		while (this.busy) {
			yield return 0;
		}
		Application.LoadLevel ("level1");
	}

	new protected IEnumerator TypeText ()
	{
		StopCoroutine (ShowCursorWriting ());
		this.cleanCursor ();
		foreach (char letter in chars) {
			this.cleanCursor();
			i++;
			this.gameObject.GetComponent<Text> ().text += letter;
			charPerLines++;
			char currentLetter = letter;

			
			
			
			//If the textline is to long, start a new one
			if(charPerLines%this.lineLength == 0 && !(i ==0))
			{
				this.gameObject.GetComponent<Text> ().text += '\n';
				currentLetter = '\n';
			}
			
			//Count tfhe number of lines if the number of lines reach the bottom of the textfield, start to wrap the lines
			if (currentLetter == '\n')
			{
				charPerLines = 0;
				if(this.getNumberOfLines()>this.maxNumberOfLines)
				{
					wrapLines();
				}
			}
			//Debug.Log (letterPause);
			yield return new WaitForSeconds(letterPause);
		}
		this.busy = false;
		yield return 0;
	}

	override protected IEnumerator scriptInput(string body)
	{		
		//Not needed
		yield return 0;
	}

	override protected void endInput(string name){
		//Not needed
	}
}

