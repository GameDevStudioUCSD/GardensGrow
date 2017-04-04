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
    }

    public void Brighten(float amount) {
        level += amount;
		if(!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
		spriteRenderer.color = new Color(level, level, level);
    }

    public void Dim() {
        level = 0;
		if(!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
		spriteRenderer.color = new Color(level, level, level);
    }
}
