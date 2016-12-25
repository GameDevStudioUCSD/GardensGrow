using UnityEngine;
using System.Collections.Generic;

public class PlatformTrigger : MonoBehaviour
{
    private int nonLavaObjects = 0;
    public bool isTriggered = false;

    void Update()
    {
        if (nonLavaObjects == 0) isTriggered = false;
        else isTriggered = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((!other.isTrigger || other.CompareTag("Ground")) && !other.CompareTag("Lava") && !other.CompareTag("Player") && other.isActiveAndEnabled)
        {
            nonLavaObjects++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((!other.isTrigger || other.CompareTag("Ground")) && !other.CompareTag("Lava") && !other.CompareTag("Player") && !other.isActiveAndEnabled)
        {
            nonLavaObjects--;
        }
    }

}
