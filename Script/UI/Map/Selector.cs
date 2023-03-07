using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    [SerializeField] private Camera m_Cam;
    //private Grid m_Map;
    private HexGridXZ m_HexGridXZ;
    private GridHexMap m_GridHexMap;
    private BuildManager m_BuildManager; 
    [SerializeField] private Transform m_Transform;
    [SerializeField] private LayerMask m_TargetLayer;
    private MapActions m_Actions;
    private CameraControls m_CameraActions;
    [SerializeField] private GameObject m_GreenCurser, m_RedCurser;
    [SerializeField] Color m_Green, m_Red;
    [SerializeField] private GameObject[] m_TowerModel;
    [SerializeField] private Material m_ModelMatertial;
    private void Awake()
    {
        m_Actions = new();
        m_Actions.BuildActions.MouseMove.performed += _ => UpdatePosition();
        m_Actions.BuildActions.Select.performed += _ => Build();
    }

    private void Start()
    {
        m_CameraActions = CameraController.Instance.CameraActions;
        m_CameraActions.Camera.MoveCamera.performed += _ => UpdatePosition(); 
        m_BuildManager = BuildManager.Instance;
        m_Cam = Camera.main;
        //m_Map = m_BuildManager.Grid;
        m_GridHexMap = GridHexMap.Instance;
        m_HexGridXZ = m_GridHexMap.HexGridXZ;
        m_BuildManager.OnSelect += SelectTarget;
        m_BuildManager.OnSelect += () => CurserEnable(true);
        m_BuildManager.OnCancelSelect += () => CurserEnable(false);
        GameManager.Instance.OnGameOver += GameOver;
    }

    private void OnEnable()
    {
        m_Actions.Enable();
    }

    private void OnDisable()
    {
        m_Actions.Disable();
    }

    private void GameOver()
    {
        gameObject.SetActive(false);
    }

    [SerializeField] Vector3 m_MouseWorldPos;
    [SerializeField] Vector3 m_MousePos;
    [SerializeField] Vector2Int m_CellPos;
    bool m_CanBuild;
    private void UpdatePosition()
    {
        if (!m_BuildManager.OnSelected) return;
        m_MousePos = Mouse.current.position.ReadValue();
        m_MousePos.z = 0.3f;
        var ray = m_Cam.ScreenPointToRay(m_MousePos);
        RaycastHit hit;       
        if (Physics.Raycast(ray, out hit, 600.0f, m_TargetLayer))
        {
            CurserEnable(true);
            m_MouseWorldPos = hit.point;
        }
        else
        {
            Debug.LogError("Selector not inside the map");
        }
        m_MouseWorldPos.y = 0;
        //m_CellPos = m_Map.WorldToCell(m_MouseWorldPos);
        m_CellPos = m_HexGridXZ.WorldToCube(m_MouseWorldPos);
        //Vector3 cellPosWorld = m_Map.GetCellCenterWorld(m_CellPos);
        Vector3 cellPosWorld = m_HexGridXZ.CubeToWorld(m_CellPos);
        cellPosWorld.y = 0.1f;
        m_Transform.position = cellPosWorld;
        //m_CanBuild = m_BuildManager.CanBuild(cellPosWorld, m_CellPos);
        m_CanBuild = m_GridHexMap.CanBuild(cellPosWorld, m_BuildManager.GetCurrentCost(), m_BuildManager.BuildTarget);
        m_RedCurser.SetActive(!m_CanBuild);
        m_GreenCurser.SetActive(m_CanBuild);
        m_ModelMatertial.color = m_CanBuild ? m_Green : m_Red;
        m_TowerModel[(int)m_BuildManager.BuildTarget - 1].SetActive(true);
    }

    private void SelectTarget()
    {
        for (int i = 0; i < m_TowerModel.Length; i++)
        {
            m_TowerModel[i].SetActive(false);
        }
    }

    private void CurserEnable(bool isEnable)
    {
        m_Transform.gameObject.SetActive(isEnable);
    }

    private void Build()
    {
        if (EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseLeftId))
        {
            return;
        }
        if (!m_BuildManager.OnSelected || !m_Transform.gameObject.activeSelf) return;
        m_BuildManager.Build(m_HexGridXZ.CubeToWorld(m_CellPos));
    }
}
