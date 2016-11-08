using UnityEngine;

/// <summary>
/// Node used for search.
///
/// The Node represents a Tile in the TileMap and contains all
/// of its information without being derived from the monobehaviour
/// class to improve efficiency.
/// 
/// Does not contain a reference to the Tile.
/// 
/// For searches use Node and Node derived classes.
/// </summary>
public class Node {

    // Parent node of this node
    public Node parent;
    // Direction taken to get to this node (usually from parent)
    public Globals.Direction directionTaken;

    // Information retrieved from Tile
    public bool isPatheable = true;
    public Vector2 worldPosition;
    public Vector2 gridPosition;
    public int gCost;
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(Tile tile, Node parent=null)
    {
        this.parent = parent;

        isPatheable = tile.IsOpen();
        worldPosition = tile.transform.position;
        gridPosition = tile.transform.localPosition;
        gCost = tile.gCost;
    }

    public Node(Tile tile, Node parent, Globals.Direction directionTaken)
    {
        this.parent = parent;
        this.directionTaken = directionTaken;

        isPatheable = tile.IsOpen();
        worldPosition = tile.transform.position;
        gridPosition = tile.transform.localPosition;
        gCost = tile.gCost;
    }

    public Node(Node other)
    {
        parent = other.parent;
        directionTaken = other.directionTaken;

        isPatheable = other.isPatheable;
        worldPosition = other.worldPosition;
        gridPosition = other.gridPosition;
        gCost = other.gCost;
        hCost = other.hCost;
    }

    public override string ToString()
    {
        //return base.ToString();
        string output = "Position: " + gridPosition + " gCost: " + gCost + " hCost: " + hCost;

        return output;
    }
}
