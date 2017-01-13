using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {

	public string[] textLines;
	public TextAsset textFile;
	public Text displayedLine;
	public int lineCounter = 0;
	bool writing = false;

	//Class initialization.
	public void LoadText()
	{
		textLines = null;
		lineCounter = 0;
		//GetComponentInChildren<Text> ().text = "";
		if (textFile == null) {
			Debug.LogError ("Text file was not loaded correctly.");
		}

		textLines = textFile.text.Split ('\n');
		if (!writing) {
			writing = true;
			StartCoroutine (AnimateText (textLines [lineCounter]));
		}
		lineCounter++;

	}

	//You only need concern yourself with WaitForSeconds.
	IEnumerator AnimateText(string strComplete){
		//Debug.Log (strComplete);
		displayedLine.text = "";
		for(int i = 0; i < strComplete.Length; i++){
			if (Input.GetKeyDown (KeyCode.Tab) && i > 5) {
				displayedLine.text += strComplete.Substring (i);
				break;
			} else {
				displayedLine.text += strComplete [i];
				yield return new WaitForSecondsRealtime (0.01F);
			}
		}

		writing = false;
	}

	void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return)) && !writing)
		{
			//insert text sound
			if (lineCounter < textLines.Length) {
				
				writing = true;
				StartCoroutine (AnimateText (textLines [lineCounter]));
			
				lineCounter++;
			}
			 else {
				//Reset for next dialogue input.
				this.transform.parent.parent.GetComponent<UIController>().EndDialog();
			}
		}
	}
}
