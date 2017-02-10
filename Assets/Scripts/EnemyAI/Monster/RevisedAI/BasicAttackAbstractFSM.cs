using UnityEngine;

public abstract class BasicAttackAbstractFSM : BehaviourModule  {
     
    protected float transitionedAt;
    public enum State { 
        Attack = 0
    }  
    public State state = State.Attack;

    public void Step()
    {
		State prevState = state;
		 
                switch(state) {
                    case State.Attack:
                        ExecuteActionAttack();
                        break;
            }

// The following switch statement handles the HLSM's state transition logic
            switch(state) {
                case State.Attack:
                    break;
            }		
		
            if (prevState != state)
            {
                transitionedAt = Time.time;
                OnTransition();
            }
			
    }
    // State Logic Functions
    protected abstract void ExecuteActionAttack();
    // Transitional Logic Functions
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }

    
    protected virtual void OnTransition() { }
  
}