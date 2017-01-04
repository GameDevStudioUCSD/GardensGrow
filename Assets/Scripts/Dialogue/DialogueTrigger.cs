using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {

	public GameObject dialogue;
	public bool condition;
	public string textFileName;
	public Collider2D activeRegion;
	public PlayerGridObject player;

	void Update()
	{
		if (!dialogue.activeSelf && activeRegion.bounds.Contains(player.transform.position) && Input.GetKeyDown(KeyCode.Tab)) {
			dialogue.SetActive (true);

			dialogue.GetComponentInChildren<DialogueSystem> ().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
			dialogue.GetComponentInChildren<DialogueSystem> ().LoadText ();
		}
	}
		
}
