using UnityEngine;
using System.Collections;

public class NaturalLight : StaticGridObject {

    public int radius;

    public float lightLevel;

    void OnEnable() {
        ChangeAdjacentLightLevel(1.0f / lightLevel);
    }

    void OnDisable() {
        ChangeAdjacentLightLevel(-1.0f / lightLevel);
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
