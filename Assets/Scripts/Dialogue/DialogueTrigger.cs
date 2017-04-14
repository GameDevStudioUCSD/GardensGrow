using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {

    //static
    private int thisNpcNum=0;

	public string textFileName;
	public Collider2D activeRegionTrigger;

	public PlayerGridObject player;
    public GameObject exclamationMark;

    //privates
    private bool isTalkingToPlayer = false;
    private GameObject dialogue;
	private UIController canvas;
    private bool readAlready = false;

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
            if (exclamationMark)
            {
                exclamationMark.SetActive(false);
            }
            readAlready = true;
            isTalkingToPlayer = false;
        }
        if (!dialogue.activeSelf && activeRegionTrigger.bounds.Contains(player.transform.position) &&
             (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return)) && isTalkingToPlayer)
        {
            canvas.ShowDialog();
            dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
            dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
        }
    }
    public void OnDisable()
    {
        Globals.npcNum++;
        thisNpcNum = Globals.npcNum;
        PlayerPrefsX.SetBool("npc" + thisNpcNum, readAlready);
        
        //Debug.Log("i: " + thisNpcNum + "value: " + readAlready);
    }
    public void OnEnable()
    {
        //TODO
        readAlready = PlayerPrefsX.GetBool("npc" + thisNpcNum);
        if (!readAlready)
        {
            exclamationMark.SetActive(true);
        }
        else
        {
            exclamationMark.SetActive(false);
        }
    }
}
