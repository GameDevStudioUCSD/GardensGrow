using UnityEngine;
using System.Collections;

public class FireWallObject : StaticGridObject {

	public virtual void Disable() {
        gameObject.SetActive(false);
    }
}
