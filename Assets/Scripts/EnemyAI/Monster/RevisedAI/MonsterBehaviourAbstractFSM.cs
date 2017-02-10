
using UnityEngine;
using System;
using System.IO;
using System.Collections;

public abstract class MonsterBehaviourAbstractFSM: EnemyGridObject, IStateMachine {
     
    protected float transitionedAt;
    [Header("State Machine Variables")]
    public int exceptionCount;
    public int shutDownFSMAfterNExceptions = 10;
    public Coroutine coroutine;
    public enum State { 
        PathFinding = 0,
        Damaged = 1,
        PrimaryBehaviour = 2
    }  
    public State state = State.PathFinding;
    protected virtual void Start() {
        base.Start();
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
                    case State.PathFinding:
                        stateAction = ExecuteActionPathFinding();
                        break;
                    case State.Damaged:
                        stateAction = ExecuteActionDamaged();
                        break;
                    case State.PrimaryBehaviour:
                        stateAction = ExecuteActionPrimaryBehaviour();
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
                case State.PathFinding:
                    if( CanAct() ) 
                        state = State.PrimaryBehaviour;
                    if( OnHit() ) 
                        state = State.Damaged;
                    break;
                case State.Damaged:
                    if( Recovered() ) 
                        state = State.PathFinding;
                    break;
                case State.PrimaryBehaviour:
                    if( IsPathStale() ) 
                        state = State.PathFinding;
                    if( OnHit() ) 
                        state = State.Damaged;
                    break;
            }		
            }
            catch(Exception e) {
                LogException(e);
            }
            

					
            

    }
    // State Logic Functions
    protected abstract IEnumerator ExecuteActionPathFinding();
    protected abstract IEnumerator ExecuteActionDamaged();
    protected abstract IEnumerator ExecuteActionPrimaryBehaviour();
    // Transitional Logic Functions
    protected abstract bool IsPathStale();
    protected abstract bool CanAct();
    protected abstract bool Recovered();
    protected abstract bool OnHit();
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }

    
    protected virtual void OnTransition() { }
  
}