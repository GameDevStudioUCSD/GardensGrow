using UnityEngine;
using System.Collections;

public class SpinStun : StatusEffect {

    protected Coroutine spinCoroutine;

    public override void ApplyEffect(KillableGridObject target, float duration)
    {
        this.target = target;
        this.duration = duration;

        // parent this object to the target
        this.transform.parent = target.transform;

        StartEffect();
    }

    public override void StartEffect()
    {
        spinCoroutine = StartCoroutine(SpinTarget());
    }

    /// <summary>
    /// Spins the target around.
    /// If the target is player, then prevent movement.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpinTarget()
    {
        // Disable movement for player
        if (target.tag == Globals.player_tag)
            target.gameObject.GetComponent<PlayerGridObject>().canMove = false;

        // Spin the target
        for (int i = 0; i < 4; i++)
        {
            // If the target died mid spin, exit coroutine
            if (target == null)
                yield break;

            target.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
            yield return new WaitForSeconds(duration / 4.0f);
        }

        // Reenable movement for player
        if (target.tag == Globals.player_tag)
            target.gameObject.GetComponent<PlayerGridObject>().canMove = true;

        EndEffect();
    }

    public override void EndEffect()
    {
        Destroy(this.gameObject);
    }

}
