using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float m_Speed = 5f;
    [SerializeField]
    protected Vector3 m_MovementOffset;
    [SerializeField]
    protected float m_CastRadius = 1f;
    [SerializeField]
    protected LayerMask m_CastLayer;
    [SerializeField]
    protected GameObject m_OnHitEffect;
    [SerializeField]
    protected bool m_DestroyOnHit;

    protected AttackableObject m_Parent;
    protected int m_Damage;
    protected Transform m_Target;
    protected Vector3 m_Direction;
    public Vector3 Direction
    {
        set
        {
            m_Direction = value;
        }
    }

    private void Start()
    {
        Invoke(nameof(Destroy) , 5.0f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public AttackableObject Parent
    {
        set
        {
            m_Parent = value;
        }
    }

    public int Damage
    {
        get { return m_Damage; }
        set { m_Damage = value; }
    }

    public Transform Target
    {
        set 
        { 
            m_Target = value;
            //m_Direction = CalculateLaunchDirection();
        }
    }

    protected virtual Vector3 CalculateLaunchDirection()
    {
        return (m_Target.position - transform.position + m_MovementOffset).normalized;
    }

    protected virtual void MoveToTarget()
    {
        transform.Translate(m_Speed * Time.deltaTime * m_Direction);
    }

    protected virtual void SweepTarget()
    {
        if (m_Parent == null)
            return;
        if(Physics.CheckSphere(transform.position, m_CastRadius, m_CastLayer, QueryTriggerInteraction.UseGlobal))
        {
            var objs = Physics.OverlapSphere(transform.position, m_CastRadius, m_CastLayer, QueryTriggerInteraction.UseGlobal);
            foreach (var obj in objs)
            {
                var actor = obj.transform.root.GetComponent<AttackableObject>();
                if (actor == null)
                    return;

                PerformAttack(actor);

                OnAfterAttack();
            }
        }
    }

    protected virtual void PerformAttack(AttackableObject actor)
    {
        if(m_Parent != null)
            m_Parent.Attack(actor);
    }

    protected virtual void OnAfterAttack()
    {
        if (m_OnHitEffect != null)
            Instantiate(m_OnHitEffect, transform.position, Quaternion.identity);

        if (m_DestroyOnHit)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        MoveToTarget();
        SweepTarget();
    }
}
