using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Vision))]
public class ChasingMonster : ChasingMonsterAbstractFSM {
    [Header("Delay Parameters")]
    public float idleDelay = 2;
    public float wanderingDelay = 3;
    Vision visionModule;
    public override void Reset()
    {
    }

    protected override bool CanSeePlayer()
    {
        return visionModule.CanSeePlayer(direction);
    }

    protected override IEnumerator ExecuteActionChasePlayer()
    {
        Move(direction);
        yield return null;
    }

    protected override IEnumerator ExecuteActionRandomizeDirection()
    {
        direction = (Globals.Direction)UnityEngine.Random.Range(0, 4);
        yield return null;
    }

    protected override IEnumerator ExecuteActionWander()
    {
        Move(direction);
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
