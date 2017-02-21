using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

    // The TileMap grid will be mapDimension by mapDimension in size
    public int mapDimensionX, mapDimensionY;
    public PlayerGridObject player;

    // a grid to hold references to every Tile in the TileMap
    public Tile[,] grid;
    public Node[,] nodeGrid;

    public bool debug = false;
    private GameObject[] rooms;

   //start happens 1st frame
    void Start()
    {
        rooms = new GameObject[transform.GetChild(0).childCount];
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            rooms[i] = transform.GetChild(0).GetChild(i).gameObject;
        }

        int plantType;
    	Vector3 plantVector;
		foreach (KeyValuePair<Globals.PlantData, int> kvp in Globals.plants)
		{
			if (kvp.Key.PlantScene == Application.loadedLevelName) {
				plantVector = kvp.Key.PlantLocation;
				plantType = kvp.Value;
				PlantGridObject newPlant = (PlantGridObject) Instantiate(player.plants[plantType], plantVector, Quaternion.identity);
                newPlant.Rotate(kvp.Key.PlantDirection);
			}
		}

        grid = new Tile[mapDimensionX, mapDimensionY];

        // Get all Tiles that are children of this TileMap object
        Tile[] myTiles = GetComponentsInChildren<Tile>();

        // For each tile: get the tile's position with respect
        // to the tilemap's position.
        foreach (Tile tile in myTiles)
        {
            Vector3 tilePosition = tile.transform.position;

            // Tile's position with respect to the tilemap's position
            tilePosition -= this.transform.position;

            //Debug.Log(tilePosition);

            grid[(int)tilePosition.x, (int)tilePosition.y] = tile;

            //nodeGrid[(int)tilePosition.x, (int)tilePosition.y] = new Node(tile, this);
        }

	}	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < rooms.Length; i++) {
			float xDist = player.transform.position.x - rooms[i].transform.position.x;
			float yDist = player.transform.position.y - rooms[i].transform.position.y;
			if (xDist >= 21)
			{
				rooms[i].SetActive(debug);
			}
			else if (xDist < -21)
			{
				rooms[i].SetActive(debug);
			}
			else if (yDist >= 15)
			{
				rooms[i].SetActive(debug);
			}
			else if (yDist < -15)
			{
				rooms[i].SetActive(debug);
			}
			else
			{
				rooms[i].SetActive(true);
			}
		}
    }


    public Node NodeFromPosition(Vector2 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;

        if (x < 0 || x > mapDimensionX || y < 0 || y > mapDimensionY)
        {
            return null;
        }

        // TODO: we don't catch out of array index exceptions
        Tile tile = grid[x, y];

        return new Node(tile, this);
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

    public Node NextNode(Node currentNode, Globals.Direction direction)
    {
        Vector2 nextPosition = NextPosition(currentNode.gridPosition, direction);

        // Check to see if position is valid in grid
        if(IsPatheable(nextPosition))
        {
            Tile nextTile = grid[(int)nextPosition.x, (int)nextPosition.y];
            // Convert tile into Node
            Node nextNode = new Node(nextTile, this, currentNode, direction);
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
    public Vector2 NextPosition(Vector2 currentPosition, Globals.Direction direction)
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
    /// Get the tile that is in the given direction starting from the current tile.
    /// </summary>
    /// <param name="currentTile">Tile to start looking from.</param>
    /// <param name="direction">The direction to take to find the next tile.</param>
    public Tile NextTile(Tile currentTile, Globals.Direction direction)
    {
        return GetNearestTile(NextPosition(currentTile.transform.position, direction));
    }

    /// <summary>
    /// Check if a position in the TileMap is patheable or not.
    /// </summary>
    /// <param name="targetPosition">The position of the Tile to check.</param>
    /// <returns>True if the Tile is patheable, false if Tile is not patheable or it does not exist.</returns>
    public bool IsPatheable(Vector2 targetPosition)
    {
        int x = (int)targetPosition.x;
        int y = (int)targetPosition.y;

        if(grid[x, y] == null)
        {
            // throw new System.Exception("TileMap, IsPatheable(): could not find tile for vector: " + targetPosition + " at indices " + x + ", " + y);
            // Debug.LogError("TileMap, IsPatheable(): could not find tile for vector: " + targetPosition + " at indices " + x + ", " + y);
            return false;
        }
        else
        {
            return grid[x, y].isPatheable;
        }
    }

    /// <summary>
    /// Finds the closest Tile to a world position.
    /// </summary>
    /// <param name="worldPosition">World position from which we want to find the nearest tile.</param>
    /// <returns></returns>
    public Tile GetNearestTile(Vector2 worldPosition)
    {

        // Find the position relative to the tile map (Tiles are indexed by their positions relative to TileMap)
        Vector2 relativePosition = worldPosition - (Vector2)this.transform.position;

        int x = (int)Mathf.Round(relativePosition.x);
        int y = (int)Mathf.Round(relativePosition.y);

        if (grid[x, y] == null)
        {
            //throw new System.Exception("TileMap, GetNearestTile(): could not find tile for world vector: " + worldPosition + " at indices " + x + ", " + y);
            Debug.LogError("TileMap, GetNearestTile(): could not find tile for world vector: " + worldPosition + " at indices " + x + ", " + y);
            return null;
        }
        else
        {
            return grid[x, y];
        }
    }


}
