using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Boomerang : MonoBehaviour {

    //for final boss
    public bool evil = false;
    public static Dictionary<string, List<Vector3>> plants = new Dictionary<string, List<Vector3>>();

    public int damage = 1;
    public float speed = 1.0f;

    public AudioSource sfxThrow;

    private List<GameObject> itemHeld = new List<GameObject>();
    private int nextPlant = 0;
    private string roomId = null;

    public static string RoomId(Vector3 pos) {
        /*cavedungeon boss must be scene 7 in build settings*/
        if(Application.loadedLevel == 7)
        {
            return string.Format("cave", Application.loadedLevelName);
        }
        int x = Mathf.RoundToInt(pos.x / 14.0f) * 14;
        int y = Mathf.RoundToInt(pos.y / 10.0f) * 10;
        return string.Format("{0}{1:D5}{2:D5}", Application.loadedLevelName, x, y);
    }

    void OnEnable() {
        transform.position = transform.parent.position;
        roomId = RoomId(transform.parent.position);
        nextPlant = 0;
    }

    // Update is called once per frame
    void Update() {
        if (!Globals.canvas.dialogue) {
            if (nextPlant >= plants[roomId].Count) {
                nextPlant = plants[roomId].Count - 1;
            }

            if (Vector2.Distance(transform.position, plants[roomId][nextPlant]) <= speed) {
                // This boomerang is close enough to the next plant; change the
                // plant to go to
                int currPlant = nextPlant;
                nextPlant = (nextPlant + 1) % plants[roomId].Count;
                if (currPlant != nextPlant) {
                    sfxThrow.Play();
                }
            }
            else {
                Vector3 dt = (plants[roomId][nextPlant] - transform.position).normalized * speed;
                dt /= Globals.pixelSize;
                dt = new Vector3(Mathf.Round(dt.x), Mathf.Round(dt.y), 0);
                dt *= Globals.pixelSize;
                transform.position += dt;
                for (int i = itemHeld.Count - 1; i >= 0; --i) {
                    // a GameObject is equal to null if it is destroyed
                    if (itemHeld[i] == null) {
                        itemHeld.RemoveAt(i);
                    }
                    else {
                        itemHeld[i].transform.position += dt;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemySpawner") {
            other.gameObject.GetComponent<KillableGridObject>().TakeDamage(damage);
        }
        if (plants[roomId].Count > 1 && other.gameObject.tag == "Choppable") {
            Destroy(other.gameObject);
        }
        if (other.gameObject.GetComponent<ItemDrop>() != null) {
            itemHeld.Add(other.gameObject);
        }
        if(evil && other.gameObject.GetComponent<PlayerGridObject>())
        {
            other.gameObject.GetComponent<PlayerGridObject>().TakeDamage(1);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<ItemDrop>() != null) {
            itemHeld.Remove(other.gameObject);
        }
    }
}
