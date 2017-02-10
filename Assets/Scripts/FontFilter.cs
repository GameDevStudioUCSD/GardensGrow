using UnityEngine;
using System.Collections;

public class FontFilter : MonoBehaviour {

    public Font[] fonts;

    // Use this for initialization
    void Start () {
        foreach (Font font in fonts) {
            font.material.mainTexture.filterMode = FilterMode.Point;
        }
    }

    // Update is called once per frame
    void Update () {

    }
}
