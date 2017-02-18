using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// When an object with this script is destroyed, 
/// all the callback functions will be invoked.
/// 
/// Add callback functions through the Unity editor.
/// </summary>
public class OnDestroyEvent : MonoBehaviour {

    public UnityEvent callBacks;

    public void OnDestroy()
    {
        callBacks.Invoke();
    }
}
