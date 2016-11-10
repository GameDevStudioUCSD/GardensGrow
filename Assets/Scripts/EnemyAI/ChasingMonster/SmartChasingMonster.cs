using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Vision))]
public class SmartChasingMonster : SmartChasingMonsterAbstractFSM {
    [Header("DelayParameters")]
    public float idleDelay = 2;
    public float wanderingDelay = 3;
    [Range(0, 5)]
    public int speed = 2;
    Vision visionModule;

    [Header("PathFindingRequirements")]
    public TileMap tileMap;
    public GameObject targetObject;

    AStar astarAlgorithm;

    // Path found by astar
    private List<Globals.Direction> path;

    // Tile that the monster is on
    private Tile monsterTile;
    // Tile that the target object is standing on
    private Tile targetTile;


    protected override void Start()
    {
        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);
        base.Start();
    }
    public override void Reset() { }

    protected override bool CanSeePlayer()
    {
        return visionModule.CanSeePlayer(direction);
    }

    protected override IEnumerator ExecuteActionChasePlayer()
    {
        for (int i = 0; i < speed; i++ )
        {
            MoveEnemy(direction);
        }
        yield return null;
    }

    protected override IEnumerator ExecuteActionFollowPath()
    {
        Debug.Log("FollowPath");
        foreach (Globals.Direction item in path)
        {
            MoveTileLength(item);

            yield return null;
        }

    }

    protected override IEnumerator ExecuteActionPathFind()
    {
        Debug.Log("PathFind");
        Debug.Log("My position: " + this.transform.position);
        // Find where monster is on the tile map
        monsterTile = tileMap.GetNearestTile(this.transform.position);
        if (monsterTile == null)
            Debug.Log("Tile does not exist");
        else
            Debug.Log("Tile exists: " + monsterTile.transform.localPosition);

        // Find where monster has to go
        targetTile = tileMap.GetNearestTile(targetObject.transform.position);

        // Find a path
        path = astarAlgorithm.FindPath(monsterTile, targetTile);

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

    protected override void OnEnable()
    {
        base.OnEnable();
        visionModule = GetComponent<Vision>();
    }

    /// <summary>
    /// Continually calls EnemyMove until the monster has moved a Tile length.
    /// </summary>
    /// <param name="direction">Direction to move.</param>
    private void MoveTileLength(Globals.Direction dir)
    {
        MoveEnemy(dir);
    }
}
