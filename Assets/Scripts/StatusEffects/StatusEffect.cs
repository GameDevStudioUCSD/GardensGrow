using UnityEngine;
using System.Collections;

/// <summary>
/// A loose definition of how a StatusEffect should be applied, start, and end.
/// This script should be applied to a prefab that will solely be used as a
/// StatusEffect prefab.  This way the prefab can be attached to an entity.
/// 
/// A good example of how to start could be to keep the current apply function but
/// on start run a coroutine that can call the end function when the duration is done.
/// 
/// Look at SpinStun for an example.
/// </summary>
public abstract class StatusEffect : MonoBehaviour {

    [Header("Parameters")]
    [Range(0.0f, 60.0f)]
    public float duration = 1.5f;

    protected KillableGridObject effectCaster;
    protected KillableGridObject affectedTarget;

    /// <summary>
    /// How the status effect should be applied.
    /// </summary>
    /// <param name="target">Victim of the StatusEffect.</param>
    public virtual void ApplyEffect(KillableGridObject caster, KillableGridObject target)
    {
        this.effectCaster = caster;
        this.affectedTarget = target;

        // parent this object to the target
        this.transform.parent = target.transform;

        StartEffect();
    }

    /// <summary>
    /// Start doing whatever effect is required of this
    /// StatusEffect.  Usually starts a coroutine that
    /// has the actual implementation of the StatusEffect.
    /// </summary>
    public abstract void StartEffect();

    /// <summary>
    /// Clean up the status effect.
    /// Usually destroys the status effect game object.
    /// Can be called from within a coroutine that was
    /// started by this script.
    /// </summary>
    public abstract void EndEffect();
}
