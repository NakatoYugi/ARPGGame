using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].GetComponent<CharacterStats>();

            if (characterStats == null) return this;

            Vector3 targetDirection = characterStats.transform.position - transform.position;
            float viewAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewAngle > enemyManager.minimumDetectionAngle && viewAngle < enemyManager.maximumDetectionAngle)
            {
                enemyManager.currentTarget = characterStats;
                return pursueTargetState;
            }
        }

        return this;
    }
}


