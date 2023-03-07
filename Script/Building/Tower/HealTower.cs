using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTower : Tower
{
    [SerializeField]
    protected float m_HealRadius = 200f;
    [SerializeField]
    protected float m_HealDuration = 0.2f;
    [SerializeField]
    protected int m_HealAmount = 1;

    private float m_LastHealingTime = 0f;

    public override void PerformAttack(Actor target)
    {
        if (m_EnergySupporter.Count <= 0)
            return;
        if (Time.time - m_LastHealingTime >= attackDuration)
        {
            if (target == null)
                return;

            if (!target.gameObject.activeSelf)
            {
                return;
            }
            target.Heal(m_HealAmount);
            m_LastHealingTime = Time.time;
        }
    }
}
