using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public GameObject PauseUI;
    public GameObject MainMenuUI;
    public GameObject LoadMenuUI;
    public bool lockCamera;

    public PlayerGridObject player;

    private bool paused = false;

    void Start()
    {
        if (PauseUI != null)
        {
            PauseUI.SetActive(false);
        }
    }
    void Update()
    {
        if (PauseUI != null)
        {
            if (paused)
            {
                PauseUI.SetActive(true);
                player.canMove = false;
                Time.timeScale = 0;
            }
            else if (!paused)
            {
                PauseUI.SetActive(false);
                player.canMove = true;
                Time.timeScale = 1;
            }
        }

        if (!lockCamera) {
	        float xDist = player.transform.position.x - this.gameObject.transform.position.x;
			float yDist = player.transform.position.y - this.gameObject.transform.position.y;

			Vector3 currentPos = this.gameObject.transform.position;
			if (xDist >= 7)
			{
				currentPos.x += 14;
			}
			if (xDist <= -7)
			{
				currentPos.x -= 14;
			}
			if (yDist >= 5)
			{
				currentPos.y += 10;
			}
			if (yDist <= -5)
			{
				currentPos.y -= 10;
			}

			this.transform.position = currentPos;
		}
    }
    public void LoadButton()
    {
        MainMenuUI.SetActive(false);
        LoadMenuUI.SetActive(true);
    }
    public void LoadBack()
    {
        MainMenuUI.SetActive(true);
        LoadMenuUI.SetActive(false);
    }
    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
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
        MainMenuUI.SetActive(false);
        Game newGame = new Game();
        
    }
    public void Save()
    {
        SaveLoad.Save();
    }
    public void LoadSlot1()
    {
        SaveLoad.Load(0);
    }
    public void LoadSlot2()
    {
        SaveLoad.Load(1);
    }
    public void LoadSlot3()
    {
        SaveLoad.Load(2);
    }
    public void LoadSlot4()
    {
        SaveLoad.Load(3);
    }

}
