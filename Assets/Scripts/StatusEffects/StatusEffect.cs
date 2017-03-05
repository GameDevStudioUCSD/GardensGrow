using UnityEngine;
using System.Collections;

/// <summary>
/// StatusEffects are their own monobehaviour and should be their own
/// prefab instances.  This allows them to be applied, destroyed, changed, etc.
/// </summary>
public abstract class StatusEffect : MonoBehaviour {

    protected KillableGridObject target;
    protected float duration;

    /// <summary>
    /// Parent the StatusEffect gameobject to the target.
    /// </summary>
    /// <param name="target">Victim of the StatusEffect.</param>
    public abstract void ApplyEffect(KillableGridObject target, float duration);

    public abstract void StartEffect();

    /// <summary>
    /// Clean up the status effect.
    /// Usually destroys the status effect game object.
    /// </summary>
    public abstract void EndEffect();
}
