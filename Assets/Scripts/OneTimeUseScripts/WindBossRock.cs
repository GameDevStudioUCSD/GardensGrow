using UnityEngine;
using System.Collections;

public class WindBossRock : MoveableGridObject {

    public bool isRolling = false;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        bombable = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	    if (isRolling) {
            Move(direction);
            Attack();
            FinishedAttack();
        }
	}

    public override void Move(Globals.Direction direction) {
        if (isRolling) {
            base.Move(direction);
        }
    }

    public void startRolling(Globals.Direction rollDirection) {
        direction = rollDirection;
        isRolling = true;
    }
}
