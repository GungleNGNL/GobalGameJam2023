using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : AttackableObject
{
    [SerializeField]
    protected int attackPowerPerLevel = 1;
    [SerializeField]
    protected float attackDurationPerLevel = 0.2f;

    protected int m_Level = 0;

    protected override void Start()
    {
        base.Start();
    }

    public override void AddSupporter(EnergyNode energyNode)
    {
        if (m_EnergySupporter.Contains(energyNode)) return;
        base.AddSupporter(energyNode);
        LevelUp();
        if (m_EnergySupporter.Count > 1) m_BuffEffect.SetActive(true);
    }

    protected override void RemoveSupporter(EnergyNode energyNode)
    {
        if (!m_EnergySupporter.Contains(energyNode)) return;
        m_EnergySupporter.Remove(energyNode);
        LevelDown();
        if (m_EnergySupporter.Count <= 1) m_BuffEffect.SetActive(false);
        if (m_EnergySupporter.Count <= 0) StartCoroutine(ShutDown());
    }

    protected override IEnumerator ShutDown()
    {
        yield return new WaitForSeconds(0.3f);
    }

    protected void LevelUp()
    {
        m_Level++;
        attackPower += attackPowerPerLevel;
        attackDuration -= attackDurationPerLevel;
    }

    protected void LevelDown()
    {
        m_Level = Mathf.Clamp(m_Level - 1, 0, int.MaxValue);
        attackPower -= attackPowerPerLevel;
        attackDuration += attackDurationPerLevel;
    }
}
