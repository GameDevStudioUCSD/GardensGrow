using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {

    /*NOTE: Ever time you place a new sign or npc make sure to change the saveNumber
     *      in the inspector to a number not yet used (check the top of globals.cs 
     *      for saveNumbers that's already been used)
     */
    public int saveNumber;

    public string textFileName;
	public Collider2D activeRegionTrigger;

	public PlayerGridObject player;
    public GameObject exclamationMark;

    //privates
    private bool isTalkingToPlayer = false;
    private GameObject dialogue;
	private UIController canvas;
    public bool readAlready = false;
    
	// Use this for initialization
	void Start () {
		canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;
        readAlready = PlayerPrefsX.GetBool("sign" + saveNumber + "lvl" + Application.loadedLevel + "slot" + Globals.loadedSlot);
    }
	
	// Update is called once per frame
	void Update () {
        if (!player) return; //fixes first frame error
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
                readAlready = true;
                isTalkingToPlayer = false;
                exclamationMark.SetActive(false);
            }
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
        PlayerPrefsX.SetBool("sign" + saveNumber + "lvl" + Application.loadedLevel + "slot" + Globals.loadedSlot, readAlready);
        //TODO:: when build replace readAlready w/ false and vice versa
    }

    //maybe deprecate
    public void saveBool(int saveSlot)
    {
        //PlayerPrefsX.SetBool("sign" + saveNumber + "save" + saveSlot, readAlready);
    }
    public void OnEnable()
    {
        readAlready = PlayerPrefsX.GetBool("sign" + saveNumber + "lvl" + Application.loadedLevel + "slot" + Globals.loadedSlot);

        if (readAlready)
        {
            exclamationMark.SetActive(false);
        }
        else
        {
            exclamationMark.SetActive(true);
        }
    }
}
