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

    // Instantiate new Node by copying an existing node
    public Node(Node otherNode)
    {
        isPatheable = otherNode.isPatheable;
        worldPosition = otherNode.worldPosition;
        gridPosition = otherNode.gridPosition;
        gCost = otherNode.gCost;
        hCost = otherNode.hCost;
    }

    public Node(Tile tile, TileMap tileMap, Node parent=null)
    {
        this.parent = parent;

        isPatheable = tile.isPatheable;
        worldPosition = tile.transform.position;
        gridPosition = tile.transform.position - tileMap.transform.position;
        gCost = tile.gCost;
    }

    public Node(Tile tile, TileMap tileMap, Node parent, Globals.Direction directionTaken)
    {
        this.parent = parent;
        this.directionTaken = directionTaken;

        isPatheable = tile.isPatheable;
        worldPosition = tile.transform.position;
        gridPosition = tile.transform.position - tileMap.transform.position;
        gCost = tile.gCost;
    }

    public Node(Node node, TileMap tileMap, Node parent, Globals.Direction directionTaken)
    {
        this.parent = parent;
        this.directionTaken = directionTaken;

        isPatheable = node.isPatheable;
        worldPosition = node.worldPosition;
        gridPosition = node.gridPosition;
        gCost = node.gCost;
    }

    public override string ToString()
    {
        //return base.ToString();
        string output = "Grid position: " + gridPosition + " World position: " + worldPosition + " gCost: " + gCost + " hCost: " + hCost;

        return output;
    }
}
