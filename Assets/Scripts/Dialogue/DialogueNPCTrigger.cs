using UnityEngine;
using System.Collections;

public class DialogueNPCTrigger : MoveableGridObject {

	private UIController canvas;
	public string textFileName;
	public Collider2D activeRegionPreTrigger;
	public Collider2D activeRegionPostTrigger;

    //moving stuff
	private PlayerGridObject player;
    private bool talkedToPlayer = false;
    private int moveDist;
    private bool moving = true;
    private bool movingBack = true;
    private bool calculatedDist;
    private int counter = 0;
	private GameObject dialogue;
	private bool triggered;
    private bool walkingBack = false;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.gameObject.GetComponent<Animator>();
        player = FindObjectOfType<PlayerGridObject>();
		canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;
		triggered = false;
	}
    // Update is called once per frame
    void Update () {
		if (!dialogue.activeSelf && activeRegionPreTrigger.bounds.Contains(player.transform.position) && triggered == false) {
            if (!calculatedDist)
            {
                moveDist = (int)System.Math.Round(System.Math.Abs(this.transform.position.y - player.transform.position.y) / .0315)-10;
                calculatedDist = true;
                //anim.SetBool("IsWalking", true);
                anim.SetInteger("Direction", 1); //walking down
            }
            if (moving)
            {
                player.canMove = false;
                player.animator.SetBool("IsWalking", false);
                Mover(Globals.Direction.South);
                counter++;
                if(counter > moveDist)
                {
                    anim.SetInteger("Direction", 0);    //walking up 
                    moving = false;
                    counter = 0;
                }
            }
            else
            {
                canvas.ShowDialog();
                talkedToPlayer = true;
                dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
                dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
                //canvas.paused = false;

                triggered = true;
            }
		}
        if (talkedToPlayer && !activeRegionPreTrigger.bounds.Contains(player.transform.position))
        {
            if (calculatedDist)
            {
                //anim.SetBool("IsWalking", true);
                anim.SetInteger("Direction", 2); //idle
                calculatedDist = false;
                movingBack = true;
            }
    
            Mover(Globals.Direction.North);
            counter++;
            if (counter > moveDist)
            {
                movingBack = false;
                //anim.SetInteger("Direction", 2);
                anim.SetInteger("Direction", 0);
                //anim.SetBool("IsWalking", false);
                talkedToPlayer = false;
                counter = 0;
            }
        }
        if (!dialogue.activeSelf && activeRegionPostTrigger.bounds.Contains(player.transform.position) && 
			(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))&& !movingBack) {
            canvas.ShowDialog();

			dialogue.GetComponentInChildren<DialogueSystem> ().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
			dialogue.GetComponentInChildren<DialogueSystem> ().LoadText ();
            //canvas.paused = false;
		}

    }
    public void Mover(Globals.Direction dir)
    {
        if (dir == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += Globals.pixelSize;
            this.transform.position = position;
        }
        else if (dir == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += Globals.pixelSize;
            this.transform.position = position;
        }
    }
}
