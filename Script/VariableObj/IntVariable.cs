using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new integer", menuName = "Variable/Integer")]
[Serializable]
public class IntVariable : ScriptableObject
{
    [SerializeField] protected int m_Value;
    public int Value 
    {   
        get 
        {
            return m_Value; 
        } 
        set 
        {
            m_Value = value;
            OnValueChange?.Invoke();
        } 
    }
    public UnityAction OnValueChange;
    public void SetValue(int value)
    {
        Value = value;
    }

    public void SetValue(IntVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(int amount)
    {
        Value += amount;
    }

    public void ApplyChange(IntVariable amount)
    {
        Value += amount.Value;
    }
}
