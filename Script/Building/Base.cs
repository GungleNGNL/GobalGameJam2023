using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : EnergyNode
{
    protected override void Destroy()
    {
        GameManager.Instance.GameOver();
    }

    protected override void ResetStat()
    {
        m_Hp = m_MaxHp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, BuildManager.Instance.BaseSupplyRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BuildManager.Instance.EnergyNodeBuildDistance);
    }
}