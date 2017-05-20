using UnityEngine;
using System.Collections;

public class LightLevel : MonoBehaviour {

    public float level = 0;
    public SpriteRenderer spriteRenderer;

    private Color defaultIllumination;

    public void Start() {
        if(!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = new Color(0.0f, 0.0f, 0.0f);
        ChangeLightLevel(0);
    }

    public void ChangeLightLevel(float amount) {
        level += amount;
        
        if(!spriteRenderer) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        float clamped;
        if (level > 1.0f) {
            clamped = 1.0f;
        }
        else if (level < 0.0f) {
            clamped = 0.0f;
        }
        else {
            clamped = level;
        }
        spriteRenderer.color = new Color(clamped, clamped, clamped);
    }
}
