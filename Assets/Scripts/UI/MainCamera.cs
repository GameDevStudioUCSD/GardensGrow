using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    /*public GameObject PauseUI;
    public GameObject MainMenuUI;
    public GameObject LoadMenuUI;*/
    public bool lockCamera;

    public PlayerGridObject player;

    //private bool paused = false;
    void Update()
    {
        if (!lockCamera) {
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

}
