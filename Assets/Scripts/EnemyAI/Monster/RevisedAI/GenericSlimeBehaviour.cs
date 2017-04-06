using UnityEngine;
using System.Collections;

public class GenericSlimeBehaviour : GenericMonsterBehaviour {

    public GameObject fireSlime;
    public GameObject windSlime;
    public GameObject ghostSlime;

    public bool fireDebugTrigger = false;
    public bool windDebugTrigger = false;
    public bool ghostDebugTrigger = false;

    protected override void Update() {
        base.Update();
        //Debug stuff so slime can change without touching another Slime
        if (fireDebugTrigger) {
            fireDebugTrigger = false;
            ChangeType(fireSlime);
        }
        if (windDebugTrigger) {
            windDebugTrigger = false;
            ChangeType(windSlime);
        }
        if (ghostDebugTrigger) {
            ghostDebugTrigger = false;
            ChangeType(ghostSlime);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<WindMonsterBehaviour>()) {
            ChangeType(windSlime);
        }
        else if (other.gameObject.GetComponent<GhostSlimeBehaviour>()) {
            ChangeType(ghostSlime);
        }
        else if (other.gameObject.GetComponent<GenericMonsterBehaviour>()) {
            ChangeType(fireSlime);
        }
    }

    private void ChangeType(GameObject newSlime) {
        GameObject replacement = Instantiate(newSlime);
        replacement.transform.position = transform.position;
        replacement.GetComponent<KillableGridObject>().health = this.health;
        replacement.GetComponentInChildren<PathFindingModule>().parameters.tileMap = pathFindingModule.parameters.tileMap;
        replacement.GetComponentInChildren<PathFindingModule>().parameters.target = pathFindingModule.parameters.target;
        Destroy(this.gameObject);
    }
}
