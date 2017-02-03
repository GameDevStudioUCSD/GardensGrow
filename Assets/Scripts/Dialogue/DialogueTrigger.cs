using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	private UIController canvas;
	public string textFileName;
	public Collider2D activeRegionTrigger;
	public PlayerGridObject player;

	private GameObject dialogue;

	// Use this for initialization
	void Start () {
		canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;
	}
	
	// Update is called once per frame
	void Update () {
		if (!dialogue.activeSelf && activeRegionTrigger.bounds.Contains(player.transform.position) && 
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
