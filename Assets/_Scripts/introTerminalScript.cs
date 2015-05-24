using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class introTerminalScript : abstractTerminalScript {	
	public GameObject movieQuad;


	override protected void abstractStart()
	{
		this.gameObject.GetComponentInParent<Canvas> ().enabled = true;
		this.instructions = File.ReadAllLines(Application.dataPath+ "/terminalintro.txt");
		this.movieQuad.SetActive (false);
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
		MovieTexture movTexture = this.movieQuad.GetComponent<Renderer>().material.mainTexture as MovieTexture;
		this.movieQuad.SetActive (true);
		movTexture.Play ();
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

	override protected IEnumerator scriptInput(string body)
	{		
		//Not needed
		yield return 0;
	}

	override protected void endInput(string name){
		//Not needed
	}
}

