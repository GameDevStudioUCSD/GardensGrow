using UnityEngine;
using System.Collections.Generic;

public class PlatformTrigger : MonoBehaviour
{
    private int nonLavaObjects = 0;
    public bool isTriggered = false;
    private List<KillableGridObject> killList = new List<KillableGridObject>();

    void Update()
    {
        foreach (KillableGridObject target in killList)
        {
            if (target == null || target.isDying)
            {
                killList.Remove(target);
                nonLavaObjects--;
                break;
            }
        }
        killList.RemoveAll((KillableGridObject target) => target == null);
        if (nonLavaObjects <= 0) isTriggered = false;
        else isTriggered = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((!other.isTrigger || other.CompareTag("Ground")) && !other.CompareTag("Lava") && !other.CompareTag("Player") && other.isActiveAndEnabled)
        {
            nonLavaObjects++;
        }
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemySpawner"))
        {
            KillableGridObject enemy = other.GetComponentInParent<KillableGridObject>();
            killList.Add(enemy);
            enemy.TakeDamage(GetComponentInParent<PlatformGridObject>().damage);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((!other.isTrigger || other.CompareTag("Ground")) && !other.CompareTag("Lava") && !other.CompareTag("Player") && !other.isActiveAndEnabled)
        {
            nonLavaObjects--;
            KillableGridObject killable = other.GetComponent<KillableGridObject>();
            if (killable && killList.Contains(killable))
            {
                killList.Remove(killable);
            }
        }
    }

}
