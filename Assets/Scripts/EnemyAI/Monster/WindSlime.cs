using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Vision))]
public class WindSlime : PathFindingMonsterAbstractFSM {
    private PlayerGridObject player;

    private int dir2;

    private int counter = 0;
    public int turnDelay;
    private int maxSpinCounter = 0;
    public int maxSpin;
    private bool canSpin = false;

    public GameObject littleSlime;

    public float spawnDelay;

    private Vector3 spawnPosition;
    public List<GameObject> list = new List<GameObject>();

    private Quaternion spawnRotation = Quaternion.identity;
    System.Random randGen = new System.Random();
    private int randInt;
    private int currentSpawns = 0;
    public int maxSpawn;

    [Header("Required Components")]
    public TileMap tileMap;
    public GameObject targetObject;

    [Header("Pathfinding Parameters")]
    [Tooltip("Number of move calls per step.")]
    [Range(1, 64)]
    public int moveAmount = 32;
    [Tooltip("Delay between each step.")]
    public float stepDelay = 1.0f;
    [Tooltip("The allowed distance error from the center of a tile when moving from tile to tile")]
    public float allowedOffset = 0.50f;

    public bool debug = false;

    protected Vision visionModule;

    protected AStar astarAlgorithm;

    // Path found by astar
    protected List<Globals.Direction> path;
    [SerializeField]
    protected int currentPathIndex = 0;

    // Data about the path the monster is on
    [SerializeField]
    protected Tile startTile;
    [SerializeField]
    protected Tile currentTile;
    [SerializeField]
    protected Tile nextTile;
    [SerializeField]
    protected Tile targetTile;

    protected int stepIndex = 0;
    protected float stepToMoveDelay;

    // Transition conditions
    protected bool pathNeedsReevaluation = false;
    protected bool pathIsComplete = false;

    protected override void Start() {
        player = FindObjectOfType<PlayerGridObject>();
        animator = GetComponent<Animator>();

        path = new List<Globals.Direction>();
        astarAlgorithm = new AStar(tileMap);
        visionModule = GetComponent<Vision>();

        startTile = tileMap.GetNearestTile(transform.position);
        // TODO: 
        if (startTile == null)
            Destroy(this);
        currentTile = startTile;

        stepToMoveDelay = stepDelay / (float)moveAmount;
        StartCoroutine(SpawnRandomDir());

        base.Start();
    }
    protected override void Update() {
        if (canSpin) {
            counter++;
            if (counter > turnDelay) {
                player.gameObject.transform.Rotate(new Vector3(0, 0, 90f));
                // or player.gameObject.direction = Globals.Direction.NEWS;
                counter = 0;
                maxSpinCounter++;

                if (maxSpinCounter >= maxSpin) {
                    player.TakeDamage(damage);
                    canSpin = false;
                    player.gameObject.transform.rotation = spawnRotation;
                    player.canMove = true;
                    maxSpinCounter = 0;
                    //depending on player direction, push player in x direction
                    dir2 = (int)player.direction * -1;
                    for (int i = 0; i < 15; i++) {
                        player.Move((Globals.Direction)dir2);
                    }
                }
            }
        }
        foreach (GameObject obj in list) {
            if (obj == null) {
                currentSpawns--;
                list.Remove(obj);
                break; //prevents error from modifying list during foreach loop
            }
        }
        base.Update();
    }
    public void OnDrawGizmos() {
        if (!debug) return;

        if (!Application.isPlaying) return;

        Vector2 acc = startTile.transform.position;
        foreach (var v in path) {
            var v_real = Globals.DirectionToVector(v);
            Gizmos.DrawRay(acc, v_real);
            acc += v_real;
        }
    }

    // ============================================================
    // | States
    // ============================================================

    /// <summary>
    /// Take a step on the path to target.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ExecuteActionTakeStep() {
        stepIndex = 0;

        nextTile = tileMap.NextTile(currentTile, path[currentPathIndex]);

