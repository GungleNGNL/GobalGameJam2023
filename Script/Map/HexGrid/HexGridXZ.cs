using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridXZ
{
    private Vector3 m_StartWorldPos; //middle
    private Vector2 m_CellSize;

    public HexGridXZ(Vector3 startPos, Vector2 cellSize)
    {
        m_StartWorldPos = startPos;
        m_CellSize = cellSize;
    }

    public Vector3 CubeToWorld(Vector2Int pos) //q r s
    {
        float x = m_CellSize.x * (pos.x + (pos.y / 2f));
        float y = 0.1f;
        float z = m_CellSize.y * pos.y * 0.75f;
        return new Vector3(x, y, z) + m_StartWorldPos;
    }

    public Vector3 CubeToWorld(int q, int r)
    {
        float x = m_CellSize.x * (q + (r / 2f));
        float y = 0.1f;
        float z = m_CellSize.y * r * 0.75f;
        return new Vector3(x, y, z) + m_StartWorldPos;
    }

    public Vector2Int WorldToCube(Vector3 pos)
    {
        Vector3 offset = pos - m_StartWorldPos;
        int r = Mathf.RoundToInt(offset.z / m_CellSize.y / 0.75f);
        int q = Mathf.RoundToInt((offset.x / m_CellSize.x) - (r / 2f));
        return new Vector2Int(q, r);
    }

    public Vector3[] GetArea(Vector3 pos, int range)
    {
        Vector2Int centre = WorldToCube(pos);
        List<Vector3> result = new();
        for(int q = -range; q <= range; q++)
        {
            for(int r = Mathf.Max(-range, -q -range); r <= Mathf.Min(range, -q + range); r++)
            {
                result.Add(CubeToWorld(q + centre.x, r + centre.y));
            }
        }
        return result.ToArray();
    }

    public Vector2Int[] GetCubeArea(Vector2Int pos, int range)
    {
        List<Vector2Int> result = new();
        for (int q = -range; q <= range; q++)
        {
            for (int r = Mathf.Max(-range, -q - range); r <= Mathf.Min(range, -q + range); r++)
            {
                result.Add(new Vector2Int(q, r) + pos);
            }
        }
        return result.ToArray();
    }
}
