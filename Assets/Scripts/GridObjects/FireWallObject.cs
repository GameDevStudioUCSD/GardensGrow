using UnityEngine;
using System.Collections;

public class FireWallObject : StaticGridObject {

	public virtual void Toggle() {
        if (gameObject.activeSelf) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
