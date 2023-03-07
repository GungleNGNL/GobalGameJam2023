using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField]
    protected LineRenderer m_LineRenderer;
    [SerializeField]
    protected Vector3 m_StartPositionOffset;
    [SerializeField]
    protected Vector3 m_EndPositionOffset;
    [SerializeField]
    protected GameObject m_HitEffect;

    [Header("Only for test")]
    [SerializeField]
    protected GameObject target;

    private GameObject m_HitEffectInstance;
    private Actor m_Target;
    private float m_LastAttackTime = 0f;

    public override void PerformAttack(Actor target)
    {
        if (m_EnergySupporter.Count <= 0)
            return;

        if (target == null)
            return;

        if (!target.gameObject.activeSelf)
        {
            m_Target = null;
            OnTargetDie(target);
            return;
        }

        if (m_Target != target)
            target.actorEvent.OnDie += OnTargetDie;

        m_Target = target;

        m_LineRenderer.positionCount = 2;
        m_LineRenderer.SetPosition(0, transform.position + m_StartPositionOffset);
        m_LineRenderer.SetPosition(1, target.transform.position + m_EndPositionOffset);
        m_LineRenderer.gameObject.SetActive(true);

        if (m_LastAttackTime <= 0f || Time.time - m_LastAttackTime >= attackDuration)
        {
            Attack(target);
            m_LastAttackTime = Time.time;
        }

        if (m_HitEffectInstance == null)
        {
            m_HitEffectInstance = Instantiate(m_HitEffect, target.transform.position + m_EndPositionOffset, Quaternion.identity);
        }

        m_HitEffectInstance.transform.position = target.transform.position + m_EndPositionOffset;
    }

    protected virtual void OnTargetDie(Actor actor)
    {
        Destroy(m_HitEffectInstance);
        m_LineRenderer.gameObject.SetActive(false);
    }

    //Only for test
    protected override void Update()
    {
        base.Update();
        //PerformAttack(target.GetComponent<Actor>());
    }
}
