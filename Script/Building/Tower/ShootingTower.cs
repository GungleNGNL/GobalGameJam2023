using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTower : Tower
{
    [SerializeField]
    protected Turret m_Turret;

    protected float m_LastAttackTime = 0;

    public override void PerformAttack(Actor target)
    {
        if (m_EnergySupporter.Count <= 0)
            return;

        if (Time.time - m_LastAttackTime >= attackDuration)
        {
            if (target == null)
                return;

            if (!target.gameObject.activeSelf)
            {
                m_Turret.Target = null;
                return;
            }
            m_Turret.Target = target.transform;
            m_Turret.Shoot();
            m_LastAttackTime = Time.time;
        }
    }
}
