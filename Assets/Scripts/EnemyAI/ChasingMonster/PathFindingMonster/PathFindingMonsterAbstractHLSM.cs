using UnityEngine;
using System;
using System.IO;
using System.Collections;
public abstract class PathFindingMonsterAbstractFSM : EnemyGridObject, IStateMachine{
    protected float transitionedAt;
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
        Disabled = 6
    }  
    protected virtual void OnEnable() { 
        RunFSM();
    }
    public State state = State.Idle;
    private IEnumerator FSMThread( float delayRate ) {
        bool isRunning = true;
        while(isRunning) {
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
                    state = State.EvaluateStep;
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
                    if( ReevaluatePath() ) 
                        state = State.PathFind;
                    if( CanAttack() ) 
                        state = State.Attack;
                    if( CanSenseTarget() ) 
                        state = State.ChaseTarget;
                    if( Continue() ) 
                        state = State.TakeStep;
                    break;
                case State.Idle:
                    if( CanSenseTarget() ) 
                        state = State.ChaseTarget;
                    else 
                       state = State.PathFind;
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
            }            }
            catch(Exception e) {
                LogException(e);
            }
            yield return new WaitForSeconds( delayRate );
            if( exceptionCount > shutDownFSMAfterNExceptions )
            {
                Debug.LogError( this + " has exceeded the number of allowed exceptions! Shutting down.");
                isRunning = false;
            }
            else if ( prevState!=state ) {
                transitionedAt = Time.time;
                OnTransition();
            }
        }
    }

    // State Logic Functions
    protected abstract IEnumerator ExecuteActionTakeStep();
    protected abstract IEnumerator ExecuteActionAttack();
    protected abstract IEnumerator ExecuteActionChaseTarget();
    protected abstract IEnumerator ExecuteActionEvaluateStep();
    protected abstract IEnumerator ExecuteActionPathFind();
    protected abstract IEnumerator ExecuteActionDisabled();
    // Transitional Logic Functions
    protected abstract bool ReevaluatePath();
    protected abstract bool CanAttack();
    protected abstract bool CanSenseTarget();
    protected abstract bool Continue();
    public void RunFSM()
    {
        RunFSM(Time.fixedDeltaTime);
    }
    public void RunFSM(float delayRate)
    {
        coroutine = StartCoroutine(FSMThread(delayRate));
    }
    public float TimeInState()
    {
        return Time.time - transitionedAt;
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
    protected virtual void OnTransition() { }
    public abstract void Reset();
}