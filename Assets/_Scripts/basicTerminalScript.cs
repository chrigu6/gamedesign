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
		name = name.Trim(new char[] {'▇'});
		this.inputString = name;
		this.typing = false;
	}

	override protected IEnumerator scriptInput(string body)
	{
		while (this.busy) {
			yield return 0;
		}
		this.busy = true;
		body = body.ToUpper ();
		string[] lines = body.Split ('\n');
		int j = 0;

		while (j<lines.Length) {
			if(lines[j].Equals("{INPUT}"))
			{
				j++;
				while(!lines[j].Equals("{OUTPUT}"))
				 {
					this.writeLineMethod(lines[j]);
					j++;
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
					//Debug.Log(j);
				}
				else{
					this.writeLineMethod("\nIncorrect. Try Again");
					this.originalText = this.gameObject.GetComponent<Text> ().text;
					this.inputField.enabled = false;
					system.SetSelectedGameObject(null);
					j--;
				}
			}

			if(!lines[j].Equals ("{OUTPUT}") && !lines[j].Equals("{INPUT}"))
			{
				break;
			}
		}
		this.cleanCursor();
		this.busy = false;
	}
}