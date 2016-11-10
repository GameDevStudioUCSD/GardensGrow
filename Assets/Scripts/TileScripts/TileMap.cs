using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

    // The TileMap grid will be mapDimension by mapDimension in size
    public int mapDimension;

    // a grid to hold references to every Tile in the TileMap
    public Tile[,] grid;

    // Awake happens before Start and is preferred for generating references between objects
    void Awake()
    {
        grid = new Tile[mapDimension, mapDimension];

        // Get all Tiles that are children of this TileMap object
        Tile[] myTiles = GetComponentsInChildren<Tile>();

        // For each Tile: get the Tile's local position to TileMap
        // and use it as index into the grid array
        foreach (Tile tile in myTiles)
        {
            Vector3 tilePosition = tile.transform.localPosition;

            grid[(int)tilePosition.x, (int)tilePosition.y] = tile;
        }

        Debug.Log("TILEMAP FINISH AWAKE");

	}

    // Use this for initialization
    void Start() {}

    
	
	// Update is called once per frame
	void Update () {

    }


    public Node NodeFromPosition(Vector2 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;

        if (x < 0 || x > mapDimension || y < 0 || y > mapDimension)
        {
            return null;
        }

        // TODO: we don't catch out of array index exceptions
        Tile tile = grid[x, y];

        return new Node(tile);
    }

    /// <summary>
    /// Get all Nodes reachable from the current node by taking any of the four Directions.
    /// </summary>
    /// <param name="currentNode">The node to get successors of.</param>
    /// <returns>A list of nodes representing the successor nodes.</returns>
    public List<Node> GetSuccessors(Node currentNode)
    {
        List<Node> successors = new List<Node>();

        foreach(Globals.Direction direction in System.Enum.GetValues(typeof(Globals.Direction)))
        {
            Node candidateNode = NextNode(currentNode, direction);

            if(candidateNode != null)
            {
                successors.Add(candidateNode);
            }
        }

        return successors;
    }

    Node NextNode(Node currentNode, Globals.Direction direction)
    {
        Vector2 nextPosition = NextPosition(currentNode.gridPosition, direction);

        // Check to see if position is valid in grid
        if(IsPatheable(nextPosition))
        {
            Tile nextTile = grid[(int)nextPosition.x, (int)nextPosition.y];
            // Convert tile into Node
            Node nextNode = new Node(nextTile, currentNode, direction);
            return nextNode;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Get the next position after taking a direction from the current position.
    /// </summary>
    /// <param name="currentPosition">Position before taking direction.</param>
    /// <param name="direction">Direction to take from current position.</param>
    /// <returns>The position after taking direction from current position.</returns>
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

    /// <summary>
    /// Check if a position in the TileMap is patheable or not.
    /// </summary>
    /// <param name="targetPosition">The position of the Tile to check.</param>
    /// <returns>True if the Tile is patheable, false if Tile is not patheable or it does not exist.</returns>
    bool IsPatheable(Vector2 targetPosition)
    {
        int x = (int)targetPosition.x;
        int y = (int)targetPosition.y;

        if (x < 0 || x > mapDimension || y < 0 || y > mapDimension)
        {
            return false;
        }

        return grid[x, y] != null ? grid[x, y].isPatheable : false;
    }

    /// <summary>
    /// Finds the closest Tile to a world position.
    /// </summary>
    /// <param name="worldPosition">Vector2 in world coordinates.</param>
    /// <returns></returns>
    public Tile GetNearestTile(Vector2 worldPosition)
    {
        Debug.Log("Does position match? " + worldPosition);
        float roundedX = Mathf.Round(worldPosition.x);
        float roundedY = Mathf.Round(worldPosition.y);

        // Find the position relative to the tile map (Tiles are indexed by their positions relative to TileMap)
        Vector3 localPosition = this.transform.InverseTransformPoint(roundedX, roundedY, 0.0f);
        Debug.Log("Find tile at local: " + localPosition);

        int x = (int)localPosition.x;
        int y = (int)localPosition.y;

        // Check if in bound
        if (x < 0 || x > mapDimension || y < 0 || y > mapDimension)
        {
            Debug.Log("tile out of bound");
            return null;
        }

        Debug.Log("Finding tile at grid: " + x + ", " + y);
        return grid[x, y];
    }
}
