using UnityEngine;

public class EnemyAnimatorManager :AnimatorManager
{
    EnemyLocomotionManager enemyLocomotionManager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyLocomotionManager.rigidbody.drag = 0f;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0f;
        Vector3 velocity = deltaPosition / delta;
        enemyLocomotionManager.rigidbody.velocity = velocity;
    }
}


