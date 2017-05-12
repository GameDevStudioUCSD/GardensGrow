using UnityEngine;
using System.Collections;

public class FinalDungeonBoss : MonoBehaviour {

    //spawning information
    public GameObject[] tentacles;

    /* Tentacle nums
     * 0    watermelon  red
     * 1    turbine     green
     * 2    cactus      yellow
     * 3    bomb        pink
     * 4    mushroom    black
     * 5    boomerang   blue
     * 6    spinning    teal
     */
    private Quaternion spawnRotationHorizontal= Quaternion.Euler(0, 0, 90.0f);
    private Quaternion spawnRotationVertical = Quaternion.Euler(0, 0, 0f);

    //spawning locations
    private Vector3 spawnPositionNorth = new Vector3(0.0f, 10.0f, 0.0f);
    private Vector3 spawnPositionNorthLeft = new Vector3(-2.0f, 10.0f, 0.0f);
    private Vector3 spawnPositionNorthRight = new Vector3(2.0f, 10.0f, 0.0f);

    private Vector3 spawnPositionSouth = new Vector3(0.0f, -8.0f, 0.0f);
    private Vector3 spawnPositionSouthLeft = new Vector3(-2.0f, -8.0f, 0.0f);
    private Vector3 spawnPositionSouthRight = new Vector3(2.0f, -8.0f, 0.0f);

    private Vector3 spawnPositionEast = new Vector3(8.0f, -2.0f, 0.0f);
    private Vector3 spawnPositionEastLeft = new Vector3(8.0f, -4.0f, 0.0f);
    private Vector3 spawnPositionEastRight = new Vector3(8.0f, 0.0f, 0.0f);

    private Vector3 spawnPositionWest = new Vector3(-15.0f, -2.0f, 0.0f);
    private Vector3 spawnPositionWestLeft = new Vector3(-15.0f, -4.0f, 0.0f);
    private Vector3 spawnPositionWestRight = new Vector3(-15.0f, 0.0f, 0.0f);

    //booleans to control boss state
    public int hp = 15;
    private int lvl = 1;
    private GameObject currentTentacle;
    private GameObject currentTentacleLeft;
    private GameObject currentTentacleRight;

    private int currTentacleNum;
    private int currSituation;
    private bool spawnedActiveTentacle = false;
    private int counter = 0;
    public bool touchedPlayer = false;
	// Update is called once per frame
	void Update () {

        if (spawnedActiveTentacle)
        {
            counter++;
            if (counter > 170 || touchedPlayer) //2
            {
                if (counter > 250)
                {
                    Destroy(currentTentacle.gameObject);
                    if (currentTentacleLeft)
                    {
                        Destroy(currentTentacleLeft);
                    }
                    if (currentTentacleRight)
                    {
                        Destroy(currentTentacleRight);
                    }
                    spawnedActiveTentacle = false;
                    touchedPlayer = false;
                    counter = 0;
                }
                /*tentacles are moving back*/
                if (currSituation == 0)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.South);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.South);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.South);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.South);
                    }
                }
                else if (currSituation == 1)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.North);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.North);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.North);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.North);
                    }
                }
                else if (currSituation == 2)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.East);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.East);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.East);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.East);
                    }
                }
                else if (currSituation == 3)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.West);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.West);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.West);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.West);
                    }
                }

            }
            else if (!touchedPlayer && counter < 70) //tentacles are moving out
            {
                if (currSituation == 0)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.North);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.North);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.North);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.North);
                    }
                }
                else if (currSituation == 1)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.South);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.South);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.South);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.South);
                    }
                }
                else if (currSituation == 2)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.West);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.West);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.West);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.West);
                    }
                }
                else if (currSituation == 3)
                {
                    if (hp < 11)
                    {
                        if (currentTentacleLeft || currentTentacleRight)
                        {
                            currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.East);
                            currentTentacleLeft.GetComponent<Tentacle>().Move(Globals.Direction.East);
                            currentTentacleRight.GetComponent<Tentacle>().Move(Globals.Direction.East);
                        }
                    }
                    else
                    {
                        currentTentacle.GetComponent<Tentacle>().Move(Globals.Direction.East);
                    }
                }
            }
        }
        else
        {
            currSituation = Random.Range(0, 4);
            currTentacleNum = Random.Range(0, 6);

            if (currSituation == 0)
            {
                if (hp < 11)
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionSouth, spawnRotationVertical);
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionSouthLeft, spawnRotationVertical);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionSouthRight, spawnRotationVertical);
                }
                else
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionSouth, spawnRotationVertical);
                }
            }
            else if (currSituation == 1)
            {
                if (hp < 11)
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionNorth, spawnRotationVertical);
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionNorthLeft, spawnRotationVertical);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionNorthRight, spawnRotationVertical);
                }
                else
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionNorth, spawnRotationVertical);
                }
            }
            else if (currSituation == 2)
            {
                if (hp < 11)
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEast, spawnRotationHorizontal);
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEastLeft, spawnRotationHorizontal);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEastRight, spawnRotationHorizontal);

                }
                else
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEast, spawnRotationHorizontal);
                }
            }
            else if (currSituation == 3)
            {
                if (hp < 11)
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWest, spawnRotationHorizontal);
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWestLeft, spawnRotationHorizontal);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWestRight, spawnRotationHorizontal);
                }
                else
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWest, spawnRotationHorizontal);
                }
            }
            currentTentacle.GetComponent<Tentacle>().speed = 3.0f;
            currentTentacle.GetComponent<Tentacle>().tentacleNum = currTentacleNum;

            if (currentTentacleLeft)
            {
                currentTentacleLeft.GetComponent<Tentacle>().speed = 3.0f;
                currentTentacleLeft.GetComponent<Tentacle>().tentacleNum = currTentacleNum;
            }
            if (currentTentacleRight)
            {
                currentTentacleRight.GetComponent<Tentacle>().speed = 3.0f;
                currentTentacleRight.GetComponent<Tentacle>().tentacleNum = currTentacleNum;
            }

            spawnedActiveTentacle = true;

        }
	}
}
