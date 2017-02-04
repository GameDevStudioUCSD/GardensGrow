
using UnityEngine;
public abstract class MonsterBehaviourAbstractFSM  {
     
    protected float transitionedAt;
    public enum State { 
        PathFinding = 0,
        Damaged = 1,
        PrimaryBehaviour = 2
    }  
    public State state = State.PathFinding;

    public void Step()
    {
		State prevState = state;
		 
                switch(state) {
                    case State.PathFinding:
                        ExecuteActionPathFinding();
                        break;
                    case State.Damaged:
                        ExecuteActionDamaged();
                        break;
                    case State.PrimaryBehaviour:
                        ExecuteActionPrimaryBehaviour();
                        break;
            }

// The following switch statement handles the HLSM's state transition logic
            switch(state) {
                case State.PathFinding:
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
		
            if (prevState != state)
            {
                transitionedAt = Time.time;
                OnTransition();
            }
			
    }
    // State Logic Functions
    protected abstract void ExecuteActionPathFinding();
    protected abstract void ExecuteActionDamaged();
    protected abstract void ExecuteActionPrimaryBehaviour();
    // Transitional Logic Functions
    protected abstract bool IsPathStale();
    protected abstract bool Recovered();
    protected abstract bool OnHit();
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }

    
    protected virtual void OnTransition() { }
  
}