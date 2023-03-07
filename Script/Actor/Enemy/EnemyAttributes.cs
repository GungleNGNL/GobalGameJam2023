using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Melee,
    Range,
    Explosion,
}
public class EnemyAttributes : AttackableObject
{
    [SerializeField] protected IntVariable m_Energy;
    [SerializeField] protected float detectRange = 180f;
    [SerializeField] protected float movingSpeed = 40;
    [SerializeField] protected float rotSpeed = 120;
    [SerializeField] protected int funding = 30;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform bulletSpawnPos;
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected LayerMask enemyLayer;
    public LayerMask EnemyLayer() => enemyLayer;
    public float DetectRange() => detectRange;
    public float MovingSpeed() => movingSpeed;
    public float RotSpeed() => rotSpeed;

    protected override void Awake()
    {
        base.Awake();
        m_ActorEvent.OnDie += ApplyEnergy;
    }

    public override void PerformAttack(Actor target)
    {
        //throw new System.NotImplementedException();
        if(enemyType == EnemyType.Melee)
        {
            Attack(target);
        }
        else if(enemyType == EnemyType.Range)
        {
            if (bullet == null) return;
            var spawnPos = bulletSpawnPos == null ? transform : bulletSpawnPos;
            var o = Instantiate(bullet, spawnPos.position, Quaternion.identity);
            var b = o.GetComponent<Bullet>();
            b.Parent = GetComponent<AttackableObject>();
            b.Target = target.transform;
            b.Direction = (target.transform.position - b.transform.position).normalized;
        }
        else
        {
            if (m_OnDieEffect != null)
                Instantiate(m_OnDieEffect, transform.position, transform.rotation);
            Damage(this, m_Hp);
        }
    }

    private void ApplyEnergy(Actor a)
    {
        m_Energy.ApplyChange(funding);
    }
}
