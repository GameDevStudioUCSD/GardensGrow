using UnityEngine;
using System.Collections;

public class BasicAttackModule : BasicAttackAbstractFSM {

    [Header("Attacking Components")]
    public EnemyGridObject creature;

    protected override void ExecuteActionAttack()
    {
        throw new System.NotImplementedException();
    }
}
