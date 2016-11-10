using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Vision))]
public class ChasingMonster : ChasingMonsterAbstractFSM {
    [Header("Delay Parameters")]
    public float idleDelay = 2;
    public float wanderingDelay = 3;
    [Range(0,5)]
    public int speed = 2;
    Vision visionModule;

    protected override void Start() {
        base.Start();
    }

    public override void Reset()
    {
    }

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

    protected override bool IsBored()
    {
        return TimeInState() > (idleDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        visionModule = GetComponent<Vision>();
    }


}
