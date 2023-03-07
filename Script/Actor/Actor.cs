using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    [SerializeField]
    protected string m_ActorName;
    [SerializeField]
    protected int m_MaxHp;
    [SerializeField]
    protected int m_Hp;
    [SerializeField]
    protected GameObject m_OnDieEffect;
    [SerializeField]
    protected GameObject m_HealingEffect;
    [SerializeField]
    protected GameObject m_BuffEffect;

    protected ActorEvent m_ActorEvent;
    //protected List<Actor> m_HealingObjs;

    public string ActorName => m_ActorName;
    public int MaxHp => m_MaxHp;
    public int Hp => m_Hp;
    public bool IsAlive => m_Hp > 0;
    public ActorEvent actorEvent => m_ActorEvent;
    protected virtual void Awake()
    {
        m_ActorEvent = new ActorEvent();
        if (m_HealingEffect)
            m_HealingEffect.SetActive(false);
        if (m_BuffEffect)
            m_BuffEffect.SetActive(false);
        ResetStat();
    }

    protected virtual void Destroy()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void ResetStat()
    {
        m_Hp = m_MaxHp;
    }


    public void Damage(Actor source, int damage)
    {
        if (damage <= 0 || !IsAlive) 
            return;

        m_Hp = Mathf.Clamp(m_Hp - damage, 0, m_MaxHp);
        if (m_Hp > 0) 
            actorEvent.OnDamage?.Invoke(new DamageSource(source, damage));
        else
        {
            actorEvent.OnDie?.Invoke(this);
            Debug.Log("EnemyDead");
            Destroy();
        }
    }

    public void Heal(int amount)
    {
        m_Hp = Mathf.Clamp(m_Hp + amount, 0, m_MaxHp);
        StartCoroutine(StartActiveEffect());
    }

    IEnumerator StartActiveEffect() 
    {
        m_HealingEffect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        m_HealingEffect.SetActive(false);
    }
}
