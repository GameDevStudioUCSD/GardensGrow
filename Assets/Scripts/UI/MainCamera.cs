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

        float playerXDist = player.transform.position.x - this.transform.position.x;
        float playerYDist = player.transform.position.y - this.transform.position.y;

		Vector3 position = this.gameObject.transform.position;
        if (playerXDist > 7) {
        	position.x += 14;
        	this.transform.position = position;
       	}
		if (playerXDist < -7) {
        	position.x -= 14;
        	this.transform.position = position;
       	}
		if (playerYDist > 5) {
        	position.y += 10;
        	this.transform.position = position;
       	}
		if (playerYDist < -5) {
        	position.y -= 10;
        	this.transform.position = position;
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
