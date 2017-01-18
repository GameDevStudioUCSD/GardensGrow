
using UnityEngine;
using System;
using System.IO;
using System.Collections;

public abstract class PathFindingMonsterAbstractFSM: EnemyGridObject, IStateMachine {
     
    protected float transitionedAt;
    [Header("State Machine Variables")]
    public int exceptionCount;
    public int shutDownFSMAfterNExceptions = 10;
    public Coroutine coroutine;
    public enum State { 
        TakeStep = 0,
        Attack = 1,
        ChaseTarget = 2,
        EvaluateStep = 3,
        Idle = 4,
        PathFind = 5,
        Disabled = 6,
        Stepping = 7,
        Wander = 8
    }  
    public State state = State.Idle;
    protected virtual void OnEnable() { 
        RunFSM();
    }
    private IEnumerator FSMThread( float delayRate ) {
        bool isRunning = true;
        while(isRunning) {
            yield return Step();
            yield return new WaitForSeconds(delayRate);
            if (exceptionCount > shutDownFSMAfterNExceptions)
            {
                Debug.LogError(this + " has exceeded the number of allowed exceptions! Shutting down.");
                isRunning = false;
            }
        }
    }
	public void RunFSM()
    {
        RunFSM(Time.fixedDeltaTime);
    }
    public void RunFSM(float delayRate)
    {
        coroutine = StartCoroutine(FSMThread(delayRate));
    }

	public bool TestAndSet(ref bool variable, bool val) {
        bool rv = variable;
        variable = val;
        return rv;
    }
    protected void LogException(Exception e) {
        string exceptionAcc = this + " threw exception " + e.GetType();
        exceptionAcc += " during state: " + state + "\n";
        #if (EXCEPTION_LOGGER)
		if( exceptionCount++ == 0 ) {
			var dest = ExceptionLogger.LogException(e, exceptionAcc, this);
			exceptionAcc += "Full details logged to: " + dest + "\n";
			exceptionAcc += e.StackTrace;
		}
		#else
		exceptionCount++;
		#endif
        Debug.LogError( exceptionAcc );
    }
	public abstract void Reset();

    public IEnumerator Step()
    {
		State prevState = state;
		
        IEnumerator stateAction = null;
        try {
			    // The following switch statement handles the state machine's action logic
                switch(state) {
                    case State.TakeStep:
                        stateAction = ExecuteActionTakeStep();
                        break;
                    case State.Attack:
                        stateAction = ExecuteActionAttack();
                        break;
                    case State.ChaseTarget:
                        stateAction = ExecuteActionChaseTarget();
                        break;
                    case State.EvaluateStep:
                        stateAction = ExecuteActionEvaluateStep();
                        break;
                    case State.PathFind:
                        stateAction = ExecuteActionPathFind();
                        break;
                    case State.Disabled:
                        stateAction = ExecuteActionDisabled();
                        break;
                    case State.Stepping:
                        stateAction = ExecuteActionStepping();
                        break;
                    case State.Wander:
                        stateAction = ExecuteActionWander();
                        break;
                }
            }
        catch( Exception e ) {
                LogException(e);
            }
        yield return stateAction;

            try {
                

// The following switch statement handles the HLSM's state transition logic
            switch(state) {
                case State.TakeStep:
                    state = State.Stepping;
                    break;
                case State.Attack:
                    if( CanAttack() ) 
                        state = State.Attack;
                    else 
                       state = State.Idle;
                    break;
                case State.ChaseTarget:
                    if( CanAttack() ) 
                        state = State.Attack;
                    if( !CanSenseTarget() ) 
                        state = State.Idle;
                    break;
                case State.EvaluateStep:
                    if( PathComplete() )
                    {
                        state = State.Idle;
                        break;
                    }
                    if( ReevaluatePath() ) 
                        state = State.PathFind;
                    else 
                       state = State.TakeStep;
                    if( CanAttack() ) 
                        state = State.Attack;
                    if( CanSenseTarget() ) 
                        state = State.ChaseTarget;
                    break;
                case State.Idle:
                    if( CanSenseTarget() ) 
                        state = State.ChaseTarget;
                    else
                    {
                        if (PathComplete())
                            state = State.Wander;
                        else
                            state = State.PathFind;
                    }
                    break;
                case State.PathFind:
                    if( CanAttack() ) 
                        state = State.Attack;
                    else 
                       state = State.TakeStep;
                    if( CanSenseTarget() ) 
                        state = State.ChaseTarget;
                    else 
                       state = State.TakeStep;
                    break;
                case State.Disabled:
                    break;
                case State.Stepping:
                    if( StepFinished() ) 
                        state = State.EvaluateStep;
                    else 
                       state = State.Stepping;
                    if( CanAttack() ) 
                        state = State.Attack;
                    if( CanSenseTarget() ) 
                        state = State.ChaseTarget;
                    break;
                case State.Wander:
                    state = State.TakeStep;
                    break;
            }		
            }
            catch(Exception e) {
                LogException(e);
            }
            

					
            

    }
    // State Logic Functions
    protected abstract IEnumerator ExecuteActionTakeStep();
    protected abstract IEnumerator ExecuteActionAttack();
    protected abstract IEnumerator ExecuteActionChaseTarget();
    protected abstract IEnumerator ExecuteActionEvaluateStep();
    protected abstract IEnumerator ExecuteActionPathFind();
    protected abstract IEnumerator ExecuteActionDisabled();
    protected abstract IEnumerator ExecuteActionStepping();
    protected abstract IEnumerator ExecuteActionWander();
    // Transitional Logic Functions
    protected abstract bool PathComplete();
    protected abstract bool CanSenseTarget();
    protected abstract bool StepFinished();
    protected abstract bool CanAttack();
    protected abstract bool ReevaluatePath();
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }

    
    protected virtual void OnTransition() { }
  
}