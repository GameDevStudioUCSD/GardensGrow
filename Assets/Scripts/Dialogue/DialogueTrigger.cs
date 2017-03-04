using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	private UIController canvas;
	public string textFileName;
	public Collider2D activeRegionTrigger;
    private bool isTalkingToPlayer = false;
	public PlayerGridObject player;

	private GameObject dialogue;

	// Use this for initialization
	void Start () {
		canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;
	}
	
	// Update is called once per frame
	void Update () {
        if (activeRegionTrigger.bounds.Contains(player.transform.position))
        {
            if (!isTalkingToPlayer)
            {
                isTalkingToPlayer = true;
                canvas.ShowDialog();
                dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
                dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
            }
        }
        else if (!activeRegionTrigger.bounds.Contains(player.transform.position) && isTalkingToPlayer)
        {
            canvas.EndDialog();
            isTalkingToPlayer = false;
        }
        if (!dialogue.activeSelf && activeRegionTrigger.bounds.Contains(player.transform.position) &&
             (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return)) && isTalkingToPlayer)
        {
            canvas.ShowDialog();
            dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
            dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
        }
        /*
		if (!dialogue.activeSelf && activeRegionTrigger.bounds.Contains(player.transform.position) && 
			(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))) {
            canvas.ShowDialog();

			dialogue.GetComponentInChildren<DialogueSystem> ().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
			dialogue.GetComponentInChildren<DialogueSystem> ().LoadText ();
		}*/
    }
}
