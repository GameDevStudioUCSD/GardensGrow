﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Invokes all callback functions when a trigger collider 2D
/// attached to this object is entered.
/// 
/// Can only be triggered by the player (hardcoded for now).
/// 
/// Add callbacks in the Unity Editor.
/// </summary>
public class OnTriggerEnterEvent : MonoBehaviour {

    [Header("Event Parameters")]
    public bool triggerOnlyOnce;

    public UnityEvent callBacks;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        callBacks.Invoke();

        if (triggerOnlyOnce)
            this.enabled = false;
    }
}
