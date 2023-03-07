using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionFSM : EnemyFSM
{
    protected override void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            enemyAttributes.PerformAttack(attackTarget.GetComponent<Actor>());
            ChangeState(FSMState.Dead);
        }
    }
}
