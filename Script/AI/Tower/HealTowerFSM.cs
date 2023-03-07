using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTowerFSM : TowerFSM
{
    protected override AttackableObject MyAttributes() => GetComponent<HealTower>();

    protected override void ScanTaget()
    {
        if (Physics.CheckSphere(transform.position, ScanRange(), EnemyLayer))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, ScanRange(), EnemyLayer);
            float hpRate = 0;
            foreach (Collider hit in hits)
            {
                if (!hit.gameObject.activeSelf)
                    continue;
                var actor = hit.transform.root.GetComponent<Actor>();
                if (actor != null)
                {
                    if (actor.Hp >= actor.MaxHp) continue;
                    float healthRate = actor.Hp / actor.MaxHp;
                    if (healthRate <= hpRate || hpRate == 0)
                    {
                        hpRate = actor.Hp;
                        attackTarget = hit.transform.root.gameObject;
                    }
                }
            }
        }
        if (attackTarget != null)
        {
            ChangeState(AfterScanState());
        }
    }

    protected override void Attack() // Heal
    {
        var actor = attackTarget.GetComponent<Actor>();
        if (!attackTarget.activeSelf || attackTarget == null || actor.Hp >= actor.MaxHp)
        {
            attackTarget = null;
            ChangeState(FSMState.BaseAction);
            return;
        }
        MyAttributes().PerformAttack(actor);
    }
}
