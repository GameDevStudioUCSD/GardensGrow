using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public GameObject PauseUI;
    public PlayerGridObject player;

    private bool paused = false;

    void Start()
    {
        PauseUI.SetActive(false);
    }
    void Update()
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
    public void Pause()
    {
        paused = !paused;
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
        Application.LoadLevel(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
