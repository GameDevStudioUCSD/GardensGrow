using UnityEngine;
using System.Collections;

public class DialogueNPCTrigger : MoveableGridObject {

	private UIController canvas;
	public string textFileName;
	public Collider2D activeRegionPreTrigger;
    public GameObject exclamationMark;

	public bool VolcanoBoss;
	public bool TornadoBoss;
	public bool JellyFishBoss;

	public string textFileNameAfterVolcano;
	public string textFileNameAfterTornado;
	public string textFileNameAfterJellyfish;

    
   /*NOTE: Ever time you place a new sign or npc make sure to change the saveNumber
    *      in the inspector to a number not yet used (check the top of globals.cs 
    *      for saveNumbers that's already been used)
    */
    public int saveNumber;
    private int loadedSlot = -1;

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

    public bool readAlready = false;

    private GameObject dialogue;
    private Animator anim;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        originalPosition = this.gameObject.transform.position;

        anim = this.gameObject.GetComponent<Animator>();
        player = FindObjectOfType<PlayerGridObject>();
		canvas = FindObjectOfType<UIController>();
		dialogue = canvas.dialogUI;

		if (VolcanoBoss && !Globals.lavaBossBeaten) {
			textFileName = textFileNameAfterVolcano;
		}
		if (TornadoBoss && !Globals.windBossBeaten) {
			textFileName = textFileNameAfterTornado;
		}
		if (JellyFishBoss && !Globals.caveBossBeaten) {
			textFileName = textFileNameAfterJellyfish;
		}
    }
    // Update is called once per frame
    protected override void Update () {
        base.Start();

        if (activeRegionPreTrigger.bounds.Contains(player.transform.position))
        {
            calculatedDistUp = false;
            if (!calculatedDistDown)
            {
                moveDistDown = (int)System.Math.Round(System.Math.Abs(this.transform.position.y - player.transform.position.y) / .0315) - 10;
                calculatedDistDown = true;
                anim.SetInteger("Direction", 1); //walking down
                isTalkingToPlayer = true;
                movingDown = true;
                canvas.ShowDialog();
                dialogue.GetComponentInChildren<DialogueSystem>().textFile = Resources.Load<TextAsset>("Text/" + textFileName);
                dialogue.GetComponentInChildren<DialogueSystem>().LoadText();
            }
            if (movingDown)
            {
                moveDistDown = (int)System.Math.Round(System.Math.Abs(this.transform.position.y - player.transform.position.y) / .0315) - 10;
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
                    if (exclamationMark)
                    {
                        exclamationMark.SetActive(false);
                    }
                    readAlready = true;
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

    public void saveBool(int saveSlot)
    {
        PlayerPrefsX.SetBool("npc" + saveNumber + "save" + saveSlot, readAlready);
    }
    public void OnEnable()
    {
        loadedSlot = Globals.loadedSlot;

        if(loadedSlot != -1)
        {
            readAlready = PlayerPrefsX.GetBool("npc" + saveNumber + "save" + loadedSlot);
        }

        if (readAlready)
        {
            exclamationMark.SetActive(false);
        }
        else
        {
            exclamationMark.SetActive(true);
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
