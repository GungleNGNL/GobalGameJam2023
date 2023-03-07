using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BaseHP : MonoBehaviour
{
    [SerializeField] private Image m_HpBar;
    [SerializeField] GameObject m_Base;
    [SerializeField] private Actor core;
    private void Start()
    {
        StartCoroutine(GetBase());
    }

    IEnumerator GetBase()
    {
        yield return new WaitForEndOfFrame();
        m_Base = BuildManager.Instance.BaseInstance;
        core = m_Base.GetComponent<Actor>();
        core.actorEvent.OnDamage += Changed;
    }

    private void Changed(DamageSource source)
    {
        m_HpBar.fillAmount = (float)((float)core.Hp / (float)core.MaxHp);
    }
}
