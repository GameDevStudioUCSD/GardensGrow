using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
    private UIController canvas;
	public string textFileName;
	public Collider2D activeRegion;
	public PlayerGridObject player;

	private GameObject dialogue;

	void Start()
	{
        canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;
	}

	void Update()
	{
		if (!dialogue.activeSelf && activeRegion.bounds.Contains(player.transform.position) && 
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
