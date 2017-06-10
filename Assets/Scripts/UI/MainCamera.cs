using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    /*public GameObject PauseUI;
    public GameObject MainMenuUI;
    public GameObject LoadMenuUI;*/
    public bool lockCamera;

    public bool cutScene = false;
    public GameObject finalCreditsButton;
    public PlayerGridObject player;

    private TileMap tm;
    private int speed = 1;

    //private bool paused = false;

    private void Start()
    {
        tm = FindObjectOfType<TileMap>();
        if (cutScene)
        {
            tm.debug = true;
            finalCreditsButton.SetActive(true);
        }
    }
    void Update()
    {
        if (!lockCamera && !cutScene) {
	        float xDist = player.transform.position.x - this.gameObject.transform.position.x;
			float yDist = player.transform.position.y - this.gameObject.transform.position.y;

			Vector3 currentPos = this.gameObject.transform.position;
			if (xDist >= 7)
			{
				currentPos.x += 14;
			}
			if (xDist < -7)
			{
				currentPos.x -= 14;
			}
			if (yDist >= 5)
			{
				currentPos.y += 10;
			}
			if (yDist < -5)
			{
				currentPos.y -= 10;
			}

			this.transform.position = currentPos;
		}
    }

    public void StartCutScene()
    {
        tm.player = this.gameObject;


        EnemySpawner[] es = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner e in es) e.gameObject.SetActive(false);

        EnemyGridObject[] eo = FindObjectsOfType<EnemyGridObject>();
        foreach (EnemyGridObject e in eo) e.gameObject.SetActive(false);

        DialogueNPCTrigger[] dnt = FindObjectsOfType<DialogueNPCTrigger>();
        foreach (DialogueNPCTrigger d in dnt) d.gameObject.SetActive(false);

        DialogueTrigger[] dt = FindObjectsOfType<DialogueTrigger>();
        foreach (DialogueTrigger d in dt) d.gameObject.SetActive(false);

        //set health and iventory activefalse
        cutScene = true;
        StartCoroutine(MoveCameraPath());
    }
    IEnumerator MoveCameraPath()
    {
        finalCreditsButton.SetActive(false);
        speed = 2;
        for (int i = 0; i < 440; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 160; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 230; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 320; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 230; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 160; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 225; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 160; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);

        speed = 5;

        for (int i = 0; i < 64; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 450; i++)
        {
            Move(Globals.Direction.West);
            yield return new WaitForEndOfFrame();
        }
        speed = 2;
        //start of exploration of world 2
        for (int i = 0; i < 480; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < 225; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < 320; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < 450; i++)
        {
            Move(Globals.Direction.West);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);
        //start traversing to world 3
        this.gameObject.transform.position = new Vector3(0, 0, -2f);
        for (int i = 0; i < 450; i++)
        {
            Move(Globals.Direction.West);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 160; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 450; i++)
        {
            Move(Globals.Direction.West);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 320; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 450; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 160; i++)
        {
            Move(Globals.Direction.North);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 450; i++)
        {
            Move(Globals.Direction.East);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.0f);
        //traverse world 4
        for (int i = 0; i < 320; i++)
        {
            Move(Globals.Direction.South);
            yield return new WaitForEndOfFrame();
        }
    }

    void Move(Globals.Direction direction)
    {
        if (direction == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= speed*Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= speed*Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += speed*Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += speed*Globals.pixelSize;
            this.transform.position = position;
        }
    }
}
