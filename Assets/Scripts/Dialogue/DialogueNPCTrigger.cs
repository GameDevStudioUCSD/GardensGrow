using UnityEngine;
using System.Collections;

public class DialogueNPCTrigger : MoveableGridObject {
	private UIController canvas;
	public string textFileName;
	public Collider2D activeRegionPreTrigger;
	public Collider2D activeRegionPostTrigger;
	public PlayerGridObject player;

	private GameObject dialogue;
	private bool triggered;

	// Use this for initialization
	void Start () {
		canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!dialogue.activeSelf && activeRegionPreTrigger.bounds.Contains(player.transform.position) && triggered == false) {

            //NOTE: this code SHOULD work to pause the game if talking to sign 
            //Time.timeScale = 0;
            //canvas.paused = true;
            canvas.ShowDialog();

			dialogue.GetComponentInChildren<DialogueSystem> ().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
			dialogue.GetComponentInChildren<DialogueSystem> ().LoadText ();
            //canvas.paused = false;

            triggered = true;
		}
		if (!dialogue.activeSelf && activeRegionPostTrigger.bounds.Contains(player.transform.position) && 
			(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))) {

            //NOTE: this code SHOULD work to pause the game if talking to sign 
            //Time.timeScale = 0;
            //canvas.paused = true;
            canvas.ShowDialog();

			dialogue.GetComponentInChildren<DialogueSystem> ().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
			dialogue.GetComponentInChildren<DialogueSystem> ().LoadText ();
            //canvas.paused = false;
		}
	}
}
