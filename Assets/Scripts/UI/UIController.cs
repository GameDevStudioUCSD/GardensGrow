﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    public const int totalHearts = 6;

    public UnityEngine.UI.Image[] uiPlants = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] healthIcons;
    public UnityEngine.UI.Text[] uiPlantCounters;
    public UnityEngine.UI.Text[] uiButtonCounter;
    public UnityEngine.UI.Image uiKeyIcon;
    public UnityEngine.UI.Text uiKeyCounter;

    public GameObject pauseUI;
    public GameObject mainMenuUI;
    public GameObject loadMenuUI;
    public GameObject dialogUI;
    public GameObject saveMenuUI;
    public GameObject creditsUI;
    public GameObject deathPanelUI;
    public GameObject loadingScreenUI;

    public PlayerGridObject player;

    public Sprite[] seedPackets;
    public Sprite fullHeart;
    public Sprite brokenHeart;

    public bool paused;
    public bool dialogue = false;

    private CutsceneTrigger cutsceneTrigger;

    // Use this for initialization
    void Start () {
        UpdateUI();
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
    }

    void Update() {
        float yPos = 0f;
        //null check
        if (player)
        {
            yPos = ((player.transform.position.y + 5) % 10 + 10) % 10;
        }
		Image image;
		if (yPos < 2) {
			for (int i = 0; i < 8; i++) {
				image = uiPlants[i].GetComponent<Image>();
            	image.color = new Color(image.color.r, image.color.g, image.color.b, 0.2f);
        	}
		}
		else {
			for (int i = 0; i < 8; i++) {
				image = uiPlants[i].GetComponent<Image>();
            	image.color = new Color(image.color.r, image.color.g, image.color.b, 0.8f);
        	}
		}

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

        if (Globals.numKeys != 0) {
            uiKeyCounter.text = Globals.numKeys.ToString();
            uiKeyIcon.enabled = true;
        }
        else {
            uiKeyCounter.text = "";
            uiKeyIcon.enabled = false;
        }
    }

    public void UpdateHealth (int health) {
        for (int i = 0; i < totalHearts; i++) {
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

    public void UpdatePlantCooldown(bool canPlant) {
        Color tint;
        if (canPlant) {
            tint = new Color(1.0f, 1.0f, 1.0f);
        }
        else {
            tint = new Color(0.5f, 0.5f, 0.5f);
        }

        for (int i = 0; i < 8; i++) {
            uiPlants[i].GetComponent<Image>().color = tint;
        }
    }

    // Hides the plant UI when dialog is being said
    public void ShowDialog() {
        //player.GetComponent<Animator>().StartPlayback(); //don't know why start stops animations, but it does
        dialogue = true;
        dialogUI.SetActive(true);

        for (int i = 0; i < 8; i++) {
            uiPlants[i].enabled = false;
            uiPlantCounters[i].enabled = false;
            uiButtonCounter[i].enabled = false;
        }
    }

    // Hides the dialog box and enables the plant UI again
    public void EndDialog() {
        //player.GetComponent<Animator>().StopPlayback(); //don't know why stop starts animations, but it does
        dialogue = false;
        paused = false;
        dialogUI.SetActive(false);

        for (int i = 0; i < 8; i++) {
            uiPlants[i].enabled = true;
            uiPlantCounters[i].enabled = true;
            uiButtonCounter[i].enabled = true;
        }

        if (cutsceneTrigger) {
            cutsceneTrigger.FinishCutscene();
            cutsceneTrigger = null;
        }
    }
    
    public void ShowDialog(CutsceneTrigger trigger) {
        //player.GetComponent<Animator>().StartPlayback(); //don't know why start stops animations, but it does
        dialogue = true;
        dialogUI.SetActive(true);

        for (int i = 0; i < 8; i++) {
            uiPlants[i].enabled = false;
            uiPlantCounters[i].enabled = false;
            uiButtonCounter[i].enabled = false;
        }
        cutsceneTrigger = trigger;
    }

    public void ShowSaveMenu() {
        pauseUI.SetActive(false);
        saveMenuUI.SetActive(true);
    }

    public void HideSaveMenu() {
        pauseUI.SetActive(true);
        saveMenuUI.SetActive(false);
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

    public void Credits()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }
    
    public void CreditsBack()
    {
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        Globals.spawnLocation = new Vector3(0, -2f, 0);
        mainMenuUI.SetActive(false);
        loadingScreenUI.SetActive(true);
        Globals.loadedSlot = 4;
        PlayerPrefs.DeleteAll();
        Application.LoadLevel(1);

    }

    public void Save1()
    {
        Globals.SaveTheGame(1);
    }
    public void Save2()
    {
        Globals.SaveTheGame(2);
    }
    public void Save3()
    {
        Globals.SaveTheGame(3);
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
}
