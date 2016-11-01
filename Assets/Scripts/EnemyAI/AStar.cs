using UnityEngine;
using System.Collections.Generic;

public class AStar {

    public TileMap tileMap;

    List<Globals.Direction> aStarAlgorithm(Vector2 currentPosition, Vector2 targetPosition)
    {
        List<Globals.Direction> path = new List<Globals.Direction>();
        // Hold nodes we are currently looking at
        PriorityQueue<int, Vector2> frontier = new PriorityQueue<int, Vector2>();
        Dictionary<Vector2, Node> inPriorityQueue = new Dictionary<Vector2, Node>();

        // "Nodes" we have already seen
        HashSet<Vector2> explored = new HashSet<Vector2>();

        // Insert current node into PQ
        Node startNode = new Node(currentPosition, null);
        frontier.Enqueue(currentPosition, 0);
        inPriorityQueue.Add(currentPosition, startNode);

        while (!frontier.IsEmpty)
        {
            Vector2 currentNodePosition = frontier.Dequeue();
            Node currentNode = null;
            inPriorityQueue.TryGetValue(currentNodePosition, out currentNode);

            if (currentNode.position == targetPosition)
            {
                // Goal state found
                while (currentNode.parent != null)
                {
                    path.Add(Globals.VectorsToDirection(currentNode.parent.position, currentNode.position));
                    currentNode = currentNode.parent;
                }

                path.Reverse();

                return path;
            }

            int pathCost = currentNode.pathCost;

            // Don't look at this state again
            explored.Add(currentNode.position);

            // Get successor nodes
            List<Node> successorNodes = tileMap.GetSuccessors(currentNode);
            foreach (Node successorNode in successorNodes)
            {
                Vector2 successorPosition = successorNode.position;

                // TODO: we are currently using 1 as our path costs but change it later
                int newPathCost = pathCost + 1;
                int newFCost = newPathCost + heuristic(successorNode.position, targetPosition);

                if (!inPriorityQueue.ContainsKey(successorPosition) && !explored.Contains(successorPosition))
                {
                    successorNode.pathCost = newPathCost;
                    successorNode.fCost = newFCost;
                    successorNode.parent = currentNode;
                    // Push successor into PQ
                    frontier.Enqueue(successorPosition, newFCost);
                    inPriorityQueue.Add(successorPosition, successorNode);

                    // TODO: Add to explored
                    explored.Add(successorPosition);
                }
                else if (inPriorityQueue.ContainsKey(successorPosition))
                {
                    // The node currently in the frontier
                    Node frontierNode = null;
                    inPriorityQueue.TryGetValue(successorPosition, out frontierNode);

                    // Compare current node's cost with new cost
                    if (newFCost < frontierNode.fCost)
                    {
                        frontier.Replace(successorPosition, frontierNode.fCost, newFCost);
                        inPriorityQueue[successorPosition] = successorNode;
                    }
                }
            }
        } // while

        return null;
    }

    int heuristic(Vector2 currentPosition, Vector2 goalPosition)
    {
        return (int)Vector2.Distance(currentPosition, goalPosition);
    }

    int pathCost(Vector2 currentPosition, Vector2 startPosition)
    {
        // TODO:
        return 0;
    }
}
