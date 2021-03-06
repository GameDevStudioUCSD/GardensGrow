﻿using UnityEngine;
using System.Collections;

public class LightSource : MonoBehaviour {

    public int radius;
    public float lightLevel;
    public bool belongsToRoom = false;
    private bool finished = false;

    public void OnEnable() {
        /*
         * Light up after first update has been called to make sure every tile
         * is initialized beforehand
         */
        StartCoroutine(LateInit());
    }

    void OnDestroy()
    {
        OnDisable();
    }
    public void OnDisable()
    {
        if (!finished) {
            ChangeAdjacentLightLevel(-1.0f / lightLevel);
            finished = true;
        }
    }

    private IEnumerator LateInit() {
        /*
         * this will continue executing after all Update functions has been
         * called on the next frame.
         */
        yield return null;
        ChangeAdjacentLightLevel(1.0f / lightLevel);
    }

    private void ChangeAdjacentLightLevel(float amount) {
        Vector2 tilePos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        for (int i = 1; i <= radius; i += 1) {
            Vector2 offset = new Vector2(i, i);
            Collider2D[] colliders = Physics2D.OverlapAreaAll(tilePos - offset, tilePos + offset);
            foreach (Collider2D collider in colliders) {
                LightLevel ll = collider.GetComponent<LightLevel>();
                if (ll != null) {
                    ll.ChangeLightLevel(amount);
                }
            }
        }
    }
}
