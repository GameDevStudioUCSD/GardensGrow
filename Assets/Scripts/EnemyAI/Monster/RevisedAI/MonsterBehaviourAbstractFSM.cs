
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
        Detect = 2,
        Attack = 3
    }  
    public State state = State.PathFinding;
    protected override void Start() {
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
                    case State.Detect:
                        stateAction = ExecuteActionDetect();
                        break;
                    case State.Attack:
                        stateAction = ExecuteActionAttack();
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
                    if( Detected() ) 
                        state = State.Detect;
                    if( CanAttack() ) 
                        state = State.Attack;
                    if( OnHit() ) 
                        state = State.Damaged;
                    break;
                case State.Damaged:
                    if( Recovered() ) 
                        state = State.PathFinding;
                    break;
                case State.Detect:
                    state = State.PathFinding;
                    if( OnHit() ) 
                        state = State.Damaged;
                    break;
                case State.Attack:
                    if( !CanAttack() ) 
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
    protected abstract IEnumerator ExecuteActionDetect();
    protected abstract IEnumerator ExecuteActionAttack();
    // Transitional Logic Functions
    protected abstract bool Detected();
    protected abstract bool CanAttack();
    protected abstract bool Recovered();
    protected abstract bool OnHit();
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }

    
    protected virtual void OnTransition() { }
  
}