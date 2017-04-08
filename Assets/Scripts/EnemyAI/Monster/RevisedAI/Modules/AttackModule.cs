using System;
using UnityEngine;

public abstract class AttackModule : MonoBehaviour {

    [Serializable]
    public class AttackAbstractParameters
    {
        [Header("Required Component")]
        [Tooltip("Creature that is attacking.")]
        public EnemyGridObject creature;
    }
}