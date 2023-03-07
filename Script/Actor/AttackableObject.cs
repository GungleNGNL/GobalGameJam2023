using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackableObject : EnergyNode, IAttackBehaviour
{
    [SerializeField]
    protected int attackPower;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float attackDuration;

    public UnityAction<Actor> beforeAttack;
    public UnityAction<Actor> afterAttack;

    public float AttackRange => attackRange;
    public float AttackDuration => attackDuration;

    protected override void Awake()
    {
        base.Awake();
        m_ActorEvent.OnDamage += OnDamage;
    }

    public virtual void Attack(Actor target)
    {
        if (target == null || !target.IsAlive)
            return;

        beforeAttack?.Invoke(target);

        target.Damage(this, attackPower);

        afterAttack?.Invoke(target);
    }

    protected override void Destroy()
    {
        ResetStat();
    }

    protected override void ResetStat()
    {
        m_Hp = m_MaxHp;
        gameObject.SetActive(false);
    }

    private void OnDamage(DamageSource damageSource)
    {
        //Debug.Log($"Take {damageSource.damage} damage from {damageSource.source.ActorName}");
    }

    public abstract void PerformAttack(Actor target);
}
