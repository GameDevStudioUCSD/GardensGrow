using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Vision))]
public class SmartChasingMonster : SmartChasingMonsterAbstractFSM
{
    [Header("DelayParameters")]
    public float idleDelay = 2.0f;
    public float wanderingDelay = 3.0f;
    public float pathingDelay = 1.0f;
    [Range(0, 5)]
    public int speed = 2;
    Vision visionModule;

    [Header("PathFindingRequirements")]
    public TileMap tileMap;
    public GameObject targetObject;

    [Header("Status")]
    public bool isDisabled;

    AStar astarAlgorithm;

    // Path found by astar
    private List<Globals.Direction> path;
    private int currentPathIndex = 0;

    // Tile that the monster is on
    private Tile monsterTile;
    // Tile that the target object is standing on
    private Tile targetTile;

    // Does the path need to be re-evaluated
    private bool needsReevaluation;

    // Use this for initialization
    protected override void Start()
    {
        isDisabled = false;
        needsReevaluation = false;
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);
        visionModule = GetComponent<Vision>();
        base.Start();
    }

    public override void Reset() { }

    protected override bool CanSeePlayer()
    {
        return visionModule.CanSeePlayer(direction);
    }

    protected override bool Disabled()
    {
        return isDisabled;
    }

    protected override IEnumerator ExecuteActionChasePlayer()
    {
        for(int i = 0; i < speed; i++)
        {
            MoveEnemy(direction);
        }
        yield return null;
    }

    protected override IEnumerator ExecuteActionDisabled()
    {
        yield return null;
    }

    /// <summary>
    /// Check if our path is still valid.
    /// The path might not be valid anymore if the target has moved
    /// or target is dead.
    /// </summary>
    protected override IEnumerator ExecuteActionEvaluatePath()
    {
        // Check if the target is still alive
        if(targetObject)
        {
            // Check if the target is still on the tile we last found a path to
            Tile currTargetTile = tileMap.GetNearestTile(targetObject.transform.position);
            if(currTargetTile == targetTile)
            {
                needsReevaluation = false;

                // Next step in the path
                currentPathIndex++;
            }
            else
            {
                needsReevaluation = true;
            }
        }
        else
        {
            // target is dead
            needsReevaluation = true;
        }

        yield return null;
    }

    /// <summary>
    /// Find a path from the monster to the target.
    /// </summary>
    protected override IEnumerator ExecuteActionPathFind()
    {
        // If the target still exists, find a path toward it
        if(targetObject)
        {
            // Find where monster is on the tile map
            monsterTile = tileMap.GetNearestTile(this.transform.position);

            // Find where monster has to go
            targetTile = tileMap.GetNearestTile(targetObject.transform.position);

            // Find a path
            path = astarAlgorithm.FindPath(monsterTile, targetTile);

            // We are on the first step of the path
            currentPathIndex = 0;
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionRandomizeDirection()
    {
        direction = (Globals.Direction)UnityEngine.Random.Range(0, 4);
        yield return null;
    }

    /// <summary>
    /// The monster takes a step on the path to the player.
    /// </summary>
    protected override IEnumerator ExecuteActionTakeStep()
    {
        for(int i = 0; i < speed; i++)
        {
            MoveEnemy(path[currentPathIndex]);
        }
        yield return null;
    }

    protected override IEnumerator ExecuteActionWander()
    {
        MoveEnemy(direction);
        yield return null;
    }

    /// <summary>
    /// The path is the list of directions from start to target.
    /// The path is finished if we took all steps or the path
    /// needs to be re-evaluated due to player dying/moving.
    /// </summary>
    /// <returns>Are we done with the current path?</returns>
    protected override bool FinishedPath()
    {
        return currentPathIndex >= path.Count || needsReevaluation;
    }

    /// <summary>
    /// A step is one direction in the list of directions for a path.
    /// This check adds a slight delay before the next step is taken.
    /// </summary>
    /// <returns>Has enough time passed for the delay?</returns>
    protected override bool FinishedStep()
    {
        return TimeInState() > pathingDelay;
    }

    protected override bool FinishedWandering()
    {
        return TimeInState() > wanderingDelay;
    }

}
