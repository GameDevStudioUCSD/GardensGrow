using UnityEngine;
using System.Collections;

public class ScorpionVenom : StatusEffect {

    [Range(0, 99)]
    public int damagePerTick;
    [Tooltip("Number of times the poison will apply damage over its duration.")]
    [Range(0, 99)]
    public int damageTicks;
    public Color poisonedTint;

    protected Coroutine poisonCoroutine;

    protected SpriteRenderer sprite;

    // Allow reapplying of the venom, refresh tick count
    public override void ApplyEffect()
    {
        // Check if the status effect already exists on the target
        ScorpionVenom previousScorpionVenom = affectedTarget.GetComponentInChildren<ScorpionVenom>();
        if(previousScorpionVenom)
        {
            // Get rid of the previous instance of scorpion venom
            previousScorpionVenom.EndEffect();
        }

        // Apply this status effect
        base.ApplyEffect();
    }

    public override void StartEffect()
    {
        PoisonHeartTint(poisonedTint);

        poisonCoroutine = StartCoroutine(PoisonTarget());
    }

    private IEnumerator PoisonTarget()
    {
        int tickCount = 0;
        while(tickCount < damageTicks)
        {
            // The first tick of poison damage does not happen immediately
            // upon being poisoned
            yield return new WaitForSeconds(duration / (float)damageTicks);

            // damage
            affectedTarget.TakeDamage(damagePerTick);

            tickCount++;
        }

        EndEffect();
    }

    protected void PoisonHeartTint(Color tint)
    {
        // Reset heart tint
        if (affectedTarget is PlayerGridObject)
        {
            PlayerGridObject player = affectedTarget as PlayerGridObject;

            for (int i = 0; i < UIController.totalHearts; i++)
            {
                player.canvas.healthIcons[i].color = tint;
            }
        }
    }

    public override void EndEffect()
    {
        // Reset the heart tint to default
        PoisonHeartTint(Color.white);

        Destroy(this.gameObject);
    }

}
