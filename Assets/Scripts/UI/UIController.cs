using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public UnityEngine.UI.Image[] uiPlants;
	public UnityEngine.UI.Image[] healthIcons;
	public UnityEngine.UI.Text[] uiPlantCounters;

	public GameObject pauseUI;
    public GameObject mainMenuUI;
    public GameObject loadMenuUI;
	public GameObject dialogUI;
	public PlayerGridObject player;

	public Sprite[] seedPackets;
	public Sprite fullHeart;
	public Sprite brokenHeart;

	public bool paused;

	// Use this for initialization
	void Start () {
		UpdateUI();
		if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
	}

	void Update() {
       
		if (pauseUI != null)
        {
            if (paused)
            {
                Time.timeScale = 0;
            }
            else if (!paused)
            {
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
        	if (paused) {
        		Resume();
        	}
        	else {
        		Pause();
        	}
        }
	}

	public void UpdateUI () {
		for (int i = 0; i < 8; i++) {
			uiPlantCounters[i].text = "";
			uiPlantCounters[i].color = new Color (1,1,1,1);
			if (Globals.unlockedSeeds[i] == true || Globals.inventory[i] > 0) {
				Globals.unlockedSeeds[i] = true;
				uiPlants[i].sprite = seedPackets[i];
				uiPlantCounters[i].text = Globals.inventory[i].ToString();
				if (Globals.inventory[i] == 0) {
					uiPlantCounters[i].color = new Color (0.92f, 0.42f, 0.01f, 1);
				}
				if (Globals.inventory[i] == 9) {
					uiPlantCounters[i].color = new Color (0.35f, 0.73f, 0.13f, 1);
				}
			}
		}
	}

	public void UpdateHealth (int health) {
		for (int i = 0; i < 6; i++) {
			if ((i + 1) * 2 <= health) {
				healthIcons[i].enabled = true;
				healthIcons[i].sprite = fullHeart;
			}
			else if ((i + 1) * 2 == health + 1) {
				healthIcons[i].enabled = true;
				healthIcons[i].sprite = brokenHeart;
			}
			else {
				healthIcons[i].enabled = false;
			}
		}
	}

	// Hides the plant UI when dialog is being said
	public void ShowDialog() {
        player.GetComponent<Animator>().StartPlayback(); //don't know why start stops animations, but it does
        paused = true;
		dialogUI.SetActive(true);

		for (int i = 0; i < 8; i++) {
			uiPlants[i].enabled = false;
			uiPlantCounters[i].enabled = false;
		}

		player.canMove = false;
	}

	// Hides the dialog box and enables the plant UI again
	public void EndDialog() {
        player.GetComponent<Animator>().StopPlayback(); //don't know why stop starts animations, but it does
        paused = false;
        dialogUI.SetActive(false);

		for (int i = 0; i < 8; i++) {
			uiPlants[i].enabled = true;
			uiPlantCounters[i].enabled = true;
		}

		player.canMove = true;
    }

	public void LoadButton()
    {
        mainMenuUI.SetActive(false);
        loadMenuUI.SetActive(true);
    }
    public void LoadBack()
    {
        mainMenuUI.SetActive(true);
        loadMenuUI.SetActive(false);
    }
    public void Pause()
    {
		pauseUI.SetActive(true);
        player.canMove = false;
        paused = true;
    }

    public void Resume()
    {
		pauseUI.SetActive(false);
        player.canMove = true;
        paused = false;
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        mainMenuUI.SetActive(false);
        Application.LoadLevel(1);

    }
    public void Save()
    {
        /* autosaves to 1st loadslot
         * please change UI and logic
         * to give the option to save in 3 
         * different slots
         */
        Globals.SaveTheGame(1);
    }
    public void LoadSlot1()
    {
        if(Globals.LoadTheGame(1) == 1)
        {
            loadMenuUI.SetActive(false);
        }
    }
    public void LoadSlot2()
    {
        if (Globals.LoadTheGame(2) == 1)
        {
            loadMenuUI.SetActive(false);
        }
    }
    public void LoadSlot3()
    {
        if (Globals.LoadTheGame(3) == 1)
        {
            loadMenuUI.SetActive(false);
        }
    }
    public void LoadSlot4() //no UI for 4th load slot maybe take out this method?
    {
        if (Globals.LoadTheGame(4) == 1)
        {
            loadMenuUI.SetActive(false);
        }
    }

}
