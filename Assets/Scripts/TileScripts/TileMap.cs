using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour
{
    // The TileMap grid will be mapDimension by mapDimension in size
    public int mapDimension;    

    // a grid to hold references to every Tile in the TileMap
    public Tile[,] grid;



    // Gets grid coordinates of tile current object is on
    public static Vector2 GetGridCoordinates(float x, float y)
    {
        return new Vector2((int)x, (int)y);

    }




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
            Vector3 tilePosition = tile.transform.position;

            grid[(int) tilePosition.x, (int) tilePosition.y] = tile;
        }
    }


    /// <summary>
    /// Returns the tile an object is standing on. Uses the (0.5, 0) of a 1x1 object which has pivot at center (0.5, 0.5). 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Tile GetTileCenteredOn(float x, float y)
    {
        x = Mathf.Round(x);
        y = Mathf.Round(y);
        if (x >= 0 && x < mapDimension && y >= 0 && y < mapDimension)
        {
            if (grid != null)
            {
                return grid[(int)x, (int)y];
            }
        }
        return null;
    }


    /// <summary>
    /// Returns the tile an object is standing on (and should be affected by the tile).
    /// </summary>
    /// <returns></returns>
    public Tile GetTileCenteredOn(Vector3 position)
    {
        return GetTileCenteredOn(position.x, position.y);
    }

    /// <summary>
    /// Returns the tile an object is standing on. Uses the (0.5, 0) of a 1x1 object which has pivot at center (0.5, 0.5). 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Tile GetTileStandingOn(float x, float y)
    {
        if (x >= -0.5f && y >= -0.2f)
        {
            x += 0.5f;
            if (x < mapDimension && y < mapDimension)
            {
                if (grid != null)
                {
                    return grid[(int) x, (int) y];
                }
            }
        }
        return null;
    }


    /// <summary>
    /// Returns the tile an object is standing on (and should be affected by the tile).
    /// </summary>
    /// <returns></returns>
    public Tile GetTileStandingOn(Vector3 position)
    {
        return GetTileStandingOn(position.x, position.y);
    }


    /// <summary>
    /// Returns the coordinates in the grid of the tile this point is at (==GetTileCenteredOn)
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2 GetGridCoordinates(Vector3 position)
    {
        return GetGridCoordinates(position.x, position.y);
    }

    // Use this for initialization
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
    }


    public Node NodeFromPosition(Vector2 position)
    {
        int x = (int) position.x;
        int y = (int) position.y;

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

        foreach (Globals.Direction direction in System.Enum.GetValues(typeof(Globals.Direction)))
        {
            Node candidateNode = NextNode(currentNode, direction);

            if (candidateNode != null)
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
        if (IsPatheable(nextPosition))
        {
            Tile nextTile = grid[(int) nextPosition.x, (int) nextPosition.y];
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
        int x = (int) currentPosition.x;
        int y = (int) currentPosition.y;
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
        return GetTileStandingOn(targetPosition).IsOpen();
    }
}