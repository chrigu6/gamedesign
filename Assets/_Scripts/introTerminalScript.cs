using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class introTerminalScript : abstractTerminalScript {	

	override protected void abstractStart()
	{
		this.gameObject.GetComponentInParent<Canvas> ().enabled = true;
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

		Application.LoadLevel("level1");
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

