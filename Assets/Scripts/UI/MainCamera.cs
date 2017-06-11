using UnityEngine;
using UnityEngine.UI;
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
    private float speed = 1;

    //credits stuff
    public Text title;
    public Text person;
    public GameObject creditsUI;
    public GameObject healthUI;
    public GameObject inventoryUI;
    public GameObject numbers;
    public GameObject pauseButton;
    public GameObject plantCounter;

    private AudioSource musicSource;
    public AudioClip song;

    private string[] titles = {"-Artist-", "-Game Developer-", "-Game Developer-", "-Lead Sound Designer-", "-Game Developer-", "-Artist-",
                               "-Lead Game Designer and Developer-", "-Music Composer-", "-Game Developer-", "-Artist-",
                               "-Special Thanks To-", "-Game Developer-", "-Lead Artist and Art Director-", "-Artist-", "-Artist-",
                               "-Artist-", "-Artist-", "-Game Developer-", "-Game Developer-", "-Artist-", "-Game Developer-", "-Game Tester-", "-Artist-",
                               "-Game Developer-", "-Artist-", "-Lead AI Developer-", "-Game Developer-", "-Level Designer-", "-Game Developer-"};
    private string[] persons = { "Alisa Ren", "Andrew Nguyen", "Anhquan Nguyen", "Christopher Loree", "Daniel Griffiths",
                                 "Delaney Carmen", "Eric Boming Cheng", "Erick Morales", "Frank Su", "Gavin Badillo",
                                 "Geoff Voelker", "Guyue Zhou", "Hilary Hg", "Jeremy Raab", "Joung Hun Suk", "Justin Lieu",
                                 "Kyle Lokken Henderson", "Mark Longsheng Zhao", "Michael Phalen", "Noel Nguyen", "Peter Vugia",
                                 "Sammy Jing Ma", "Sharon Mo", "Sher Zahed", "Siddarth Govindan", "Steven Lee",
                                 "Sung Rim Huh", "Tyler Bakke", "Yuanmin Zhang"};
    //private bool paused = false;

    private void Start()
    {
        tm = FindObjectOfType<TileMap>();

        if(Globals.startCredits == true)
        {
            cutScene = true;
        }

        if (cutScene)
        {
            this.gameObject.transform.position = new Vector3(0, -20, -2);
            tm.debug = true;
            creditsUI.SetActive(true);
            healthUI.SetActive(false);
            inventoryUI.SetActive(false);
            numbers.SetActive(false);
            pauseButton.SetActive(false);
            plantCounter.SetActive(false);

            Globals.startCredits = false;
            
        }
    }
    IEnumerator changeNames()
    {
        yield return new WaitForSeconds(5.13f);

        for(int i=0; i<persons.Length; i++)
        {
            title.text = titles[i];
            person.text = persons[i];
            yield return new WaitForSeconds(5.13f); // for 28 names in 159s
        }

        title.gameObject.SetActive(false);
        person.text = "GardensGrow";

        yield return new WaitForSeconds(10.26f);
        creditsUI.SetActive(false);
        
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
        musicSource = tm.GetComponent<AudioSource>();
        musicSource.clip = song;
        musicSource.Play();

        EnemySpawner[] es = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner e in es) e.gameObject.SetActive(false);

        PlantGridObject[] pl = FindObjectsOfType<PlantGridObject>();
        foreach (PlantGridObject p in pl) p.gameObject.SetActive(false);

        EnemyGridObject[] eo = FindObjectsOfType<EnemyGridObject>();
        foreach (EnemyGridObject e in eo) e.gameObject.SetActive(false);

        DialogueNPCTrigger[] dnt = FindObjectsOfType<DialogueNPCTrigger>();
        foreach (DialogueNPCTrigger d in dnt) d.gameObject.SetActive(false);

        DialogueTrigger[] dt = FindObjectsOfType<DialogueTrigger>();
        foreach (DialogueTrigger d in dt) d.gameObject.SetActive(false);

        //set health and iventory activefalse
        cutScene = true;

        Globals.Direction left = Globals.Direction.West;
        Globals.Direction right = Globals.Direction.East;
        Globals.Direction down = Globals.Direction.South;
        Globals.Direction up = Globals.Direction.North;

        Globals.Direction[] dirs = { up, up, right, right, up, right, down, down, right, up, right, right, left, up};
        StartCoroutine(MoveCameraPath1(dirs));
        StartCoroutine(changeNames());
    }

    IEnumerator MoveCameraPath1(Globals.Direction[] dir)
    {
        finalCreditsButton.SetActive(false);
        int dist;
        speed = 1.399f;
        Globals.Direction left = Globals.Direction.West;
        Globals.Direction right = Globals.Direction.East;
        Globals.Direction down = Globals.Direction.South;
        Globals.Direction up = Globals.Direction.North;

        for (int k = 0; k < dir.Length; k++)
        {
            dist = 228; //reset
            if (dir[k] == Globals.Direction.East || dir[k] == Globals.Direction.West)
            {
                dist = 323;
            }
            for (int i = 0; i < dist; i++)
            {
                this.Move(dir[k]);
                yield return new WaitForEndOfFrame();
            }

        }
        Globals.Direction[] dirs = {up, up, up, right, up, up, left, up, down, left};
        this.gameObject.transform.position = new Vector3(0, 0, -2f); //back to beginning
        StartCoroutine(MoveCameraPath2(dirs));
    }
    IEnumerator MoveCameraPath2(Globals.Direction[] dir)
    {
        int dist;
        Globals.Direction left = Globals.Direction.West;
        Globals.Direction right = Globals.Direction.East;
        Globals.Direction down = Globals.Direction.South;
        Globals.Direction up = Globals.Direction.North;

        for (int k = 0; k < dir.Length; k++)
        {
            dist = 228; //reset
            if (dir[k] == Globals.Direction.East || dir[k] == Globals.Direction.West)
            {
                dist = 323;
            }
            for (int i = 0; i < dist; i++)
            { 
                this.Move(dir[k]);
                yield return new WaitForEndOfFrame();
            }
        }
        Globals.Direction[] dirs = {left, left, up, left, left, down, down, right, right, up, right, right};
        this.gameObject.transform.position = new Vector3(0, 0, -2f); //back to beginning
        StartCoroutine(MoveCameraPath3(dirs));
    }
    IEnumerator MoveCameraPath3(Globals.Direction[] dir)
    {
        int dist;

        for (int k = 0; k < dir.Length; k++)
        {
            dist = 228; //reset
            if (dir[k] == Globals.Direction.East || dir[k] == Globals.Direction.West)
            {
                dist = 323;
            }
            for (int i = 0; i < dist; i++)
            {

                this.Move(dir[k]);
                yield return new WaitForEndOfFrame();
            }
        }

        //after credits should change song and reload all things that were set inactive to make playable
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