        yield return null;
    }

    protected override IEnumerator ExecuteActionStepping() {
        Move(path[currentPathIndex]);

        stepIndex++;

        yield return null;
    }

    protected override IEnumerator ExecuteActionAttack() {
        Attack();

        yield return null;
    }

    protected override IEnumerator ExecuteActionEvaluateStep() {
        // Check if we got close to the next tile
        float offsetMagnitude = Vector2.Distance(transform.position, nextTile.transform.position);

        if (offsetMagnitude < allowedOffset) {
            // Move the monster to the center of the tile
            transform.position = nextTile.transform.position;
        }
        else {
            // TODO: if too far from the next tile do something
            pathNeedsReevaluation = true;
        }

        // Manipulate the index, current tile, etc. for next step
        currentPathIndex++;

        // Check if we finished our path
        if (currentPathIndex >= path.Count) {
            pathIsComplete = true;
        }

        currentTile = nextTile;
        pathNeedsReevaluation = false;

        yield return null;
    }

    protected override IEnumerator ExecuteActionChaseTarget() {
        Move(direction);

        yield return null;
    }

    /// <summary>
    /// Create the path from current location to the target object
    /// </summary>
    protected override IEnumerator ExecuteActionPathFind() {
        if (targetObject) {
            targetTile = tileMap.GetNearestTile(targetObject.transform.position);

            // Find a path
            path = astarAlgorithm.FindPath(currentTile, targetTile);

            // We are on the first step of the path
            currentPathIndex = 0;
        }
        else {
            // Idle if there is nothing to target
            state = State.Idle;
        }

        yield return null;
    }

    protected override IEnumerator ExecuteActionWander() {
        Globals.Direction dir = (Globals.Direction)UnityEngine.Random.Range(0, 4);
        path.Add(dir);
        yield return null;
    }

    protected override IEnumerator ExecuteActionDisabled() {
        // Do nothing, the monster is disabled
        yield return null;
    }

    // ============================================================
    // | Transitions
    // ============================================================

    protected override bool ReevaluatePath() {
        return pathNeedsReevaluation;
    }

    protected override bool CanAttack() {
        // Check cooldown
        AttackCollider edgeTrigger = GetHitColliderFromDirection(direction);

        List<KillableGridObject> killList = edgeTrigger.GetKillList();

        // Check if there is anything to kill
        if (killList.Count > 0) {
            // Check if any of the killables are an enemy
            foreach (KillableGridObject target in killList) {
                if (target.faction != this.faction) {
                    // Rotate in the direction of the target monster is attacking
                    Rotate(Globals.VectorsToDirection(transform.position, target.transform.position));
                    return true;
                }
            }
        }

        return false;
    }

    protected override bool CanSenseTarget() {
        return visionModule.CanSeePlayer(direction);
    }

    protected override bool StepFinished() {
        // step is finished if we have gone moveAmount number of steps
        // and finished the step to move amount delay
        return stepIndex >= moveAmount && TimeInState() > stepToMoveDelay;
    }

    protected override bool PathComplete() {
        return pathIsComplete;
    }

    public override void Reset() { }

    void SpawnSlime() {
        randInt = randGen.Next(0, 4);

        if (randInt == 1) {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0.0f);
        }
        else if (randInt == 2) {
            spawnPosition = new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, 0.0f);
        }
        else if (randInt == 3) {
            spawnPosition = new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, 0.0f);
        }
        else {
            spawnPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, 0.0f);
        }
        GameObject newLilSlime = (GameObject)Instantiate(littleSlime, spawnPosition, spawnRotation);
        list.Add(newLilSlime);
        newLilSlime.GetComponent<littleWindSlime>().tileMap = tileMap;
        newLilSlime.GetComponent<littleWindSlime>().targetObject = targetObject;

        currentSpawns++;

    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            player.canMove = false;
            canSpin = true;
        }
    }
    IEnumerator SpawnRandomDir() {
        while (health > 0) {
            if (currentSpawns < maxSpawn) {

                SpawnSlime();
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return 0;
        }
        yield return 0;
    }

    protected override void Die() {
        SpawnSlime();
        SpawnSlime();
        SpawnSlime();
        SpawnSlime();
        player.canMove = true;
        base.Die();
    }
}
