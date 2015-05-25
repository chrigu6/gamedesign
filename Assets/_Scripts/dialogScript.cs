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
	private bool waitingForKey = false;
	private bool next = false;
	string dialog;
	ArrayList dialogLines = new ArrayList();

	// Use this for initialization
	void Start () {
		this.dialog = File.ReadAllText(Application.dataPath+ "/Dialog/" + this.gameObject.name + ".txt");
		this.text = dialogCanvas.GetComponentInChildren<Text> ();
		this.formatDialog ();
		this.dialogCanvas.enabled = false;
	}

	private void formatDialog (){
		string[] words = dialog.Split (new char[]{' ','\n'});
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
					this.dialogLines.Add(lineString);
					currentNumberOfLines = 0;
					lineString = s;
					currentLineLength = s.Length;
				}
			}
		}

	}

	public void dialogCollision ()
	{
		if (!dialogShown) {
			this.cameras.GetComponent<CameraController>().ShowDialog();
			this.dialogCanvas.enabled = true;
			this.dialogShown = true;
			StartCoroutine(ShowDialog());
		}
	}


	private IEnumerator ShowDialog()
	{
		while(this.busy)
		{
			yield return 0;
		}
		this.busy = true;
		foreach (string s in this.dialogLines) {
			this.text.text = s;
			while(!next)
			{
				StartCoroutine(WaitForKey());
			}

		}
		this.dialogCanvas.enabled = false;
		this.cameras.GetComponent<CameraController>().EndDialog();
		this.busy = false;
		yield return 0;
	}

	private IEnumerator WaitForKey()
	{
		if (waitingForKey) {
			yield return 0;
		}

		waitingForKey = true;
		next = Input.GetButtonDown ("Submit");
		waitingForKey = false;
		yield return 0;
	}

	
}
