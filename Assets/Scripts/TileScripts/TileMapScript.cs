using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapScript : MonoBehaviour {

    public int mapDimension = 10;
    public TileScript[,] grid;

	// Use this for initialization
	void Start () {
        grid = new TileScript[mapDimension, mapDimension];

        TileScript[] myTileScript = GetComponentsInChildren<TileScript>();

        foreach (TileScript tileScript in myTileScript)
        {
            Vector3 tilePosition = tileScript.transform.position;
            Debug.Log("x: " + tilePosition.x + "y: " + tilePosition.y);
            grid[(int)tilePosition.x, (int)tilePosition.y] = tileScript;
        }

	}
	
	// Update is called once per frame
	void Update () {

    }

    public List<ActionState> GetSuccessor(Vector2 currentPosition)
    {
        int x = (int)currentPosition.x;
        int y = (int)currentPosition.y;

        List<ActionState> successors = new List<ActionState>();

        foreach(Globals.Direction direction in System.Enum.GetValues(typeof(Globals.Direction)))
        {
            Vector2 candidatePosition = NextPosition(currentPosition, direction);

            if(IsPatheable(candidatePosition))
            {
                successors.Add(new ActionState(candidatePosition, direction));
            }
        }

        return successors;
    }

    Vector2 NextPosition(Vector2 currentPosition, Globals.Direction direction)
    {
        int x = (int)currentPosition.x;
        int y = (int)currentPosition.y;
        switch (direction)
        {
            case Globals.Direction.North:
                y += 1;
                break;
            case Globals.Direction.East:
                x += 1;
                break;
            case Globals.Direction.South:
                y -= 1;
                break;
            case Globals.Direction.West:
                x -= 1;
                break;
        }

        return new Vector2(x, y);
    }

    bool IsPatheable(Vector2 targetPosition)
    {
        int x = (int)targetPosition.x;
        int y = (int)targetPosition.y;

        if (x < 0 || x > mapDimension || y < 0 || y > mapDimension)
        {
            return false;
        }

        return grid[x, y] != null ? true : false;
    }
}
