using UnityEngine;

public class EnemyManager :MonoBehaviour
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    public State currentState;
    public CharacterStats currentTarget;
    public bool isPerformingAction;


    [Header("A.I Setting")]
    public float detectionRadius = 20;
    public float minimumDetectionAngle = -50f;
    public float maximumDetectionAngle = 50f;

    public float currentRecoveryTime = 0;
    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Update()
    {
        HandleRecoveryTimer();
    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    public void HandleCurrentAction() 
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if (currentState != nextState && nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }


        //if (enemyLocomotionManager.currentTarget == null)
        //{
        //    enemyLocomotionManager.HandleDetection();
        //    return;
        //}

        //enemyLocomotionManager.distanceFromTarget = Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);

        //if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
        //{
        //    enemyLocomotionManager.HandleMoveToTarget();
        //}
        //else
        //{
        //    AttackTarget();
        //}
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }

    #region Attacks
    private void AttackTarget()
    {
        //if (isPerformingAction) return;

        //if (currentAttack == null)
        //{
        //    GetNewAttack();
        //}
        //else
        //{
        //    isPerformingAction = true;
        //    currentRecoveryTime = currentAttack.recoverTime;
        //    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        //    currentAttack = null;
        //}
    }

    private void GetNewAttack()
    {
        //if (currentAttack != null || isPerformingAction)
        //    return;

        //Vector3 targetDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
        //float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        //enemyLocomotionManager.distanceFromTarget = targetDirection.magnitude;

        //int maxScore = 0;

        //for (int i = 0; i < enemyAttacks.Length; i++)
        //{
        //    EnemyAttackAction attackAction = enemyAttacks[i];

        //    if (enemyLocomotionManager.distanceFromTarget < attackAction.minimumDistanceNeededToAttack
        //        || enemyLocomotionManager.distanceFromTarget > attackAction.maximumDistanceNeededToAttack)
        //    {
        //        continue;
        //    }

        //    if (viewableAngle > attackAction.maximumAttackAngle || viewableAngle < attackAction.minimumAttackAngle)
        //    {
        //        continue;
        //    }

        //    maxScore += attackAction.attackScore;
        //}

        //int randomValue = UnityEngine.Random.Range(0, maxScore);
        //int tempScore = 0;

        //for (int i = 0; i < enemyAttacks.Length; i++)
        //{
        //    EnemyAttackAction attackAction = enemyAttacks[i];

        //    if (enemyLocomotionManager.distanceFromTarget < attackAction.minimumDistanceNeededToAttack
        //        || enemyLocomotionManager.distanceFromTarget > attackAction.maximumDistanceNeededToAttack)
        //    {
        //        continue;
        //    }

        //    if (viewableAngle > attackAction.maximumAttackAngle || viewableAngle < attackAction.minimumAttackAngle)
        //    {
        //        continue;
        //    }

        //    tempScore += attackAction.attackScore;
        //    if(tempScore > randomValue)
        //    {
        //        currentAttack = attackAction;
        //        return;
        //    }
        //}
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
                isPerformingAction = false;
        }
    }
    #endregion
}


