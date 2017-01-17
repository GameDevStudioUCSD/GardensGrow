using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

    // The TileMap grid will be mapDimension by mapDimension in size
    public int mapDimensionX, mapDimensionY;
    public PlayerGridObject player;

    // a grid to hold references to every Tile in the TileMap
    public Tile[,] grid;

    public bool debug = false;
    private GameObject[] rooms;

    // Awake happens before Start and is preferred for generating references between objects
    void Awake()
    {
    	int plantType;
    	Vector3 plantVector;
    	for (int i = 0; i < Globals.plantedListTypes.Count; i++)
    	{
    		if (Globals.plantedListScenes[i] == Application.loadedLevelName)
    		{
	    		plantType = Globals.plantedListTypes[i];
	    		plantVector = Globals.plantedListVectors[i];
	    		Instantiate(player.plants[plantType], plantVector, Quaternion.identity);
    		}
    	}

        grid = new Tile[mapDimensionX, mapDimensionY];

        // Get all Tiles that are children of this TileMap object
        Tile[] myTiles = GetComponentsInChildren<Tile>();

        // For each Tile: get the Tile's local position to TileMap
        // and use it as index into the grid array
        foreach (Tile tile in myTiles)
        {
            Vector3 tilePosition = tile.transform.position;
            tilePosition -= this.transform.position;

            grid[(int)tilePosition.x, (int)tilePosition.y] = tile;
        }

	}

    // Use this for initialization
    void Start() {
    	rooms = new GameObject[transform.GetChild(0).childCount];
       	for (int i = 0; i < transform.GetChild(0).childCount; i++) {
			rooms[i] = transform.GetChild(0).GetChild(i).gameObject;
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

    Node NextNode(Node currentNode, Globals.Direction direction)
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

        if (x < 0 || x > mapDimensionX || y < 0 || y > mapDimensionY)
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

        // Find the position relative to the tile map (Tiles are indexed by their positions relative to TileMap)
        Vector2 relativePosition = worldPosition - (Vector2)this.transform.position;

        int x = (int)Mathf.Round(relativePosition.x);
        int y = (int)Mathf.Round(relativePosition.y);

        if (grid[x, y] == null) Debug.Log("ERROR: Finding nearest tile outside of TileMap");

        // Check if in bound
        if (x < 0 || x > mapDimensionX || y < 0 || y > mapDimensionY)
        {
            return null;
        }

        return grid[x, y];
    }


}
