using UnityEngine;
using System.Collections;

public class FireWallObject : StaticGridObject {

    private bool destroyed = false;
    private float x;
    private float y;
    private float z;

    public virtual void Toggle()
    {
        if (gameObject.activeSelf)
        {
            destroyed = true;
            gameObject.SetActive(false);
        }
        else
        {
            destroyed = false;
            gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        x = this.gameObject.transform.position.x;
        y = this.gameObject.transform.position.y;
        z = this.gameObject.transform.position.z;

        destroyed = PlayerPrefsX.GetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
            + "pos x" + x + "pos y" + y + "pos z" + z + "firewall");

        if (destroyed)
        {
            gameObject.SetActive(false);
        }
        /*else
        {
            gameObject.SetActive(true);
        }*/

    }
    void OnDisable()
    {
        if (Globals.restartSaveState)
        {
            PlayerPrefsX.SetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
                  + "pos x" + x + "pos y" + y + "pos z" + z + "firewall", false);
        }
        else
        {
            PlayerPrefsX.SetBool("scene" + Application.loadedLevel + "loadedSlot" + Globals.loadedSlot
                  + "pos x" + x + "pos y" + y + "pos z" + z + "firewall", destroyed);
        }

    }
    /*
	void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGridObject>();
        animator = GetComponent<Animator>();
        if (!spawnsOnce) // Continuous spawn
        {
            StartCoroutine(spawnRandomDir());
        }
    }*/


}
