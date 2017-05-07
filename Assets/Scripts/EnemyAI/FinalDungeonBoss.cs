using UnityEngine;
using System.Collections;

public class FinalDungeonBoss : MonoBehaviour {

    //spawning information
    public GameObject tentacle;
    private Quaternion spawnRotationHorizontal = Quaternion.Euler(0, 0, 90.0f);
    private Quaternion spawnRotationVertical = Quaternion.Euler(0, 0, 0f);

    //spawning locations
    private Vector3 spawnPositionNorth = new Vector3(0.0f, 10.0f, 0.0f);
    private Vector3 spawnPositionSouth = new Vector3(0.0f, -8.0f, 0.0f);
    private Vector3 spawnPositionEast = new Vector3(8.0f, -2.0f, 0.0f);
    private Vector3 spawnPositionWest = new Vector3(-15.0f, -2.0f, 0.0f);

    //booleans to control boss state
    private GameObject currentTentacle;
    private int currSituation;
    private bool spawnedActiveTentacle = false;
    private int counter = 0;

	// Update is called once per frame
	void Update () {

        if (spawnedActiveTentacle)
        {
            counter++;
            if (counter > 80)
            {
                if(counter > 250)
                {
                    Destroy(currentTentacle.gameObject);
                    spawnedActiveTentacle = false;
                    counter = 0;
                }
                if (currSituation == 0)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.South);
                }
                else if (currSituation == 1)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.North);
                }
                else if (currSituation == 2)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.East);
                }
                else if (currSituation == 3)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.West);
                }
            }
            else
            {
                if (currSituation == 0)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.North);
                }
                else if (currSituation == 1)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.South);
                }
                else if (currSituation == 2)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.West);
                }
                else if (currSituation == 3)
                {
                    currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.East);
                }
            }
        }
        else
        {
            currSituation = Random.Range(0, 3);
            if (currSituation == 0)
            {
                currentTentacle = (GameObject)Instantiate(tentacle, spawnPositionSouth, spawnRotationVertical);
            }
            else if(currSituation == 1)
            {
                currentTentacle = (GameObject)Instantiate(tentacle, spawnPositionNorth, spawnRotationVertical);
            }
            else if(currSituation == 2)
            {
                currentTentacle = (GameObject)Instantiate(tentacle, spawnPositionEast, spawnRotationHorizontal);
            }
            else if(currSituation == 3)
            {
                currentTentacle = (GameObject)Instantiate(tentacle, spawnPositionWest, spawnRotationHorizontal);
            }
            currentTentacle.GetComponent<Tentacle>().speed = 3.0f;
            spawnedActiveTentacle = true;

        }
	}
}
