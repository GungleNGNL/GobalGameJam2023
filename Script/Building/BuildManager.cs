using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class BuildManager : MonoSingleton<BuildManager>
{
    [SerializeField] private GameObject m_Base;
    [SerializeField] private GameObject m_EnergyChain;
    public GameObject BaseInstance;
    //[SerializeField] private Grid m_Grid;
    [SerializeField] private GridHexMap m_GridHexMap;
    [SerializeField] private BuildingPool m_EnergyNodePool, m_RepairNodePool, m_GunTowerPool, m_LaserTowerPool, m_GrenadeTowerPool;
    [SerializeField] private IntVariable m_Energy;
    [SerializeField] private BuildingCost[] m_BuildingCosts;
    private Dictionary<BuildingType, BuildingCost> m_BuildingCostsDict = new();
    [SerializeField] private LayerMask m_NodeLayer;
    [SerializeField] private LayerMask m_TargetLayer; // both node and building layer
    [Header("Stat")]
    [SerializeField] private Vector2Int m_MapSize;
    public Vector2Int MapSize => m_MapSize;
    [SerializeField] private float m_EnergyNodeSupplyRange;
    public float EnergyNodeSupplyRange => m_EnergyNodeSupplyRange;
    [SerializeField] private float m_BaseSupplyRange;
    public float BaseSupplyRange { get { return m_BaseSupplyRange; } }
    [SerializeField] private float m_EnergyNodeBuildDistance;
    public float EnergyNodeBuildDistance => m_EnergyNodeBuildDistance;
    [Header("Build Selection")]
    [SerializeField] private bool m_OnSelected = false;
    public bool OnSelected { get { return m_OnSelected; } }
    public UnityAction OnSelect;
    public UnityAction OnCancelSelect;
    [SerializeField] private BuildingType m_BuildTarget;
    public BuildingType BuildTarget { get { return m_BuildTarget; } }
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.OnGameStart += BuildBase;
        for (int i = 0; i < m_BuildingCosts.Length; i++)
        {
            m_BuildingCostsDict.Add(m_BuildingCosts[i].Type, m_BuildingCosts[i]);
        }
    }
   
    private void BuildBase()
    {
        GameObject obj = Instantiate(m_Base);
        Vector3 pos = Vector3.zero;//m_Grid.CellToWorld(new Vector3Int(m_MapSize.x / 2 - 1, m_MapSize.y / 2 - 1, 0));
        obj.transform.position = pos;
        BaseInstance = obj;
    }

    public void BuildTargetSelected(BuildingType type)
    {
        m_OnSelected = true;
        m_BuildTarget = type;
        OnSelect?.Invoke();
    }

    public int GetCurrentCost()
    {
        return m_BuildingCostsDict[m_BuildTarget].Value;
    }

    public void Build(Vector3 cubeWorldPos)
    {
        //Vector3 pos = m_Grid.CellToWorld(cellPos);
        Vector3 pos = cubeWorldPos;
        bool canBuild = m_GridHexMap.CanBuild(pos, m_BuildingCostsDict[m_BuildTarget].Value, m_BuildTarget);
        if (!canBuild) return;
        GameObject target = null;
        switch (m_BuildTarget)
        {
            case BuildingType.EnergyNode:
                m_Energy.ApplyChange(-m_BuildingCostsDict[m_BuildTarget].Value);
                target = m_EnergyNodePool.Pool.Get();

                EnergyNode node;
                Debug.Log(target.TryGetComponent(out node));
                target.transform.position = pos;
                if (Vector3.Distance(pos, BaseInstance.transform.position) <= m_BaseSupplyRange)
                {
                    node.AddSupporter(BaseInstance.GetComponent<Base>());
                    var energyChain = Instantiate(m_EnergyChain);
                    energyChain.GetComponent<EnergyChain>().SetLine(node, BaseInstance.GetComponent<EnergyNode>());
                }
                Collider[] nodeInRange = Physics.OverlapSphere(pos, EnergyNodeSupplyRange, m_TargetLayer);
                Debug.Log(nodeInRange.Length);
                for (int i = 0; i < nodeInRange.Length; i++)
                {
                    if (nodeInRange[i].gameObject.tag == "base") continue;
                    if (nodeInRange[i].gameObject.tag == "supply")
                    {
                        node.AddSupporter(nodeInRange[i].GetComponent<EnergyNode>());
                        var energyChain = Instantiate(m_EnergyChain);
                        energyChain.GetComponent<EnergyChain>().SetLine(node, nodeInRange[i].GetComponent<EnergyNode>());
                    }
                    else
                    {
                        nodeInRange[i].gameObject.SendMessage("AddSupporter",node);
                    }
                }
                m_GridHexMap.SetEnergyNode(pos, true, 3);
                target.SetActive(true);
                m_OnSelected = false;
                OnCancelSelect?.Invoke();
                return;
            case BuildingType.RepairNode:
                m_Energy.ApplyChange(-m_BuildingCostsDict[m_BuildTarget].Value);
                target = m_RepairNodePool.Pool.Get();
                break;
            case BuildingType.GunTower:
                m_Energy.ApplyChange(-m_BuildingCostsDict[m_BuildTarget].Value);
                target = m_GunTowerPool.Pool.Get();
                break;
            case BuildingType.LaserTower:
                m_Energy.ApplyChange(-m_BuildingCostsDict[m_BuildTarget].Value);
                target = m_LaserTowerPool.Pool.Get();
                break;
            case BuildingType.GrenadeTower:
                m_Energy.ApplyChange(-m_BuildingCostsDict[m_BuildTarget].Value);
                target = m_GrenadeTowerPool.Pool.Get();
                break;
            default:
                Debug.LogError("Missing Selection");
                break;
        }
        target.transform.position = pos;
        target.SetActive(true);
        CheckTowerSupply(target, pos);
        m_OnSelected = false;
        OnCancelSelect?.Invoke();
        Debug.Log("Built");
    }

    private void CheckTowerSupply(GameObject tower, Vector3 pos)
    {
        if (Vector3.Distance(pos, BaseInstance.transform.position) <= m_BaseSupplyRange)
        {
            tower.SendMessage("AddSupporter", BaseInstance.GetComponent<Base>());
        }
        Collider[] nodeInRange = Physics.OverlapSphere(pos, m_EnergyNodeSupplyRange, m_NodeLayer);
        for (int i = 0; i < nodeInRange.Length; i++)
        {
            if (nodeInRange[i].tag == "base")
                continue;
            tower.SendMessage("AddSupporter", nodeInRange[i].GetComponent<EnergyNode>());
        }
    }
}

public enum BuildingType
{
    None = 0,
    EnergyNode,
    RepairNode,
    GunTower,
    LaserTower,
    GrenadeTower,
}
