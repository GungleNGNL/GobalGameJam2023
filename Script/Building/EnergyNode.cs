using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyNode : Actor
{
    [SerializeField] private Transform m_CoreTransform;
    [SerializeField] protected List<EnergyNode> m_EnergySupporter = new();
    [SerializeField] private LineRenderer m_ChainObject;
    public UnityAction<EnergyNode> OnShutDown;
    public UnityAction<EnergyNode> OnRemove;

    public Transform CoreTransform { get { return m_CoreTransform; } }
    public LineRenderer ChainObject { get { return m_ChainObject; } }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual IEnumerator ShutDown()
    {
        yield return new WaitForSeconds(0.3f);
        OnShutDown?.Invoke(this);
        ResetStat();
    }

    private void ShutDownNow()
    {
        OnShutDown?.Invoke(this);
        ResetStat();
    }

    public virtual void AddSupporter(EnergyNode energyNode)
    {
        energyNode.OnShutDown += RemoveSupporter;
        m_EnergySupporter.Add(energyNode);
    }

    protected virtual void RemoveSupporter(EnergyNode energyNode)
    {
        m_EnergySupporter.Remove(energyNode);
        OnRemove?.Invoke(energyNode);
        if (m_EnergySupporter.Count == 0) StartCoroutine(ShutDown());
    }

    protected override void Destroy()
    {
        if (m_OnDieEffect != null)
            Instantiate(m_OnDieEffect, transform.position, Quaternion.identity);
        GridHexMap.Instance.SetEnergyNode(transform.position, false, 3);
        ShutDownNow();
    }

    protected override void ResetStat()
    {
        base.ResetStat();
        m_EnergySupporter.Clear();
        gameObject.SetActive(false);
        OnShutDown = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, BuildManager.Instance.EnergyNodeSupplyRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BuildManager.Instance.EnergyNodeBuildDistance);
    }
}
