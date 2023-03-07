using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerTower : ShootingTower
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
