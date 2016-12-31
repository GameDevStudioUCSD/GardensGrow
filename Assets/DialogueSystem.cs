using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {

	public string[] textLines;
	public TextAsset textFile;
	public Text displayedLine;
	int lineCounter = 0;
	bool writing = false;

	
	public void LoadText()
	{
		if (textFile == null) {
			Debug.LogError ("Text file was not loaded correctly.");
		}

		textLines = textFile.text.Split ('\n');
		StartCoroutine (AnimateText (textLines [lineCounter]));
		lineCounter++;

	}

	IEnumerator AnimateText(string strComplete){
		int i = 0;
		displayedLine.text = "";
		while( i < strComplete.Length ){
			if (Input.GetKeyDown (KeyCode.Tab) && i > 5) {
				displayedLine.text += strComplete.Substring (i);
				i = strComplete.Length;
			}
			else
				displayedLine.text += strComplete[i++];
			yield return new WaitForSeconds(0.01F);
		}
		writing = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			//play text sound
			if (lineCounter < textLines.Length) {
				if (!writing) {
					writing = true;
					StartCoroutine (AnimateText (textLines [lineCounter]));
				}
				lineCounter++;
			}
			 else {
				textLines = null;
				textFile = null;
				lineCounter = 0;
				this.transform.parent.gameObject.SetActive (false);
			}
		}
	}
}
