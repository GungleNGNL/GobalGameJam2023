using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class IntVariableDisplay : MonoBehaviour
{
    [SerializeField] private IntVariable m_Variable;
    [SerializeField] private TextMeshProUGUI m_TextUI;

    private void Awake()
    {
        m_Variable.OnValueChange += UpdateData;
        UpdateData();
    }

    private void UpdateData()
    {
        m_TextUI.text = m_Variable.Value.ToString();
    }
}
