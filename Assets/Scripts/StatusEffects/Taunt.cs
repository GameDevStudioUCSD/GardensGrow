using UnityEngine;
using System.Collections;

/// <summary>
/// Causes the affected target to retarget onto the caster.
/// </summary>
public class Taunt : StatusEffect {

    protected GameObject originalTarget;

    public override void StartEffect()
    {
        // "Aggro" onto the caster
        Retarget(effectCaster.gameObject);

        // Play out the duration of the taunt
        StartCoroutine(WaitDuration());
    }

    protected IEnumerator WaitDuration()
    {
        yield return new WaitForSeconds(duration);
    }

    protected void Retarget(GameObject newPathFindingTarget)
    {
        // Try to find path finding module on target of taunt
        PathFindingModule pathFinding = affectedTarget.GetComponentInChildren<PathFindingModule>();
        if (pathFinding)
        {
            // Save the original target
            originalTarget = pathFinding.parameters.target;
            // Change the target to a new one
            pathFinding.parameters.target = newPathFindingTarget;

            // Have pathfinding refresh its path
            pathFinding.RefreshPath();
        }
    }

    public override void EndEffect()
    {
        // Reset "aggro" onto the original target
        Retarget(originalTarget);

        Destroy(this.gameObject);
    }
}
