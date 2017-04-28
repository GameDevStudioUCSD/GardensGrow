using UnityEngine;
using System.Collections;

public class ScorpionVenom : StatusEffect {

    [Header("Parameters")]
    [Range(0, 99)]
    public int poisonDamage;
    [Range(0, 99)]
    public int damageTicks;
    public Color poisonedTint;

    protected Coroutine poisonCoroutine;

    protected SpriteRenderer sprite;
    // Save the color of the sprite before
    // poison tint is applied
    protected Color originalSpriteColor;

    public override void StartEffect()
    {
        // Sprite poison tint
        // Problem I can see with this is if a non-original sprite tint
        // is captured by this such as the 'taking damage' tint
        sprite = target.GetComponent<SpriteRenderer>();
        if (sprite)
        {
            originalSpriteColor = sprite.color;
            sprite.color = poisonedTint;
        }

        poisonCoroutine = StartCoroutine(PoisonTarget());
    }

    private IEnumerator PoisonTarget()
    {
        int tickCount = 0;
        while(tickCount < damageTicks)
        {
            // The first tick of poison damage does not happen immediately
            // upon being poisoned
            yield return new WaitForSeconds(duration / damageTicks);

            // damage
            target.TakeDamage(poisonDamage);

            tickCount++;
        }

        EndEffect();
    }

    public override void EndEffect()
    {
        // Return sprite to original tint
        sprite.color = originalSpriteColor;

        Destroy(this.gameObject);
    }
}
