using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeExplosion : MonoBehaviour
{
    [SerializeField]
    protected float m_CastRadius;
    [SerializeField]
    protected LayerMask m_CastLayer;
    [SerializeField]
    protected float m_AttackDelay;

    protected AttackableObject m_Parent;

    public AttackableObject Parent
    {
        set { m_Parent = value; }
    }

    private void Start()
    {
        Invoke(nameof(Explosion), m_AttackDelay);
    }

    private void Explosion()
    {
        var objs = Physics.OverlapSphere(transform.position, m_CastRadius, m_CastLayer, QueryTriggerInteraction.UseGlobal);
        foreach (var obj in objs)
        {
            var actor = obj.transform.root.GetComponent<Actor>();
            if (actor == null)
                continue;
            m_Parent.Attack(actor);
        }
    }
}
