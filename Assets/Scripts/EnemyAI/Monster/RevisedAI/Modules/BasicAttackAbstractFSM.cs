using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Parent class for any attacking that only includes an attack state
/// and a cooldown state.
/// </summary>

public abstract class BasicAttackAbstractFSM : AttackModule {

    protected float transitionedAt;
    public enum State
    {
        Attack = 0,
        Cooldown = 1
    }
    public State state = State.Cooldown;

    public override void Step()
    {
        State prevState = state;

        switch (state)
        {
            case State.Attack:
                ExecuteActionAttack();
                break;
            case State.Cooldown:
                ExecuteActionCooldown();
                break;
        }

        // The following switch statement handles the HLSM's state transition logic
        switch (state)
        {
            case State.Attack:
                state = State.Cooldown;
                break;
            case State.Cooldown:
                if (AttackReady())
                    state = State.Attack;
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
    protected abstract void ExecuteActionCooldown();
    // Transitional Logic Functions
    protected abstract bool AttackReady();
    public float TimeInState()
    {
        return Time.time - transitionedAt;
    }


    protected virtual void OnTransition() { }


}
