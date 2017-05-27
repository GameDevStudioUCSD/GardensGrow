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

    //private bool paused = false;
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
        //finalCreditsButton.SetActive(false);
        TileMap tm = FindObjectOfType<TileMap>();

        tm.player = this.gameObject;

        EnemySpawner[] es = FindObjectsOfType<EnemySpawner>();

        foreach (EnemySpawner e in es) e.gameObject.SetActive(false);

        EnemyGridObject[] eo = FindObjectsOfType<EnemyGridObject>();

        foreach (EnemyGridObject e in eo) e.gameObject.SetActive(false);

        HostileTerrainObject[] ho = FindObjectsOfType<HostileTerrainObject>();



        player.gameObject.SetActive(false);
        cutScene = true;
        StartCoroutine(MoveCameraPath());
    }
    IEnumerator MoveCameraPath()
    {
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
    }

    void Move(Globals.Direction direction)
    {
        if (direction == Globals.Direction.South)
        {
            Vector3 position = this.transform.position;
            position.y -= 2*Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.West)
        {
            Vector3 position = this.transform.position;
            position.x -= 2*Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.North)
        {
            Vector3 position = this.transform.position;
            position.y += 2*Globals.pixelSize;
            this.transform.position = position;
        }
        else if (direction == Globals.Direction.East)
        {
            Vector3 position = this.transform.position;
            position.x += 2*Globals.pixelSize;
            this.transform.position = position;
        }
    }
}
