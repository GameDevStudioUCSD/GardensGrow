using UnityEngine;
using System.Collections;

public class TreeGridObject : TerrainObject {
	public Sprite[] sprites2;
	public Sprite chosenSprite2;

	public SpriteRenderer top;
	public SpriteRenderer bottom;

	// Use this for initialization
	protected override void Start ()
    {
    	Randomize();
    	base.Start();
    }

	private void Randomize()
    {
        if (sprites.Length > 0)
        {
			int spriteIndex = Random.Range(0, sprites.Length);
            chosenSprite = sprites[spriteIndex];
            chosenSprite2= sprites2[spriteIndex];

            top.sprite = chosenSprite;
            bottom.sprite = chosenSprite2;
        }
    }
}