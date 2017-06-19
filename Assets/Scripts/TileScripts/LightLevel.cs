using UnityEngine;
using System.Collections;

public class LightLevel : MonoBehaviour {

    public float level = 0;
    public SpriteRenderer spriteRenderer;

    private Color defaultIllumination;

    public virtual void Start() {
        if(!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //null check
        if (spriteRenderer)
        {
            spriteRenderer.color = new Color(0.0f, 0.0f, 0.0f);
        }
    }

    public virtual void ChangeLightLevel(float amount) {
        level += amount;
        
        if(!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();

        float clamped = level;
        if (level > 1.0f) clamped = 1.0f;
        else if (level < 0.0f) clamped = 0.0f;

        spriteRenderer.color = new Color(clamped, clamped, clamped);
    }
}
