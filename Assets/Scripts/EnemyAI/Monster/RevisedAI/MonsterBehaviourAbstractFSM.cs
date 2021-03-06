using UnityEngine;
using System;
using System.IO;
using System.Collections;

public abstract class MonsterBehaviourAbstractFSM: EnemyGridObject, IStateMachine {

    protected float transitionedAt;
    [Header("State Machine Variables")]
    public int exceptionCount;
    public int shutDownFSMAfterNExceptions = 10;
    public Coroutine FSMCoroutine;
    public enum State { 
        PathFinding = 0,
        Damaged = 1,
        PrimaryBehaviour = 2,
        Disabled = 3,
        Attack = 4
    }  
    public State state = State.PathFinding;
    public bool isDisabled = true;
    protected override void Start() { 
        base.Start();

        if (isDisabled)
            DisableAI();
        else
            StartAI();
    }
    private IEnumerator FSMThread( float delayRate ) {
        yield return new WaitForEndOfFrame(); // Stops race conditions
        bool isRunning = true;
        while(isRunning) {
            if (!Globals.canvas.dialogue) yield return Step();
            yield return new WaitForSeconds(delayRate);
            if (exceptionCount > shutDownFSMAfterNExceptions) {
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
        FSMCoroutine = StartCoroutine(FSMThread(delayRate));
    }

    /*
    public void Update()
    {
        if (coroutine == null)
            Debug.Log("NULL");
        else
            Debug.Log("EXISTS");
    }
    */

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
		//State prevState = state;
		
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
                    case State.Disabled:
                        stateAction = ExecuteActionDisabled();
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
                    if( CanAttack() ) 
                        state = State.Attack;
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
                    if( CanMove() ) 
                        state = State.PathFinding;
                    if( OnHit() ) 
                        state = State.Damaged;
                    break;
                case State.Disabled:
                    break;
                case State.Attack:
                    if( CanMove() ) 
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
    protected abstract IEnumerator ExecuteActionDisabled();
    protected abstract IEnumerator ExecuteActionAttack();
    // Transitional Logic Functions
    protected abstract bool CanMove();
    protected abstract bool CanAct();
    protected abstract bool CanAttack();
    protected abstract bool Recovered();
    protected abstract bool OnHit();
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }

    
    protected virtual void OnTransition() { }

    public void OnDisable()
    {
        DisableAI();
    }

    /// <summary>
    /// Stop the AI
    /// </summary>
    public virtual void DisableAI()
    {
        isDisabled = true;
        state = State.Disabled;
        if (FSMCoroutine != null)
        {
            StopCoroutine(FSMCoroutine);
        }
    }

    /// <summary>
    /// Start the AI
    /// </summary>
    public virtual void StartAI()
    {
        isDisabled = false;
        state = State.PathFinding;
        if (FSMCoroutine != null)
        {
            StopCoroutine(FSMCoroutine);
        }

        RunFSM();
    }
}