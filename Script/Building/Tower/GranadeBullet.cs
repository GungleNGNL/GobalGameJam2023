using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GranadeBullet : Bullet
{
    [SerializeField]
    protected float m_Time = 3f;
    [SerializeField]
    protected float m_Gravity = 9.8f;

    protected Vector3 m_speed;
    protected Vector3 m_CurrentGravity;
    protected float m_ElapsedTime;

    private void Reset()
    {
        m_CastLayer = LayerMask.GetMask("Map");
    }

    private void Start()
    {
        m_speed = new Vector3((m_Target.position.x - transform.position.x) / m_Time,
            (m_Target.position.y - transform.position.y) / m_Time - 0.5f * m_Gravity * m_Time, (m_Target.position.z - transform.position.z) / m_Time);

        m_CurrentGravity = Vector3.zero;
    }

    protected override void MoveToTarget()
    {
        m_CurrentGravity.y = m_Gravity * (m_ElapsedTime += Time.fixedDeltaTime);//v=at
        transform.Translate(m_speed * Time.fixedDeltaTime);
        transform.Translate(m_CurrentGravity * Time.fixedDeltaTime);
    } 

    protected override void SweepTarget()
    {
        if (transform.position.y > 0.1f) return;
        //if (!Physics.Raycast(transform.position, Vector3.down, 0.5f, m_CastLayer)) return;
        var obj = Instantiate(m_OnHitEffect, transform.position, Quaternion.identity);
        var explosion = obj.GetComponent<GranadeExplosion>();
        explosion.Parent = m_Parent;
       Destroy(gameObject);
    }

    protected override void PerformAttack(AttackableObject actor)
    {
        
    }
}
