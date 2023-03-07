using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerTowerFSM : TowerFSM
{
    private GunnerTower m_GunnerTower;
    protected override AttackableObject MyAttributes() => m_GunnerTower;
    private void Awake()
    {
        StateReset();
        m_GunnerTower = GetComponent<GunnerTower>();
    }
}
