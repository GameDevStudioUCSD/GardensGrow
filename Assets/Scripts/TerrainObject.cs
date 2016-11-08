using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TerrainObject : StaticGridObject
{
    public Sprite[] sprites;
    public Sprite chosenSprite;
    public bool randomize;
    private SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
	    if (!Application.isPlaying)
	    {
	        if (sprites.Length > 0)
	        {
	            if (!chosenSprite || randomize)
	            {
	                randomize = false;
	                chosenSprite = sprites[(int) Random.Range(0, sprites.Length - 0.0001f)];
	                spriteRenderer.sprite = chosenSprite;
	            }
	            else if (spriteRenderer.sprite != chosenSprite)
	            {
	                spriteRenderer.sprite = chosenSprite;
	            }
	        }
	    }
	}
}
