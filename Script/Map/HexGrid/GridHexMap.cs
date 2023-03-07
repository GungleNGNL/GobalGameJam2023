using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHexMap : MonoSingleton<GridHexMap>
{   
    [SerializeField] private int m_MapSize; // radius
    private GameObject[,] m_GridObject;
    private MapData[,] m_MapData;
    private HexGridXZ m_HexGridXZ;
    public HexGridXZ HexGridXZ { get { return m_HexGridXZ; } }
    [SerializeField] private GameObject m_Hexagon;
    [SerializeField] private Vector2 m_CellSize;
    [SerializeField] private IntVariable m_Energy;

    protected override void Awake()
    {
        //m_MapSize = BuildManager.Instance.MapSize.x;
        m_HexGridXZ = new(Vector3.zero, m_CellSize);
        int dataSize = m_MapSize * 2 + 1;
        m_GridObject = new GameObject[dataSize, dataSize]; // radius * 2 + centre
        m_MapData = new MapData[dataSize, dataSize];
        for(int i = 0; i < m_MapData.GetLength(0); i++)
        {
            for (int j = 0; j < m_MapData.GetLength(1); j++)
                m_MapData[i, j] = new();
        }
        SetEnergyNode(Vector3.zero, true, 5);
    }

    public bool CanBuild(Vector3 pos, int cost, BuildingType type)
    {
        if (cost > m_Energy.Value) return false;
        Vector2Int des = m_HexGridXZ.WorldToCube(pos);
        if (des.x < -m_MapSize || des.y < -m_MapSize || des.y > m_MapSize || des.x > m_MapSize)
        {
            return false;
        }
        Vector2Int[] area = m_HexGridXZ.GetCubeArea(des, 2);
        if(type == BuildingType.EnergyNode)
        {
            for(int i = 0; i < area.Length; i++)
            {
                if (m_MapData[area[i].y + m_MapSize, area[i].x + m_MapSize].Type == BuildingType.EnergyNode)
                    return false;
            }
        }      
        MapData data = m_MapData[des.y + m_MapSize, des.x + m_MapSize];
        return (data.PowerLevel > 0 && data.Type == 0);
    }

    public void SetEnergyNode(Vector3 pos, bool isActive, int range)
    {
        Vector2Int centre = m_HexGridXZ.WorldToCube(pos);
        m_MapData[centre.y + m_MapSize, centre.x + m_MapSize].Type = isActive? BuildingType.EnergyNode : 0;
        Vector2Int[] area = m_HexGridXZ.GetCubeArea(centre, range);
        for(int i = 0; i < area.Length; i++)
        {
            Vector2Int des = area[i];
            if (isActive)
            {
                ActiveGridObject(des);
                m_MapData[des.y + m_MapSize, des.x + m_MapSize].PowerLevel++;
            }
            else
            {
                m_MapData[des.y + m_MapSize, des.x + m_MapSize].PowerLevel--;
                if(m_MapData[des.y + m_MapSize, des.x + m_MapSize].PowerLevel <= 0)
                    InactiveGridObject(des);
            }
        }
    }

    private void ActiveGridObject(Vector2Int des)
    {
        GameObject obj = m_GridObject[des.y + m_MapSize, des.x + m_MapSize]; 
        if(obj == null)
        {
            SpawnGrid(des);
        }
        else
        {
            obj.SetActive(true);
        }
    }

    private void InactiveGridObject(Vector2Int des)
    {
        GameObject obj = m_GridObject[des.y + m_MapSize, des.x + m_MapSize];
        StartCoroutine(obj.GetComponent<GridController>().DisableObject());
    }

    private void SpawnGrid(Vector2Int des)
    {
        GameObject obj = Instantiate(m_Hexagon, m_HexGridXZ.CubeToWorld(des.x, des.y), Quaternion.identity);
        m_GridObject[des.y + m_MapSize, des.x + m_MapSize] = obj;
        obj.SetActive(true);
    }
}

public class MapData
{
    public int PowerLevel = 0;
    public BuildingType Type = 0;
    public MapData()
    {

    }
}
