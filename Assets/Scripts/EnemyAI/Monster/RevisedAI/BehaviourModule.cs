using UnityEngine;
using System.Collections;

/// <summary>
/// BehaviourModule
/// 
/// Mainly used for typechecking purposes.
/// 
/// All of the AI state machine behaviour modules aside from the high level
/// one should inherit from this class.
/// </summary>
public abstract class BehaviourModule : MonoBehaviour {

    public virtual void Step() { }
}
