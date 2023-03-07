using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    protected Transform m_BulletSpawnPoint;
    [SerializeField]
    protected GameObject m_Bullet;
    [SerializeField]
    protected Transform m_Target;
    [SerializeField]
    protected Vector3 m_TargetOffset;
    [SerializeField]
    protected float m_RotationSpeed = 5f;
    [SerializeField]
    protected bool m_IsNeedRotation = true;

    protected bool m_RequestShoot = false;

    public Transform BulletSpawnPoint => m_BulletSpawnPoint;
    public GameObject BulletObject => m_Bullet;
    public Transform Target
    {
        get
        {
            return m_Target;
        }

        set
        {
            if (value != null)
                m_Target = value;
        }
    }

    public void Shoot() => m_RequestShoot = true;

    protected virtual void PerformRotation()
    {
        var dir = (m_Target.position - transform.position + m_TargetOffset).normalized;
        var lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.fixedDeltaTime * m_RotationSpeed);
    }

    protected virtual void PerformShooting()
    {
        var obj = Instantiate(m_Bullet, m_BulletSpawnPoint.transform.position, Quaternion.identity);
        var bullet = obj.GetComponent<Bullet>();
        bullet.Parent = GetComponentInParent<AttackableObject>();
        bullet.Target = m_Target;
        bullet.Direction = transform.forward;
    }

    private void FixedUpdate()
    {
        if (m_Target == null)
            return;

        if (m_IsNeedRotation)
            PerformRotation();

        if (m_RequestShoot)
        {
            PerformShooting();
            m_RequestShoot = false;
        }          
    }
}
