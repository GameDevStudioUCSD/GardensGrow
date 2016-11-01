using UnityEngine;
using System.Collections;

public class NodeB
{

    public bool walkable;
    public Vector2 gridPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public NodeB(bool walkable, Vector2 worldPos, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.gridPosition = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
