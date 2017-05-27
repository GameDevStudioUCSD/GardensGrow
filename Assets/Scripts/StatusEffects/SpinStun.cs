using UnityEngine;
using System.Collections;

public class SpinStun : StatusEffect {

    [Tooltip("Each spin is actually a 90 degree turn in counter clockwise direction.")]
    [Range(0, 99)]
    public int numSpins = 4;

    protected Coroutine spinCoroutine;

    public override void StartEffect()
    {
        // Start up a coroutine for the spinning
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
        if (affectedTarget.tag == Globals.player_tag)
            affectedTarget.gameObject.GetComponent<PlayerGridObject>().canMove = false;

        // Spin the target
        for (int i = 0; i < numSpins; i++)
        {
            // If the target died mid spin, exit coroutine
            if (affectedTarget == null)
                yield break;

            affectedTarget.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
            yield return new WaitForSeconds(duration / (float)numSpins);
        }

        // Reenable movement for player
        if (affectedTarget.tag == Globals.player_tag)
            affectedTarget.gameObject.GetComponent<PlayerGridObject>().canMove = true;

        EndEffect();
    }

    public override void EndEffect()
    {
        Destroy(this.gameObject);
    }
}
