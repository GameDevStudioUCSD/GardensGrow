using UnityEngine;
using System.Collections;

public class LightLevel : TerrainObject {

    public float level = 0;

    private Color defaultIllumination;

    public void Start() {
    	base.Start();
		if(!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    	spriteRenderer.color = new Color(0.0f, 0.0f, 0.0f);
    }

    public void Brighten(float amount) {
        level += amount;
        //Debug.Log(level);
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
