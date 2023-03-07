using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectionBtn : MonoBehaviour
{
    [SerializeField] private BuildingType m_Type;

    public void OnClick()
    {
        BuildManager.Instance.BuildTargetSelected(m_Type);
    }
}
