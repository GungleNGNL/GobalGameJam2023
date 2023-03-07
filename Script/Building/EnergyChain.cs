using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnergyChain : MonoBehaviour
{
    private EnergyNode m_Anode, m_Bnode;

    [SerializeField] private LineRenderer m_LineRender;
    
    public void SetLine(EnergyNode NodeA, EnergyNode NodeB)
    {
        m_Anode = NodeA;
        m_Bnode = NodeB;
        m_Anode.OnShutDown += OnShutDown;
        m_Bnode.OnShutDown += OnShutDown;
        m_LineRender.SetPosition(1, m_Anode.transform.position);
        m_LineRender.SetPosition(0, m_Bnode.transform.position);
    }

    private void OnShutDown(EnergyNode node)
    {
        if(node == m_Anode)
        {
            m_Bnode.OnShutDown -= OnShutDown;
        }
        else
        {
            m_Anode.OnShutDown -= OnShutDown;
        }
        Destroy(gameObject);
    }
}
