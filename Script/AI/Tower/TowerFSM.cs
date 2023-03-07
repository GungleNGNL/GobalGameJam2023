using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFSM : FSM
{
    private Tower tower;
    protected override AttackableObject MyAttributes() => tower;
    private void Awake()
    {
        StateReset();
        tower = GetComponent<Tower>();
    }

    protected override void Attack()
    {
        //base.Attack();
        if (!attackTarget.activeSelf || attackTarget == null)
        {
            attackTarget = null;
            ChangeState(FSMState.BaseAction);
            return;
        }
        MyAttributes().PerformAttack(attackTarget.GetComponent<Actor>());
    }
}
