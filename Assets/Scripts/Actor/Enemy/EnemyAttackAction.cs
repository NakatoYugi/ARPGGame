﻿using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
public class EnemyAttackAction :EnemyAction
{
    public int attackScore = 3;
    public float recoverTime = 2f;

    public float maximumAttackAngle = 35f;
    public float minimumAttackAngle = -35f;

    public float minimumDistanceNeededToAttack = 0f;
    public float maximumDistanceNeededToAttack = 3f;
}


