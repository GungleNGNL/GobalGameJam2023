using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;
    private static object m_Lock = new();
    public static T Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType(typeof(T)) as T;
                    if (m_Instance == null)
                    {
                        var obj = new GameObject();
                        obj.AddComponent<T>();
                    }                   
                }
                return m_Instance;
            }
        }
    }

    protected virtual void Awake()
    {
        //DontDestroyOnLoad(Instance.gameObject);
    }
}
