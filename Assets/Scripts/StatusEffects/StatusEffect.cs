using UnityEngine;
using System.Collections;

/// <summary>
/// A loose definition of how a StatusEffect should be applied, start, and end.
/// This script should be applied to a prefab that will solely be used as a
/// StatusEffect prefab.  This way the prefab can be attached to an entity.
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
    /// Create the status effect game object to apply onto the target
    /// </summary>
    public static void ApplyStatusEffect(KillableGridObject caster, KillableGridObject target, GameObject statusEffectPrefab)
    {
        // Create status effect object instance to get the status effect component
        GameObject statusEffectObject = (GameObject)Instantiate(statusEffectPrefab);
        StatusEffect statusEffect = statusEffectObject.GetComponent<StatusEffect>();

        statusEffect.effectCaster = caster;
        statusEffect.affectedTarget = target;

        statusEffect.ApplyEffect();
    }

    /// <summary>
    /// Parent the status effect object to the target and start effect.
    /// 
    /// This can also be used to specify what happens if
    /// the target already has the status effect.
    /// </summary>
    public virtual void ApplyEffect()
    {
        this.transform.SetParent(affectedTarget.transform);

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
