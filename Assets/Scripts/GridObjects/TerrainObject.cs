﻿using UnityEngine;

public class TerrainObject : StaticGridObject
{
    public Sprite[] sprites;

    /// <summary>
    /// Allows chosenSprite field to be right-clicked in 
    /// </summary>
    [ContextMenuItem("Randomize Sprite", "Randomize")]
    public Sprite chosenSprite;

    private SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start ()
    {
    	Randomize();
    	base.Start();
    }

    private void Randomize()
    {
        if(!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (sprites.Length > 0)
        {
            chosenSprite = sprites[Random.Range(0, sprites.Length)];
            if (spriteRenderer)
            {
            	spriteRenderer.sprite = chosenSprite;
            }
                /*
            else if (spriteRenderer.sprite != chosenSprite)
            {
                spriteRenderer.sprite = chosenSprite;
            }
            */
        }
    }
}
