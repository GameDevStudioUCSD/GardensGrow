using UnityEngine;

/// <summary>
/// Abstract path finding behaviour class.
/// 
/// Pathfinding submodules should inherit from this class.
/// 
/// This is a representation of a Finite State Machine.
///
/// There should be a provided .jff file.
/// Please open the .jff file with JFlap to get a visual representation
/// of this state machine.
/// 
/// </summary>
public abstract class PathFindingBehaviourAbstractFSM : MonoBehaviour {
     
    protected float transitionedAt;
    public enum State { 
        FindPath = 0,
        StartStep = 1,
        Stepping = 2,
        EvaluateStep = 3,
        PathDone = 4
    }  
    public State state = State.FindPath;

    public void Step()
    {
		State prevState = state;
		 
                switch(state) {
                    case State.FindPath:
                        ExecuteActionFindPath();
                        break;
                    case State.StartStep:
                        ExecuteActionStartStep();
                        break;
                    case State.Stepping:
                        ExecuteActionStepping();
                        break;
                    case State.EvaluateStep:
                        ExecuteActionEvaluateStep();
                        break;
                    case State.PathDone:
                        ExecuteActionPathDone();
                        break;
            }

// The following switch statement handles the HLSM's state transition logic
            switch(state) {
                case State.FindPath:
                    if( !NewPath() ) 
                        state = State.StartStep;
                    if( FinishedPath() ) 
                        state = State.PathDone;
                    break;
                case State.StartStep:
                    state = State.Stepping;
                    break;
                case State.Stepping:
                    if( StepDone() ) 
                        state = State.EvaluateStep;
                    break;
                case State.EvaluateStep:
                    if( NewPath() ) 
                        state = State.FindPath;
                    else 
                       state = State.StartStep;
                    break;
                case State.PathDone:
                    if( !FinishedPath() ) 
                        state = State.FindPath;
                    break;
            }		
		
            if (prevState != state)
            {
                transitionedAt = Time.time;
                OnTransition();
            }
			
    }
    // State Logic Functions
    protected abstract void ExecuteActionFindPath();
    protected abstract void ExecuteActionStartStep();
    protected abstract void ExecuteActionStepping();
    protected abstract void ExecuteActionEvaluateStep();
    protected abstract void ExecuteActionPathDone();
    // Transitional Logic Functions
    protected abstract bool NewPath();
    protected abstract bool FinishedPath();
    protected abstract bool StepDone();
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }

    
    protected virtual void OnTransition() { }
  
}