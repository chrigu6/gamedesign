using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class dialogScript : MonoBehaviour {

	public Canvas dialogCanvas;
	private Text text;
	private bool dialogShown = false;
	public GameObject cameras;
	private bool busy = false;
	private int counter = 0;
	string dialog;
	object[] dialogLines;
	

	// Use this for initialization
	void Start () {

		this.dialog = File.ReadAllText(Application.dataPath+ "/Dialog/" + this.gameObject.name + ".txt");
		this.text = dialogCanvas.GetComponentInChildren<Text> ();
		this.formatDialog ();
		this.dialogCanvas.enabled = false;



	}

	private void formatDialog (){
		string[] words = dialog.Split (new char[]{' ','\n'});
		ArrayList temp = new ArrayList ();
		int currentLineLength = 0;
		int currentNumberOfLines = 0;
		string lineString = "";
		foreach (string s in words) {
			if(currentLineLength + s.Length < 60)
			{
				lineString += " " + s;
				currentLineLength += s.Length;
			}
			else{
				if(currentNumberOfLines < 2)
				{
					lineString += "\n" + s;
					currentLineLength = s.Length; 
					currentNumberOfLines++;
				}

				else{
					temp.Add(lineString);
					currentNumberOfLines = 0;
					lineString = s;
					currentLineLength = s.Length;
				}
			}
		}
		temp.Add(lineString);
		this.dialogLines = temp.ToArray ();
	}

	public IEnumerator dialogCollision ()
	{
		if (!dialogShown) {
			this.cameras.GetComponent<CameraController>().ShowDialog();
			this.dialogCanvas.enabled = true;
			this.dialogShown = true;
			while(counter < this.dialogLines.Length)
			{
				StartCoroutine(ShowDialog());
				yield return 0;
			}
			this.cameras.GetComponent<CameraController>().EndDialog();
			this.dialogCanvas.enabled = false;


			
		}
		yield return 0;
	}


	private IEnumerator ShowDialog()
	{
		while(this.busy)
		{
			yield return 0;
		}
		this.busy = true;
		this.text.text = this.dialogLines [this.counter] as string;

		if (Input.GetButton ("Submit")) {
			counter ++;
		}

		this.busy = false;
		yield return 0;
	}	
}
