using UnityEngine;
using System.Collections.Generic;

public class AStar {

    public TileMap tileMap;

    public Tile startTile;
    public Tile targetTile;

    public AStar(TileMap tileMap)
    {
        this.tileMap = tileMap;
    }

    public List<Globals.Direction> FindPath(Tile startTile, Tile targetTile)
    {
        return FindPath(new Node(startTile, tileMap), new Node(targetTile, tileMap));
    }

    public List<Globals.Direction> FindPath(Node startNode, Node targetNode)
    {
        // Result list of moves needed to get from startNode to targetNode
        List<Globals.Direction> moves = new List<Globals.Direction>();

        Vector2 startPosition = startNode.gridPosition;
        Vector2 targetPosition = targetNode.gridPosition;

        PriorityQueue<int, Vector2> frontier = new PriorityQueue<int, Vector2>();
        Dictionary<Vector2, Node> frontierNodes = new Dictionary<Vector2, Node>();

        HashSet<Vector2> explored = new HashSet<Vector2>();

        // Add the start node to openSet
        frontier.Enqueue(startNode.gridPosition, 0);
        frontierNodes.Add(startNode.gridPosition, startNode);

        while(!frontier.IsEmpty)
        {
            // Get node with lowest fCost from frontier
            Vector2 currentPosition = frontier.Dequeue();
            Node currentNode = frontierNodes[currentPosition];

            // Remove node with lowest fCost
            frontierNodes.Remove(currentPosition);

            // Remember not to expand this tile again
            explored.Add(currentPosition);

            // Check if we reached the goal
            if(currentPosition == targetPosition)
            {
                while(currentNode != startNode)
                {
                    moves.Add(currentNode.directionTaken);

                    currentNode = currentNode.parent;
                }
                moves.Reverse();
                return moves;
            }

            // Go through all successor nodes
            foreach(Node successorNode in tileMap.GetSuccessors(currentNode))
            {
                Vector2 successorPosition = successorNode.gridPosition;

                // Calculate the gCost for this successorNode using currentNode's gCost
                successorNode.gCost += currentNode.gCost;
                successorNode.hCost = heuristic(successorNode, targetNode);

                // If this position is not in frontierNodes or explored, then we can add it
                if (!frontierNodes.ContainsKey(successorPosition) && !explored.Contains(successorPosition))
                {
                    frontier.Enqueue(successorPosition, successorNode.fCost);
                    frontierNodes.Add(successorPosition, successorNode);
                }
                // If this position is in frontierNodes but the new node has a better value, then we replace it
                else if(frontierNodes.ContainsKey(successorPosition))
                {
                    Node frontierNode = frontierNodes[successorPosition];

                    if(successorNode.fCost < frontierNode.fCost)
                    {
                        // Update frontier with better costing node
                        frontier.Replace(successorPosition, frontierNode.fCost, successorNode.fCost);
                        frontierNodes[successorPosition] = successorNode;
                    }
                }

            }
        }

        return moves;
    }

    private int heuristic(Node currentNode, Node goalNode)
    {
        Vector2 currentPosition = currentNode.gridPosition;
        Vector2 goalPosition = goalNode.gridPosition;

        return heuristic(currentPosition, goalPosition);
    }
    private int heuristic(Vector2 currentPosition, Vector2 goalPosition)
    {
        return (int)Vector2.Distance(currentPosition, goalPosition);
    }
}
