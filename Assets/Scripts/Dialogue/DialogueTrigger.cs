using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public UIController	canvas;
	public string textFileName;
	public Collider2D activeRegion;
	public PlayerGridObject player;

	private GameObject dialogue;

	void Start()
	{
		dialogue = canvas.dialogUI;
	}

	void Update()
	{
		if (!dialogue.activeSelf && activeRegion.bounds.Contains(player.transform.position) && 
			(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))) {

			canvas.ShowDialog();

			dialogue.GetComponentInChildren<DialogueSystem> ().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
			dialogue.GetComponentInChildren<DialogueSystem> ().LoadText ();
		}
	}
		
}
