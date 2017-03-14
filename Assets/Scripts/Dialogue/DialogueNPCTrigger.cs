using UnityEngine;
using System.Collections;

public class DialogueNPCTrigger : MoveableGridObject {

	private UIController canvas;
	public string textFileName;
	public Collider2D activeRegionPreTrigger;
    public GameObject exclamationMark;
	//public Collider2D activeRegionPostTrigger;

    //moving stuff
	private PlayerGridObject player;
    private Vector3 originalPosition;

    private bool isTalkingToPlayer = false;

    private bool calculatedDistUp = false;
    private bool calculatedDistDown = false;

    private int moveDistUp; // |orig position - current position|
    private int moveDistDown; // |cur position - player position|
    private bool movingDown;
    private bool movingUp;

    private int upCounter = 0;
    private int downCounter = 0;


    private GameObject dialogue;
    private Animator anim;

	// Use this for initialization
	void Start () {
        originalPosition = this.gameObject.transform.position;

        anim = this.gameObject.GetComponent<Animator>();
        player = FindObjectOfType<PlayerGridObject>();
		canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;
	}
    // Update is called once per frame
    void Update () {
        if (activeRegionPreTrigger.bounds.Contains(player.transform.position))
        {
            calculatedDistUp = false;
            if (!calculatedDistDown)
            {
                moveDistDown = (int)System.Math.Round(System.Math.Abs(this.transform.position.y - player.transform.position.y) / .0315) - 10;
                calculatedDistDown = true;
                anim.SetInteger("Direction", 1); //walking down
                isTalkingToPlayer = true;
                exclamationMark.SetActive(true);
                movingDown = true;
                canvas.ShowDialog();
                dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
                dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
            }
            if (movingDown)
            {
                Mover(Globals.Direction.South);
                downCounter++;
                if (downCounter > moveDistDown)
                {
                    anim.SetInteger("Direction", 0);    //idle
                    movingDown = false;
                    downCounter = 0;
                }
            }
        }
        else if (!activeRegionPreTrigger.bounds.Contains(player.transform.position) && isTalkingToPlayer)
        {
            calculatedDistDown = false;
            if (!calculatedDistUp)
            {
                anim.SetInteger("Direction", 2); //walking up
                calculatedDistUp = true;
                moveDistUp = (int)System.Math.Round(System.Math.Abs(originalPosition.y - this.transform.position.y) / .0315) - 10;
                movingUp = true;
                canvas.EndDialog();
            }
            Mover(Globals.Direction.North);
            upCounter++;
            if (movingUp)
            {
                if (upCounter > moveDistUp)
                {
                    anim.SetInteger("Direction", 0);    //idle
                    upCounter = 0;
                    isTalkingToPlayer = false;
                    exclamationMark.SetActive(false);
                    movingUp = false;
                }
            }
         }
        if (!dialogue.activeSelf && activeRegionPreTrigger.bounds.Contains(player.transform.position) &&
             (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return)) && !movingUp)
        {
            canvas.ShowDialog();

            dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
            dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
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
