using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Vision))]
public class SmartChasingMonster : SmartChasingMonsterAbstractFSM {
    [Header("DelayParameters")]
    public float idleDelay = 2;
    public float wanderingDelay = 3;
    public float pathingDelay = 0.1f;
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


    // Use this for initialization
    protected override void Start () {
        isDisabled = false;
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);
        base.Start();	
	}

    public override void Reset() { }

    protected override bool CanSeePlayer()
    {
        return visionModule.CanSeePlayer(direction);
    }

    protected override bool DelayFinished()
    {
        return TimeInState() > pathingDelay;
    }

    protected override bool Disabled()
    {
        return isDisabled;
    }

    protected override IEnumerator ExecuteActionChasePlayer()
    {
        for (int i = 0; i < speed; i++)
        {
            MoveEnemy(direction);
        }
        yield return null;
    }

    protected override IEnumerator ExecuteActionDisabled()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteActionFollowPath()
    {
        if (path.Count > 0 && currentPathIndex < path.Count)
        {
            for (int i = 0; i < speed; i++)
            {
                MoveEnemy(path[0]);
            }
        }
        // TODO: this doesn't work but keep it for right now
        currentPathIndex++;
        yield return null;
    }

    protected override IEnumerator ExecuteActionPathFind()
    {
        // Find where monster is on the tile map
        monsterTile = tileMap.GetNearestTile(this.transform.position);

        // Find where monster has to go
        targetTile = tileMap.GetNearestTile(targetObject.transform.position);

        // Find a path
        path = astarAlgorithm.FindPath(monsterTile, targetTile);

        // We are on the first path
        currentPathIndex = 0;

        yield return null;
    }

    protected override IEnumerator ExecuteActionPathingDelay()
    {
        yield return null;
    }

    protected override IEnumerator ExecuteActionRandomizeDirection()
    {
        direction = (Globals.Direction)UnityEngine.Random.Range(0, 4);
        yield return null;
    }

    protected override IEnumerator ExecuteActionWander()
    {
        MoveEnemy(direction);
        yield return null;
    }

    protected override bool FinishedWandering()
    {
        return TimeInState() > wanderingDelay;
    }

    protected override bool PathFinished()
    {
        return currentPathIndex >= path.Count;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        visionModule = GetComponent<Vision>();
    }
}
