using UnityEngine;
using System;
using System.IO;
using System.Collections;
public abstract class ChasingMonsterAbstractFSM : EnemyGridObject, IStateMachine{
    protected float transitionedAt;
    public int exceptionCount;
    public int shutDownFSMAfterNExceptions = 10;
    public Coroutine coroutine;
    public enum State { 
        TransitionFromIdle = 0,
        Wander = 1,
        ChasePlayer = 2,
        RandomizeDirection = 3,
        Idle = 4
    }  
    protected virtual void OnEnable() { 
        RunFSM();
    }
    public State state = State.Idle;
    private IEnumerator FSMThread( float delayRate ) {
        bool isRunning = true;
        while(isRunning) {
            // Get a uniform random number for MDP transitions
            float rand = UnityEngine.Random.value;
            State prevState = state;
            IEnumerator stateAction = null;
            try {
            // The following switch statement handles the state machine's action logic
                switch(state) {
                    case State.Wander:
                        stateAction = ExecuteActionWander();
                        break;
                    case State.ChasePlayer:
                        stateAction = ExecuteActionChasePlayer();
                        break;
                    case State.RandomizeDirection:
                        stateAction = ExecuteActionRandomizeDirection();
                        break;
                }
            }
            catch( Exception e ) {
                LogException(e);
            }
            yield return stateAction;
            
            
            try {
            // The following switch statement handles the MDP's state transition logic
            switch(state) {                case State.TransitionFromIdle:
                
                    if( CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.ChasePlayer;
                    }
                    else {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.RandomizeDirection;
                        
                    }
                    break;
                case State.Wander:
                
                    if( CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.ChasePlayer;
                    }
                    if( FinishedWandering() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Idle;
                    }
                    break;
                case State.ChasePlayer:
                
                    if( !CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Wander;
                    }
                    break;
                case State.RandomizeDirection:
                
                    if( true ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Wander;
                    }
                    break;
                case State.Idle:
                
                    if( IsBored() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.TransitionFromIdle;
                    }
                    if( CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.ChasePlayer;
                    }
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
    protected abstract IEnumerator ExecuteActionWander();
    protected abstract IEnumerator ExecuteActionChasePlayer();
    protected abstract IEnumerator ExecuteActionRandomizeDirection();
    // Transitional Logic Functions
    protected abstract bool IsBored();
    protected abstract bool CanSeePlayer();
    protected abstract bool FinishedWandering();
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