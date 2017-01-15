using UnityEngine;
using System.Collections;

public class DialogueNPCTrigger : MonoBehaviour {
	private UIController canvas;
	public string textFileName;
	public Collider2D activeRegion;
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
		if (!dialogue.activeSelf && activeRegion.bounds.Contains(player.transform.position) && triggered == false) {

            //NOTE: this code SHOULD work to pause the game if talking to sign 
            //Time.timeScale = 0;
            //canvas.paused = true;
            canvas.ShowDialog();

			dialogue.GetComponentInChildren<DialogueSystem> ().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
			dialogue.GetComponentInChildren<DialogueSystem> ().LoadText ();
            //canvas.paused = false;

            triggered = true;
		}
	}
}
