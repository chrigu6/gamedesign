using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class basicTerminalScript : abstractTerminalScript {


	private string inputString;
	private string targetString;

	override protected void abstractStart()
	{
		this.gameObject.GetComponentInParent<Canvas> ().enabled = false;
	}

	override protected void endInput(string name)
	{
		name = name.Replace ("\n", "").ToUpper();
		this.inputString = name;
		this.typing = false;
		Debug.Log ("finished");
	}

	override protected IEnumerator scriptInput(string body)
	{
		while (this.busy) {
			yield return 0;
		}
		this.busy = true;
		body = body.ToUpper ();
		string[] lines = body.Split ('\n');
		int numberOfLines = lines.Length;
		int j = 0;
		while (j<lines.Length) {
			if(lines[j].Equals("{INPUT}"))
			{
				j++;
				while(!lines[j].Equals("{OUTPUT}"))
				 {
					this.writeLineMethod(lines[j]);
					j++;
					numberOfLines++;
					while(numberOfLines>20)
					{
						numberOfLines--;
						this.wrapLines();
					}
					yield return new WaitForSeconds(letterPause);
					if(j>=lines.Length)
					{
						break;
					}
				}
			}

			if(j>=lines.Length)
			{
				break;
			}

			if(lines[j].Equals ("{OUTPUT}"))
			{
				this.targetString = lines[j-1];
				j++;
				this.inputField.enabled = true;
				this.typing = true;
				Selectable next = this.inputField.GetComponentInChildren<Selectable>();
				Debug.Log (next.gameObject);
				system.SetSelectedGameObject(next.gameObject);
				this.originalText = this.gameObject.GetComponent<Text> ().text;
				while(typing )
				{
					yield return 0;
				}

				if(this.inputString.Equals(this.targetString))
				{
					this.gameObject.GetComponent<Text> ().text += "\n";
					this.inputField.enabled = false;
					system.SetSelectedGameObject(null);
				}
				else{
					this.writeLineMethod("\nIncorrect. Try Again");
					this.originalText = this.gameObject.GetComponent<Text> ().text;
					this.inputField.enabled = false;
					system.SetSelectedGameObject(null);
					j--;
				}
			}
			
		}
		this.busy = false;
	}
}