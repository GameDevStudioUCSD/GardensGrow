using UnityEngine;
using System;
using System.IO;
using System.Collections;
public abstract class SmartChasingMonsterAbstractFSM : EnemyGridObject, IStateMachine{
    protected float transitionedAt;
    public int exceptionCount;
    public int shutDownFSMAfterNExceptions = 10;
    public Coroutine coroutine;
    public enum State { 
        Idle = 0,
        Attack = 1,
        ChasePlayer = 2,
        TakeStep = 3,
        PathFind = 4,
        Disabled = 5,
        EvaluatePath = 6
    }

    public State state = State.Idle;

    protected override void Start() {
        base.Start();
        RunFSM();
    }

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
                    case State.TakeStep:
                        stateAction = ExecuteActionTakeStep();
                        break;
                    case State.Attack:
                        stateAction = ExecuteActionAttack();
                        break;
                    case State.ChasePlayer:
                        stateAction = ExecuteActionChasePlayer();
                        break;
                    case State.PathFind:
                        stateAction = ExecuteActionPathFind();
                        break;
                    case State.Disabled:
                        stateAction = ExecuteActionDisabled();
                        break;
                    case State.EvaluatePath:
                        stateAction = ExecuteActionEvaluatePath();
                        break;
                }
            }
            catch( Exception e ) {
                LogException(e);
            }
            yield return stateAction;
            
            
            try {
            // The following switch statement handles the MDP's state transition logic
            switch(state) {                case State.TakeStep:
                
                    if( Disabled() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Disabled;
                    }
                    if( FinishedStep() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.EvaluatePath;
                    }
                    if( CanAttack() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Attack;
                    }
                    else if( CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.ChasePlayer;
                    }
                    break;
                case State.Attack:
                
                    if( Disabled() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Disabled;
                    }
                    if( FinishedAttack() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Idle;
                    }
                    break;
                case State.ChasePlayer:
                
                    if( Disabled() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Disabled;
                    }
                    if( !CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Idle;
                    }
                    else if( CanAttack() )
                        {
                            if (true)
                                state = State.Attack;
                        }
                    break;
                case State.Idle:
                
                    if( Disabled() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Disabled;
                    }
                    if( CanAttack() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Attack;
                    }
                    else if( CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.ChasePlayer;
                    }
                    else {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.PathFind;
                        
                    }
                    break;
                case State.PathFind:
                
                    if( Disabled() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Disabled;
                    }
                    if( CanAttack() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Attack;
                    }
                    else if( CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.ChasePlayer;
                    }
                    else {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.TakeStep;
                        
                    }
                    break;
                case State.Disabled:
                
                    if( Disabled() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Disabled;
                    }
                    else {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Idle;
                        
                    }
                    break;
                case State.EvaluatePath:
                
                    if( Disabled() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Disabled;
                    }
                    if( !FinishedStep() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.TakeStep;
                    }
                    if( FinishedPath() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.PathFind;
                    }
                    if( CanSeePlayer() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.ChasePlayer;
                    }
                    if( CanAttack() ) {
                        // Probability of transition 100.0%
                        if( true )
                            state = State.Attack;
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
    protected abstract IEnumerator ExecuteActionTakeStep();
    protected abstract IEnumerator ExecuteActionAttack();
    protected abstract IEnumerator ExecuteActionChasePlayer();
    protected abstract IEnumerator ExecuteActionPathFind();
    protected abstract IEnumerator ExecuteActionDisabled();
    protected abstract IEnumerator ExecuteActionEvaluatePath();
    // Transitional Logic Functions
    protected abstract bool FinishedAttack();
    protected abstract bool FinishedPath();
    protected abstract bool CanSeePlayer();
    protected abstract bool Disabled();
    protected abstract bool CanAttack();
    protected abstract bool FinishedStep();
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