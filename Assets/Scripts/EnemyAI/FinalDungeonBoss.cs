using UnityEngine;
using System.Collections;

public class FinalDungeonBoss : MonoBehaviour {

    //spawning information
    public GameObject[] tentacles;

    /* HOW TO BEAT FINAL BOSS
     * place certain plants to hit certain colored
     * tentacles until boss' hp reaches 0
     */

    /* Tentacle nums
     * 0    watermelon  red
     * 1    turbine     green
     * 2    cactus      yellow
     * 3    bomb        pink
     * 4    mushroom    black
     * 5    boomerang   blue
     * 6    spinning    teal
     */
    private Quaternion spawnRotationLeft = Quaternion.Euler(0, 0, 270f);
    private Quaternion spawnRotationRight = Quaternion.Euler(0, 0, 90f);
    private Quaternion spawnRotationUp = Quaternion.Euler(0, 0, 180f);
    private Quaternion spawnRotationDown = Quaternion.Euler(0, 0, 0f);


    //spawning locations
    private Vector3 spawnPositionNorth = new Vector3(0.0f, 8.0f, 0.0f);
    private Vector3 spawnPositionNorthLeft = new Vector3(-2.0f, 8.0f, 0.0f);
    private Vector3 spawnPositionNorthRight = new Vector3(2.0f, 8.0f, 0.0f);

    private Vector3 spawnPositionSouth = new Vector3(0.0f, -8.0f, 0.0f);
    private Vector3 spawnPositionSouthLeft = new Vector3(-2.0f, -8.0f, 0.0f);
    private Vector3 spawnPositionSouthRight = new Vector3(2.0f, -8.0f, 0.0f);

    private Vector3 spawnPositionEast = new Vector3(11.0f, 0.0f, 0.0f);
    private Vector3 spawnPositionEastLeft = new Vector3(11.0f, -2.0f, 0.0f);
    private Vector3 spawnPositionEastRight = new Vector3(11.0f, 2.0f, 0.0f);

    private Vector3 spawnPositionWest = new Vector3(-11.0f, 0.0f, 0.0f);
    private Vector3 spawnPositionWestLeft = new Vector3(-11.0f, -2.0f, 0.0f);
    private Vector3 spawnPositionWestRight = new Vector3(-11.0f, 2.0f, 0.0f);

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

    public bool spawnedMe = false;
	// Update is called once per frame
	void Update () {
        //moves tentacle
        if (spawnedActiveTentacle)
        {
            counter++;
            //under this condition pull tentacles back
            if (counter > 150 || touchedPlayer) //2
            {
                //destroy
                if (counter > 200)
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
            //move forward until the following conditions are false
            else if (!touchedPlayer && counter < 50) //tentacles are moving out
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
                currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionSouth, spawnRotationDown);
                if (hp < 11)
                {
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionSouthLeft, spawnRotationDown);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionSouthRight, spawnRotationDown);
                }

                currentTentacle.GetComponent<Tentacle>().realDir = Globals.Direction.North;
            }
            else if (currSituation == 1)
            {
                currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionNorth, spawnRotationUp);
                if (hp < 11)
                {
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionNorthLeft, spawnRotationUp);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionNorthRight, spawnRotationUp);
                }
                currentTentacle.GetComponent<Tentacle>().realDir = Globals.Direction.South;
            }
            else if (currSituation == 2)
            {
                currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEast, spawnRotationRight);
                if (hp < 11)
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEast, spawnRotationRight);
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEastLeft, spawnRotationRight);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionEastRight, spawnRotationRight);

                }
                currentTentacle.GetComponent<Tentacle>().realDir = Globals.Direction.West;
            }
            else if (currSituation == 3)
            {
                currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWest, spawnRotationLeft);
                if (hp < 11)
                {
                    currentTentacle = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWest, spawnRotationLeft);
                    currentTentacleLeft = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWestLeft, spawnRotationLeft);
                    currentTentacleRight = (GameObject)Instantiate(tentacles[currTentacleNum], spawnPositionWestRight, spawnRotationLeft);
                }
                currentTentacle.GetComponent<Tentacle>().realDir = Globals.Direction.East;
            }
            currentTentacle.GetComponent<Tentacle>().speed = 3.0f;
            currentTentacle.GetComponent<Tentacle>().tentacleNum = currTentacleNum;

            StartCoroutine(Rotate());



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

    IEnumerator Rotate()
    {
        int i = 0;
        while (currentTentacle)
        {
            if (i % 7 == 0)
            {
                currentTentacle.transform.Rotate(new Vector3(0, 180f, 0));
                if (hp < 11)
                {
                    currentTentacleLeft.transform.Rotate(new Vector3(0, 180f, 0));
                    currentTentacleRight.transform.Rotate(new Vector3(0, 180f, 0));
                }

            }
            yield return new WaitForEndOfFrame();
            i++;
        }
    }
}
