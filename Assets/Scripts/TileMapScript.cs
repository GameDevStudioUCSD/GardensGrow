using UnityEngine;
using System.Collections;

public class TileMapScript : MonoBehaviour {

    public TileScript[,] grid = new TileScript[10,10];

	// Use this for initialization
	void Start () {
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

    bool GetSuccessor(Vector2 currentPosition, Globals.Direction direction)
    {
        int x = (int)currentPosition.x;
        int y = (int)currentPosition.y;
        switch(direction)
        {
            case Globals.Direction.North :
                y += 1;
                break;
            case Globals.Direction.East :
                x += 1;
                break;
            case Globals.Direction.South :
                y -= 1;
                break;
            case Globals.Direction.West :
                x -= 1;
                break;
        }

        if(x < 0 || x > 10 || y < 0 || y > 10)
        {
            return false;
        }

        return grid[x, y] != null ? true : false;
    }
}
